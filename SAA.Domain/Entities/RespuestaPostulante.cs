namespace SAA.Domain.Entities;

/// <summary>
/// Respuesta marcada por un postulante en cada pregunta del examen
/// </summary>
public class RespuestaPostulante
{
    public int IdRespuesta { get; set; }
    public int IdExamen { get; set; }
    public int IdPostulante { get; set; }
    public int IdPregunta { get; set; }
    public string RespuestaSeleccionada { get; set; } = string.Empty; // "A" | "B" | "C" | "D"
    public bool EsCorrecta { get; set; }
}
