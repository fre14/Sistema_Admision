namespace SAA.Domain.Entities;

public class Matricula
{
    public int IdMatricula { get; set; }
    public int IdPostulante { get; set; }
    public int IdProgramaAcademico { get; set; }
    public DateTime FechaMatricula { get; set; } = DateTime.Now;
    public decimal? MontoPagado { get; set; }
}
