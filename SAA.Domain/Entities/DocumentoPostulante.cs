namespace SAA.Domain.Entities;

public class DocumentoPostulante
{
    public int IdDocumento { get; set; }
    public int IdPostulante { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public DateTime FechaSubida { get; set; } = DateTime.Now;
}
