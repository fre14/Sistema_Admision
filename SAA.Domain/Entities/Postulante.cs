namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa un Postulante en el sistema de admisiones
/// </summary>
public class Postulante
{
    /// <summary>
    /// Identificador único del postulante
    /// </summary>
    public int IdPostulante { get; set; }

    /// <summary>
    /// Nombres del postulante
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del postulante
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Número de DNI (Documento Nacional de Identidad)
    /// </summary>
    public string DNI { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Programa Académico de Interés
    /// </summary>
    public int IdProgramaInteres { get; set; }

    /// <summary>
    /// Correo electrónico del postulante
    /// </summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono del postulante
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Dirección del postulante
    /// </summary>
    public string? Direccion { get; set; }

    /// <summary>
    /// Fecha de nacimiento del postulante
    /// </summary>
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Estado del postulante (Activo, Inactivo, etc.)
    /// </summary>
    public string Estado { get; set; } = "Activo";

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última actualización del registro
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }
}
