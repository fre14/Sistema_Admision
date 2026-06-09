namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa el Resultado de Admisión de un postulante
/// </summary>
public class ResultadoAdmision
{
    /// <summary>
    /// Identificador único del resultado
    /// </summary>
    public int IdResultado { get; set; }

    /// <summary>
    /// Identificador de la ficha de postulación
    /// </summary>
    public int IdFichaPostulacion { get; set; }

    /// <summary>
    /// Identificador del postulante
    /// </summary>
    public int IdPostulante { get; set; }

    /// <summary>
    /// Identificador del programa académico
    /// </summary>
    public int IdProgramaAcademico { get; set; }

    /// <summary>
    /// Calificación o puntaje obtenido
    /// </summary>
    public decimal? Calificacion { get; set; }

    /// <summary>
    /// Decisión de admisión (Aprobado, Rechazado, En Espera)
    /// </summary>
    public string Resultado { get; set; } = string.Empty;

    /// <summary>
    /// Puesto o mérito obtenido
    /// </summary>
    public int? OrdenMerito { get; set; }

    /// <summary>
    /// Observaciones del resultado
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de generación del resultado
    /// </summary>
    public DateTime FechaResultado { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador del usuario que generó el resultado
    /// </summary>
    public int? IdUsuarioEvaluador { get; set; }

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }
}
