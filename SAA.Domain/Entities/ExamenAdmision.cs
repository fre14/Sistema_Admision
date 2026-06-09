namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa un Examen de Admisión
/// </summary>
public class ExamenAdmision
{
    /// <summary>
    /// Identificador único del examen
    /// </summary>
    public int IdExamen { get; set; }

    /// <summary>
    /// Identificador de la ficha de postulación
    /// </summary>
    public int IdFichaPostulacion { get; set; }

    /// <summary>
    /// Identificador del postulante
    /// </summary>
    public int IdPostulante { get; set; }

    /// <summary>
    /// Nombre o tipo del examen (Matemática, Verbal, etc.)
    /// </summary>
    public string NombreExamen { get; set; } = string.Empty;

    /// <summary>
    /// Fecha programada del examen
    /// </summary>
    public DateTime FechaExamen { get; set; }

    /// <summary>
    /// Hora de inicio del examen
    /// </summary>
    public TimeSpan? HoraInicio { get; set; }

    /// <summary>
    /// Duración del examen en minutos
    /// </summary>
    public int? DuracionMinutos { get; set; }

    /// <summary>
    /// Sala o lugar donde se realiza el examen
    /// </summary>
    public string? Sala { get; set; }

    /// <summary>
    /// Estado del examen (Programado, Realizado, Pendiente, Anulado)
    /// </summary>
    public string Estado { get; set; } = "Programado";

    /// <summary>
    /// Puntaje obtenido en el examen (0 a 1000)
    /// </summary>
    public decimal? Puntaje { get; set; }

    /// <summary>
    /// Observaciones sobre el desarrollo del examen
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }
}
