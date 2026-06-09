namespace SAA.Domain.Entities;

/// <summary>
/// Entidad que representa un Usuario del sistema
/// </summary>
public class Usuario
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre de usuario para login
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario (hash)
    /// </summary>
    public string Contrasena { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>
    /// Rol del usuario en el sistema (Administrador, Evaluador, Registrador, etc.)
    /// </summary>
    public string Rol { get; set; } = "Usuario";

    /// <summary>
    /// Estado del usuario (Activo, Inactivo)
    /// </summary>
    public string Estado { get; set; } = "Activo";

    /// <summary>
    /// Fecha del último acceso
    /// </summary>
    public DateTime? UltimoAcceso { get; set; }

    /// <summary>
    /// Fecha de creación de la cuenta
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }
}
