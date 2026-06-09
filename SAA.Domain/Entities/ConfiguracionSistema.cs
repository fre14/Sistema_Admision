namespace SAA.Domain.Entities;

public class ConfiguracionSistema
{
    public int IdConfiguracion { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
}
