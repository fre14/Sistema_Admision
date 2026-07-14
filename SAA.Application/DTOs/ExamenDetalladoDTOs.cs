namespace SAA.Application.DTOs;

/// <summary>Detalle de una respuesta de un postulante en una pregunta específica</summary>
public class DetalleRespuestaDto
{
    public int NumeroPregunta { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Enunciado { get; set; } = string.Empty;
    public string OpcionA { get; set; } = string.Empty;
    public string OpcionB { get; set; } = string.Empty;
    public string OpcionC { get; set; } = string.Empty;
    public string OpcionD { get; set; } = string.Empty;
    public string RespuestaSeleccionada { get; set; } = string.Empty;
    public string RespuestaCorrecta { get; set; } = string.Empty;
    public bool EsCorrecta { get; set; }
}

/// <summary>Reporte detallado de un postulante con sus 100 respuestas</summary>
public class ReporteDetalladoPostulanteDto
{
    public string DNI { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string ProgramaAcademico { get; set; } = string.Empty;
    public decimal Puntaje { get; set; }
    public int TotalCorrectas { get; set; }
    public int TotalIncorrectas { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int Puesto { get; set; }
    public List<DetalleRespuestaDto> Respuestas { get; set; } = new();
}

/// <summary>Estadísticas globales del proceso de admisión</summary>
public class EstadisticasDto
{
    public int TotalPostulantes { get; set; }
    public int TotalIngresantes { get; set; }
    public int TotalAprobados { get; set; }
    public int TotalDesaprobados { get; set; }
    public decimal PromedioGeneral { get; set; }
    public decimal PuntajeMaximo { get; set; }
    public decimal PuntajeMinimo { get; set; }
    public decimal PromedioCorrectas { get; set; }
    public List<EstadisticaProgramaDto> PorPrograma { get; set; } = new();
    public List<ReporteIngresanteDto> Top10 { get; set; } = new();
    public List<DistribucionPuntajeDto> DistribucionPuntaje { get; set; } = new();
}

public class EstadisticaProgramaDto
{
    public string Programa { get; set; } = string.Empty;
    public int TotalPostulantes { get; set; }
    public int Ingresantes { get; set; }
    public int Vacantes { get; set; }
    public decimal PromedioPrograma { get; set; }
}

public class DistribucionPuntajeDto
{
    public string Rango { get; set; } = string.Empty; // "0-20", "21-40", etc.
    public int Cantidad { get; set; }
}

public class FilaCargaMasivaDto
{
    public string Dni { get; set; } = string.Empty;
    public string Respuestas { get; set; } = string.Empty;
}
