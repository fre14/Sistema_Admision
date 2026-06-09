namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa la Ficha de Postulación
/// </summary>
public class FichaPostulacion
{
    /// <summary>
    /// Identificador único de la ficha
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
    /// Número de trámite de la ficha
    /// </summary>
    public string NumeroTramite { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de postulación
    /// </summary>
    public DateTime FechaPostulacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Estado de la postulación (Registrada, En Revisión, Aceptada, Rechazada)
    /// </summary>
    public string Estado { get; set; } = "Registrada";

    /// <summary>
    /// Observaciones sobre la postulación
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }

    /// <summary>
    /// Usuario que realizó la última actualización
    /// </summary>
    public int? IdUsuarioActualizacion { get; set; }
}
