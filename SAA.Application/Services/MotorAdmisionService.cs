using Microsoft.EntityFrameworkCore;
using SAA.Application.DTOs;
using SAA.Application.Interfaces;
using SAA.Domain.Entities;

namespace SAA.Application.Services;

public class MotorAdmisionService
{
    private readonly IApplicationDbContext _context;

    public MotorAdmisionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarExamenAsync(RegistrarExamenDto dto)
    {
        var postulante = await _context.Postulantes.FindAsync(dto.IdPostulante);
        if (postulante == null)
            throw new Exception("Postulante no encontrado.");

        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = null;
        try { transaction = await _context.Database.BeginTransactionAsync(); } catch (InvalidOperationException) { /* Ignored for InMemory DB */ }
        
        try
        {
            var examen = new ExamenAdmision
            {
                IdPostulante = postulante.IdPostulante,
                Puntaje = dto.Puntaje,
                Observaciones = dto.Observaciones,
                FechaExamen = DateTime.Now,
                FechaCreacion = DateTime.Now
            };

            _context.ExamenesAdmision.Add(examen);
            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();
        }
        catch
        {
            if (transaction != null) await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            if (transaction != null) await transaction.DisposeAsync();
        }
    }

    public async Task ProcesarResultadosAsync()
    {
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = null;
        try { transaction = await _context.Database.BeginTransactionAsync(); } catch (InvalidOperationException) { /* Ignored for InMemory DB */ }
        
        try
        {
            var previousResultados = await _context.ResultadosAdmision.ToListAsync();
            _context.ResultadosAdmision.RemoveRange(previousResultados);
            await _context.SaveChangesAsync();

            var examenes = await _context.ExamenesAdmision
                .ToListAsync();

            var idsFichas = examenes.Select(e => e.IdFichaPostulacion).Distinct().ToList();
            var fichas = await _context.FichasPostulacion
                .Where(f => idsFichas.Contains(f.IdFichaPostulacion))
                .ToListAsync();

            var programas = await _context.ProgramasAcademicos.ToListAsync();
            var postulantesAsignados = new System.Collections.Generic.HashSet<int>();

            foreach (var prog in programas)
            {
                var fichasPrograma = fichas.Where(f => f.IdProgramaAcademico == prog.IdProgramaAcademico).ToList();
                var examenesPrograma = examenes.Where(e => fichasPrograma.Any(f => f.IdFichaPostulacion == e.IdFichaPostulacion))
                                               .OrderByDescending(e => e.Puntaje)
                                               .ToList();

                int cuposAsignados = 0;
                int ordenMeritoActual = 1;

                foreach (var examen in examenesPrograma)
                {
                    string estadoFinal = "Desaprobado";
                if (postulantesAsignados.Contains(examen.IdPostulante)) continue;

                    if (examen.Puntaje >= 50.0m)
                    {
                        if (cuposAsignados < (prog.Vacantes ?? 0))
                        {
                            estadoFinal = "Ingresante";
                            cuposAsignados++;
                        }
                        else
                        {
                            estadoFinal = "Aprobado";
                        }
                    }

                    var resultado = new ResultadoAdmision
                    {
                        IdPostulante = examen.IdPostulante,
                        IdProgramaAcademico = prog.IdProgramaAcademico,
                        Calificacion = examen.Puntaje,
                        Resultado = estadoFinal,
                        OrdenMerito = ordenMeritoActual,
                        FechaResultado = DateTime.Now
                    };

                    _context.ResultadosAdmision.Add(resultado);
                postulantesAsignados.Add(examen.IdPostulante);
                    ordenMeritoActual++;
                }
            }

            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();
        }
        catch
        {
            if (transaction != null) await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            if (transaction != null) await transaction.DisposeAsync();
        }
    }

    public async Task<List<ReporteIngresanteDto>> ObtenerReporteIngresantesAsync()
    {
        var resultados = await _context.ResultadosAdmision
            .Where(r => r.Resultado == "Ingresante")
            .ToListAsync();

        if (!resultados.Any()) return new List<ReporteIngresanteDto>();

        var idsPostulantes = resultados.Select(r => r.IdPostulante).ToList();
        var postulantes = await _context.Postulantes
            .Where(p => idsPostulantes.Contains(p.IdPostulante))
            .ToListAsync();

        var programas = await _context.ProgramasAcademicos.ToListAsync();

        var reporte = resultados.Select(r => {
            var post = postulantes.FirstOrDefault(p => p.IdPostulante == r.IdPostulante);
            var prog = programas.FirstOrDefault(p => p.IdProgramaAcademico == r.IdProgramaAcademico);

            return new ReporteIngresanteDto
            {
                DNI = post?.DNI ?? "",
                Nombres = post?.Nombres ?? "",
                Apellidos = post?.Apellidos ?? "",
                ProgramaAcademico = prog?.Nombre ?? "",
                Puntaje = r.Calificacion ?? 0m,
                Puesto = r.OrdenMerito ?? 1,
                FechaAdmision = r.FechaResultado
            };
        }).OrderBy(r => r.Puesto).ToList();

        return reporte;
    }

    public async Task<List<ReporteIngresanteDto>> ObtenerReporteTodosAsync()
    {
        var resultados = await _context.ResultadosAdmision.ToListAsync();

        if (!resultados.Any()) return new List<ReporteIngresanteDto>();

        var idsPostulantes = resultados.Select(r => r.IdPostulante).ToList();
        var postulantes = await _context.Postulantes
            .Where(p => idsPostulantes.Contains(p.IdPostulante))
            .ToListAsync();

        var programas = await _context.ProgramasAcademicos.ToListAsync();

        var reporte = resultados.Select(r => {
            var post = postulantes.FirstOrDefault(p => p.IdPostulante == r.IdPostulante);
            var prog = programas.FirstOrDefault(p => p.IdProgramaAcademico == r.IdProgramaAcademico);

            // Using ReporteIngresanteDto but storing the actual result in a way that can be used if needed
            // Wait, ReporteIngresanteDto doesn't have an "Estado" field! We should add it or use an anonymous type?
            // Actually I should just use dynamic or create ReporteGeneralDto if needed, but the frontend can just rely on the new DTO if I update it.
            return new ReporteIngresanteDto
            {
                DNI = post?.DNI ?? "",
                Nombres = post?.Nombres ?? "",
                Apellidos = post?.Apellidos ?? "",
                ProgramaAcademico = prog?.Nombre ?? "",
                Puntaje = r.Calificacion ?? 0m,
                Puesto = r.OrdenMerito ?? 1,
                FechaAdmision = r.FechaResultado,
                Estado = r.Resultado ?? ""
            };
        }).OrderByDescending(r => r.Puntaje).ToList();

        return reporte;
    }
}
