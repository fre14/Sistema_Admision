namespace SAA.Application.DTOs;

public class MiResultadoDto
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Programa { get; set; } = string.Empty;
    public decimal Puntaje { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int Puesto { get; set; }
}
