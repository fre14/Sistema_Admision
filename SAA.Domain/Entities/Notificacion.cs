namespace SAA.Domain.Entities;

public class Notificacion
{
    public int IdNotificacion { get; set; }
    public int IdPostulante { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; } = DateTime.Now;
    public bool Leido { get; set; }
}
