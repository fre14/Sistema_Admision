using Microsoft.EntityFrameworkCore;
using SAA.Domain.Entities;
using SAA.Application.Interfaces;

namespace SAA.Infrastructure.Data;

public class SAADbContext : DbContext, IApplicationDbContext
{
    public SAADbContext(DbContextOptions<SAADbContext> options) : base(options)
    {
    }

    // Seguridad
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Rol> Roles { get; set; } = null!;

    // Admision
    public DbSet<Postulante> Postulantes { get; set; } = null!;
    public DbSet<FichaPostulacion> FichasPostulacion { get; set; } = null!;
    public DbSet<ExamenAdmision> ExamenesAdmision { get; set; } = null!;
    public DbSet<ResultadoAdmision> ResultadosAdmision { get; set; } = null!;

    // Examen detallado (nuevas tablas)
    public DbSet<PreguntaExamen> PreguntasExamen { get; set; } = null!;
    public DbSet<RespuestaPostulante> RespuestasPostulante { get; set; } = null!;

    // Config
    public DbSet<ProgramaAcademico> ProgramasAcademicos { get; set; } = null!;
    public DbSet<PeriodoAdmision> PeriodosAdmision { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seguridad
        modelBuilder.Entity<Usuario>(entity => {
            entity.ToTable("Usuario", "Seguridad").HasKey(u => u.IdUsuario);
        });
        modelBuilder.Entity<Rol>().ToTable("Rol", "Seguridad").HasKey(r => r.IdRol);

        // Admision
        modelBuilder.Entity<Postulante>(entity => {
            entity.ToTable("Postulante", "Admision").HasKey(p => p.IdPostulante);
        });
        modelBuilder.Entity<FichaPostulacion>(entity => {
            entity.ToTable("FichaPostulacion", "Admision").HasKey(f => f.IdFichaPostulacion);
        });
        modelBuilder.Entity<ExamenAdmision>(entity => {
            entity.ToTable("ExamenAdmision", "Admision").HasKey(e => e.IdExamen);
            entity.Property(e => e.Puntaje).HasPrecision(18, 2);
        });
        modelBuilder.Entity<ResultadoAdmision>(entity => {
            entity.ToTable("ResultadoAdmision", "Admision").HasKey(r => r.IdResultado);
            entity.Property(r => r.Calificacion).HasPrecision(18, 2);
        });

        // Examen detallado
        modelBuilder.Entity<PreguntaExamen>(entity => {
            entity.ToTable("PreguntaExamen", "Admision").HasKey(p => p.IdPregunta);
        });
        modelBuilder.Entity<RespuestaPostulante>(entity => {
            entity.ToTable("RespuestaPostulante", "Admision").HasKey(r => r.IdRespuesta);
        });

        // Config
        modelBuilder.Entity<ProgramaAcademico>(entity => {
            entity.ToTable("ProgramaAcademico", "Config").HasKey(p => p.IdProgramaAcademico);
        });
        modelBuilder.Entity<PeriodoAdmision>().ToTable("PeriodoAdmision", "Config").HasKey(p => p.IdPeriodo);
    }
}
