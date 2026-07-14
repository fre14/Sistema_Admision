using Microsoft.EntityFrameworkCore;
using SAA.Application.DTOs;
using SAA.Application.Interfaces;
using SAA.Domain.Entities;

namespace SAA.Application.Services;

public class PostulanteService
{
    private readonly IApplicationDbContext _context;

    public PostulanteService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PostulanteResponseDto> CrearPostulanteAsync(CrearPostulanteDto dto)
    {
        // Validar unicidad de DNI
        var existeDni = await _context.Postulantes.AnyAsync(p => p.DNI == dto.DNI);
        if (existeDni)
        {
            throw new InvalidOperationException("El DNI ya se encuentra registrado en el sistema.");
        }

        // Crear el postulante
        var postulante = new Postulante
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            DNI = dto.DNI,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            IdProgramaInteres = dto.IdProgramaInteres,
            FechaCreacion = DateTime.Now,
            Estado = "Activo"
        };

        // En un sistema real, la contraseña debería ser encriptada y guardada en una tabla Usuario.
        // Simulando esto creando el usuario vinculado:
        var usuario = new Usuario
        {
            NombreUsuario = dto.DNI,
            Contrasena = dto.Contrasena, // Hashear en un entorno real
            NombreCompleto = $"{dto.Nombres} {dto.Apellidos}",
            Correo = dto.Correo,
            Rol = "Postulante",
            Estado = "Activo",
            FechaCreacion = DateTime.Now
        };

        // Usar transacción atómica
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Postulantes.Add(postulante);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new PostulanteResponseDto
        {
            IdPostulante = postulante.IdPostulante,
            Nombres = postulante.Nombres,
            Apellidos = postulante.Apellidos,
            DNI = postulante.DNI,
            Correo = postulante.Correo,
            Telefono = postulante.Telefono,
            IdProgramaInteres = postulante.IdProgramaInteres
        };
    }

    public async Task<List<PostulanteResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Postulantes
            .Select(p => new PostulanteResponseDto
            {
                IdPostulante = p.IdPostulante,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                DNI = p.DNI,
                Correo = p.Correo,
                Telefono = p.Telefono,
                IdProgramaInteres = p.IdProgramaInteres
            })
            .ToListAsync();
    }

    public async Task<MiResultadoDto?> ObtenerMiResultadoAsync(string dni)
    {
        var postulante = await _context.Postulantes.FirstOrDefaultAsync(p => p.DNI == dni);
        if (postulante == null) return null;

        var resultado = await _context.ResultadosAdmision.FirstOrDefaultAsync(r => r.IdPostulante == postulante.IdPostulante);
        var programa = await _context.ProgramasAcademicos.FirstOrDefaultAsync(p => p.IdProgramaAcademico == postulante.IdProgramaInteres);

        return new MiResultadoDto
        {
            Nombres = postulante.Nombres,
            Apellidos = postulante.Apellidos,
            Programa = programa?.Nombre ?? "No Asignado",
            Puntaje = resultado?.Calificacion ?? 0,
            Estado = resultado?.Resultado ?? "Pendiente",
            Puesto = resultado?.OrdenMerito ?? 0
        };
    }

    /// <summary>Retorna el detalle de las 100 respuestas del postulante autenticado</summary>
    public async Task<ReporteDetalladoPostulanteDto?> ObtenerMiDetalleAsync(string dni)
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
            Estado = resultado?.Resultado ?? "Pendiente",
            Puesto = resultado?.OrdenMerito ?? 0,
            Respuestas = detalles
        };
    }
}

