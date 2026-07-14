using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SAA.Domain.Entities;

namespace SAA.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Rol> Roles { get; }
    DbSet<Postulante> Postulantes { get; }
    DbSet<FichaPostulacion> FichasPostulacion { get; }
    DbSet<ExamenAdmision> ExamenesAdmision { get; }
    DbSet<ResultadoAdmision> ResultadosAdmision { get; }
    DbSet<ProgramaAcademico> ProgramasAcademicos { get; }
    DbSet<PeriodoAdmision> PeriodosAdmision { get; }
    DbSet<PreguntaExamen> PreguntasExamen { get; }
    DbSet<RespuestaPostulante> RespuestasPostulante { get; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
