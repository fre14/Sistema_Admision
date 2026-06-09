namespace SAA.Domain.Entities;

public class LogAuditoria
{
    public int IdLog { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Detalle { get; set; } = string.Empty;
}
