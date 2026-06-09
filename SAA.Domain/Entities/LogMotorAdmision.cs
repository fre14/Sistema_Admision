namespace SAA.Domain.Entities;

public class LogMotorAdmision
{
    public int IdLog { get; set; }
    public string Evento { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Detalle { get; set; } = string.Empty;
}
