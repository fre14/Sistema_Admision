namespace SAA.Domain.Entities;

public class PeriodoAdmision
{
    public int IdPeriodo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
