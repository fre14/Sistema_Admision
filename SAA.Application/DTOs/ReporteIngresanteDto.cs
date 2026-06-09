namespace SAA.Application.DTOs;

public class ReporteIngresanteDto
{
    public string DNI { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string ProgramaAcademico { get; set; } = string.Empty;
    public decimal Puntaje { get; set; }
    public int Puesto { get; set; }
    public DateTime? FechaAdmision { get; set; }
    public string Estado { get; set; } = string.Empty;
}
