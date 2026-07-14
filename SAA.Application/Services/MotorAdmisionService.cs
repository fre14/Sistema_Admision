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
        if (postulante == null) throw new Exception("Postulante no encontrado.");

        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;
        try { tx = await _context.Database.BeginTransactionAsync(); } catch (InvalidOperationException) { }
        try
        {
            _context.ExamenesAdmision.Add(new ExamenAdmision
            {
                IdPostulante = postulante.IdPostulante,
                Puntaje = dto.Puntaje,
                Observaciones = dto.Observaciones,
                FechaExamen = DateTime.Now,
                FechaCreacion = DateTime.Now
            });
            await _context.SaveChangesAsync();
            if (tx != null) await tx.CommitAsync();
        }
        catch { if (tx != null) await tx.RollbackAsync(); throw; }
        finally { if (tx != null) await tx.DisposeAsync(); }
    }

    public async Task ProcesarResultadosAsync()
    {
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;
        try { tx = await _context.Database.BeginTransactionAsync(); } catch (InvalidOperationException) { }
        try
        {
            var prev = await _context.ResultadosAdmision.ToListAsync();
            _context.ResultadosAdmision.RemoveRange(prev);
            await _context.SaveChangesAsync();

            var examenes = await _context.ExamenesAdmision.ToListAsync();
            var idsFichas = examenes.Select(e => e.IdFichaPostulacion).Distinct().ToList();
            var fichas = await _context.FichasPostulacion.Where(f => idsFichas.Contains(f.IdFichaPostulacion)).ToListAsync();
            var programas = await _context.ProgramasAcademicos.ToListAsync();
            var asignados = new HashSet<int>();

            foreach (var prog in programas)
            {
                var fichasProg = fichas.Where(f => f.IdProgramaAcademico == prog.IdProgramaAcademico).ToList();
                var examenesProg = examenes
                    .Where(e => fichasProg.Any(f => f.IdFichaPostulacion == e.IdFichaPostulacion))
                    .OrderByDescending(e => e.Puntaje)
                    .ToList();

                int cupos = 0, orden = 1;
                foreach (var examen in examenesProg)
                {
                    if (asignados.Contains(examen.IdPostulante)) continue;
                    string estado = "Desaprobado";
                    if (examen.Puntaje >= 50.0m)
                        estado = cupos < (prog.Vacantes ?? 0) ? "Ingresante" : "Aprobado";
                    if (estado == "Ingresante") cupos++;

                    _context.ResultadosAdmision.Add(new ResultadoAdmision
                    {
                        IdPostulante = examen.IdPostulante,
                        IdProgramaAcademico = prog.IdProgramaAcademico,
                        Calificacion = examen.Puntaje,
                        Resultado = estado,
                        OrdenMerito = orden++,
                        FechaResultado = DateTime.Now
                    });
                    asignados.Add(examen.IdPostulante);
                }
            }
            await _context.SaveChangesAsync();
            if (tx != null) await tx.CommitAsync();
        }
        catch { if (tx != null) await tx.RollbackAsync(); throw; }
        finally { if (tx != null) await tx.DisposeAsync(); }
    }

    public async Task<List<ReporteIngresanteDto>> ObtenerReporteIngresantesAsync()
    {
        var resultados = await _context.ResultadosAdmision.Where(r => r.Resultado == "Ingresante").ToListAsync();
        return await MapearReporte(resultados);
    }

    public async Task<List<ReporteIngresanteDto>> ObtenerReporteTodosAsync()
    {
        var resultados = await _context.ResultadosAdmision.ToListAsync();
        var reporte = await MapearReporte(resultados);
        return reporte.OrderByDescending(r => r.Puntaje).ToList();
    }

    private async Task<List<ReporteIngresanteDto>> MapearReporte(List<ResultadoAdmision> resultados)
    {
        if (!resultados.Any()) return new List<ReporteIngresanteDto>();
        var ids = resultados.Select(r => r.IdPostulante).ToList();
        var postulantes = await _context.Postulantes.Where(p => ids.Contains(p.IdPostulante)).ToListAsync();
        var programas = await _context.ProgramasAcademicos.ToListAsync();

        return resultados.Select(r =>
        {
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
                FechaAdmision = r.FechaResultado,
                Estado = r.Resultado ?? ""
            };
        }).ToList();
    }

    // ─────────────────────────────────────────────────────────────
    // NUEVO: Obtener detalle de respuestas de un postulante por DNI
    // ─────────────────────────────────────────────────────────────
    public async Task<ReporteDetalladoPostulanteDto?> ObtenerDetallePostulanteAsync(string dni)
    {
        var postulante = await _context.Postulantes.FirstOrDefaultAsync(p => p.DNI == dni);
        if (postulante == null) return null;

        var examen = await _context.ExamenesAdmision
            .Where(e => e.IdPostulante == postulante.IdPostulante)
            .OrderByDescending(e => e.FechaExamen)
            .FirstOrDefaultAsync();
        if (examen == null) return null;

        var resultado = await _context.ResultadosAdmision
            .FirstOrDefaultAsync(r => r.IdPostulante == postulante.IdPostulante);

        var programa = await _context.ProgramasAcademicos
            .FirstOrDefaultAsync(p => p.IdProgramaAcademico == postulante.IdProgramaInteres);

        var respuestas = await _context.RespuestasPostulante
            .Where(r => r.IdPostulante == postulante.IdPostulante && r.IdExamen == examen.IdExamen)
            .ToListAsync();

        var preguntas = await _context.PreguntasExamen
            .OrderBy(p => p.NumeroPregunta)
            .ToListAsync();

        var detalles = preguntas.Select(preg =>
        {
            var resp = respuestas.FirstOrDefault(r => r.IdPregunta == preg.IdPregunta);
            return new DetalleRespuestaDto
            {
                NumeroPregunta = preg.NumeroPregunta,
                Area = preg.Area,
                Enunciado = preg.Enunciado,
                OpcionA = preg.OpcionA,
                OpcionB = preg.OpcionB,
                OpcionC = preg.OpcionC,
                OpcionD = preg.OpcionD,
                RespuestaSeleccionada = resp?.RespuestaSeleccionada ?? "—",
                RespuestaCorrecta = preg.RespuestaCorrecta,
                EsCorrecta = resp?.EsCorrecta ?? false
            };
        }).ToList();

        int correctas = detalles.Count(d => d.EsCorrecta);

        return new ReporteDetalladoPostulanteDto
        {
            DNI = postulante.DNI,
            Nombres = postulante.Nombres,
            Apellidos = postulante.Apellidos,
            ProgramaAcademico = programa?.Nombre ?? "",
            Puntaje = examen.Puntaje ?? 0m,
            TotalCorrectas = correctas,
            TotalIncorrectas = 100 - correctas,
            Estado = resultado?.Resultado ?? "Sin resultado",
            Puesto = resultado?.OrdenMerito ?? 0,
            Respuestas = detalles
        };
    }

    // ─────────────────────────────────────────────────────────────
    // NUEVO: Estadísticas globales del proceso de admisión
    // ─────────────────────────────────────────────────────────────
    public async Task<EstadisticasDto> ObtenerEstadisticasAsync()
    {
        var resultados = await _context.ResultadosAdmision.ToListAsync();
        var programas = await _context.ProgramasAcademicos.ToListAsync();
        var postulantes = await _context.Postulantes.ToListAsync();
        var examenes = await _context.ExamenesAdmision.ToListAsync();

        if (!resultados.Any())
            return new EstadisticasDto { TotalPostulantes = postulantes.Count };

        var puntajes = resultados.Select(r => r.Calificacion ?? 0m).ToList();
        var top10 = await ObtenerReporteIngresantesAsync();

        // Distribución por rangos de puntaje
        var distribucion = new List<DistribucionPuntajeDto>
        {
            new() { Rango = "0–20",   Cantidad = puntajes.Count(p => p <= 20) },
            new() { Rango = "21–40",  Cantidad = puntajes.Count(p => p > 20 && p <= 40) },
            new() { Rango = "41–60",  Cantidad = puntajes.Count(p => p > 40 && p <= 60) },
            new() { Rango = "61–80",  Cantidad = puntajes.Count(p => p > 60 && p <= 80) },
            new() { Rango = "81–100", Cantidad = puntajes.Count(p => p > 80) },
        };

        // Estadísticas por programa
        var idsPosts = resultados.Select(r => r.IdPostulante).Distinct().ToList();
        var fichas = await _context.FichasPostulacion
            .Where(f => idsPosts.Contains(f.IdPostulante))
            .ToListAsync();

        var porPrograma = programas.Select(prog =>
        {
            var restsPrograma = resultados.Where(r => r.IdProgramaAcademico == prog.IdProgramaAcademico).ToList();
            return new EstadisticaProgramaDto
            {
                Programa = prog.Nombre,
                TotalPostulantes = restsPrograma.Count,
                Ingresantes = restsPrograma.Count(r => r.Resultado == "Ingresante"),
                Vacantes = prog.Vacantes ?? 0,
                PromedioPrograma = restsPrograma.Any()
                    ? Math.Round(restsPrograma.Average(r => r.Calificacion ?? 0m), 2)
                    : 0m
            };
        }).Where(p => p.TotalPostulantes > 0).ToList();

        // Promedio correctas (basado en puntaje que = nro correctas)
        var promedioCorrectas = examenes.Any()
            ? Math.Round(examenes.Average(e => e.Puntaje ?? 0m), 2)
            : 0m;

        return new EstadisticasDto
        {
            TotalPostulantes = resultados.Count,
            TotalIngresantes = resultados.Count(r => r.Resultado == "Ingresante"),
            TotalAprobados = resultados.Count(r => r.Resultado == "Aprobado"),
            TotalDesaprobados = resultados.Count(r => r.Resultado == "Desaprobado"),
            PromedioGeneral = puntajes.Any() ? Math.Round(puntajes.Average(), 2) : 0m,
            PuntajeMaximo = puntajes.Any() ? puntajes.Max() : 0m,
            PuntajeMinimo = puntajes.Any() ? puntajes.Min() : 0m,
            PromedioCorrectas = promedioCorrectas,
            PorPrograma = porPrograma,
            Top10 = top10.Take(10).ToList(),
            DistribucionPuntaje = distribucion
        };
    }

    // ─────────────────────────────────────────────────────────────
    // NUEVO: Exportar todos los resultados como CSV
    // ─────────────────────────────────────────────────────────────
    public async Task<string> ExportarResultadosCsvAsync()
    {
        var reporte = await ObtenerReporteTodosAsync();
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Puesto,DNI,Nombres,Apellidos,Programa Académico,Puntaje,Correctas,Estado,Fecha Admisión");

        foreach (var r in reporte)
        {
            // Calcular correctas desde puntaje (puntaje = nro correctas en nuestro modelo)
            int correctas = (int)Math.Round(r.Puntaje);
            sb.AppendLine($"{r.Puesto},{r.DNI},{r.Nombres},{r.Apellidos},{r.ProgramaAcademico},{r.Puntaje},{correctas},{r.Estado},{r.FechaAdmision:dd/MM/yyyy}");
        }
        return sb.ToString();
    }

    // ─────────────────────────────────────────────────────────────
    // CRUD Programas y Vacantes
    // ─────────────────────────────────────────────────────────────
    public async Task<List<ProgramaAcademico>> ObtenerProgramasAsync()
    {
        return await _context.ProgramasAcademicos.ToListAsync();
    }

    public async Task GuardarProgramaAsync(ProgramaAcademico dto)
    {
        if (dto.IdProgramaAcademico == 0)
        {
            dto.FechaCreacion = DateTime.Now;
            _context.ProgramasAcademicos.Add(dto);
        }
        else
        {
            var prog = await _context.ProgramasAcademicos.FindAsync(dto.IdProgramaAcademico);
            if (prog != null)
            {
                prog.Nombre = dto.Nombre;
                prog.Codigo = dto.Codigo;
                prog.Departamento = dto.Departamento;
                prog.Vacantes = dto.Vacantes;
                prog.Estado = dto.Estado;
                prog.FechaActualizacion = DateTime.Now;
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task EliminarProgramaAsync(int id)
    {
        var prog = await _context.ProgramasAcademicos.FindAsync(id);
        if (prog != null)
        {
            _context.ProgramasAcademicos.Remove(prog);
            await _context.SaveChangesAsync();
        }
    }

    // ─────────────────────────────────────────────────────────────
    // Carga Masiva de Exámenes (Lectora Óptica)
    // ─────────────────────────────────────────────────────────────
    public async Task<int> CargaMasivaExamenesAsync(List<FilaCargaMasivaDto> filas)
    {
        int procesados = 0;
        var preguntas = await _context.PreguntasExamen.OrderBy(p => p.NumeroPregunta).ToListAsync();
        if (!preguntas.Any()) throw new Exception("No hay preguntas sembradas en el sistema.");

        foreach (var fila in filas)
        {
            var dni = fila.Dni.Trim();
            var respuestasStr = fila.Respuestas.Trim().ToUpper();

            if (string.IsNullOrEmpty(dni) || respuestasStr.Length != 100) continue;

            var postulante = await _context.Postulantes.FirstOrDefaultAsync(p => p.DNI == dni);
            if (postulante == null) continue;

            // Buscar si tiene ficha
            var ficha = await _context.FichasPostulacion
                .FirstOrDefaultAsync(f => f.IdPostulante == postulante.IdPostulante);
            if (ficha == null) continue;

            // Eliminar examen y respuestas anteriores si existen
            var examenAnterior = await _context.ExamenesAdmision
                .FirstOrDefaultAsync(e => e.IdPostulante == postulante.IdPostulante);
            if (examenAnterior != null)
            {
                var respsAnteriores = await _context.RespuestasPostulante
                    .Where(r => r.IdPostulante == postulante.IdPostulante && r.IdExamen == examenAnterior.IdExamen)
                    .ToListAsync();
                _context.RespuestasPostulante.RemoveRange(respsAnteriores);
                _context.ExamenesAdmision.Remove(examenAnterior);
                await _context.SaveChangesAsync();
            }

            // Calcular correctas
            var respuestas = new List<RespuestaPostulante>();
            int correctas = 0;

            for (int i = 0; i < 100; i++)
            {
                var preg = preguntas[i];
                var sel = respuestasStr[i].ToString();
                bool esCorrecta = sel == preg.RespuestaCorrecta;
                if (esCorrecta) correctas++;

                respuestas.Add(new RespuestaPostulante
                {
                    IdPregunta = preg.IdPregunta,
                    RespuestaSeleccionada = sel,
                    EsCorrecta = esCorrecta
                });
            }

            var examen = new ExamenAdmision
            {
                IdFichaPostulacion = ficha.IdFichaPostulacion,
                IdPostulante = postulante.IdPostulante,
                NombreExamen = "Examen de Lectora Óptica",
                FechaExamen = DateTime.Now,
                Estado = "Realizado",
                Puntaje = correctas,
                FechaCreacion = DateTime.Now
            };

            _context.ExamenesAdmision.Add(examen);
            await _context.SaveChangesAsync();

            foreach (var r in respuestas)
            {
                r.IdExamen = examen.IdExamen;
                r.IdPostulante = postulante.IdPostulante;
            }

            _context.RespuestasPostulante.AddRange(respuestas);
            await _context.SaveChangesAsync();
            procesados++;
        }

        return procesados;
    }
}
