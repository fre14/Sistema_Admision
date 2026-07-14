namespace SAA.Domain.Entities;

/// <summary>
/// Pregunta del examen de admisión (banco de 100 preguntas)
/// </summary>
public class PreguntaExamen
{
    public int IdPregunta { get; set; }
    public int NumeroPregunta { get; set; }     // 1-100
    public string Area { get; set; } = string.Empty; // "Matematica" | "Verbal" | "Ciencias"
    public string Enunciado { get; set; } = string.Empty;
    public string OpcionA { get; set; } = string.Empty;
    public string OpcionB { get; set; } = string.Empty;
    public string OpcionC { get; set; } = string.Empty;
    public string OpcionD { get; set; } = string.Empty;
    public string RespuestaCorrecta { get; set; } = string.Empty; // "A" | "B" | "C" | "D"
}
