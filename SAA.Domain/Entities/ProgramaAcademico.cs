namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa un Programa Académico
/// </summary>
public class ProgramaAcademico
{
    /// <summary>
    /// Identificador único del programa
    /// </summary>
    public int IdProgramaAcademico { get; set; }

    /// <summary>
    /// Código del programa académico
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del programa académico
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del programa
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nivel académico (Pregrado, Postgrado, Técnico, etc.)
    /// </summary>
    public string? NivelAcademico { get; set; }

    /// <summary>
    /// Número de vacantes disponibles
    /// </summary>
    public int? Vacantes { get; set; }

    /// <summary>
    /// Fecha de inicio del proceso de admisión
    /// </summary>
    public DateTime? FechaInicioProceso { get; set; }

    /// <summary>
    /// Fecha de cierre del proceso de admisión
    /// </summary>
    public DateTime? FechaFinalProceso { get; set; }

    /// <summary>
    /// Estado del programa (Activo, Inactivo, Cerrado)
    /// </summary>
    public string Estado { get; set; } = "Activo";

    /// <summary>
    /// Departamento o facultad responsable
    /// </summary>
    public string? Departamento { get; set; }

    /// <summary>
    /// Fecha de creación del programa
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }
}
