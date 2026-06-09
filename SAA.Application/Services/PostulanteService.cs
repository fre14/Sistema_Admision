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
}
