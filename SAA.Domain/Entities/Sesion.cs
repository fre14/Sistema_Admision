namespace SAA.Domain.Entities;

public class Sesion
{
    public int IdSesion { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Token { get; set; }
}
