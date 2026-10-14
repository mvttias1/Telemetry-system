namespace StuBackend.Models;

public class TelemetriaBus
{
    public string Patente { get; set; } = string.Empty;
    public string Recorrido { get; set; } = string.Empty;
    public double Velocidad { get; set; }
    public bool EnParadero { get; set; }
    public double CombustiblePct { get; set; }
}

public class RespuestaTelemetria
{
    public string BusPatente { get; set; } = string.Empty;
    public string Recorrido { get; set; } = string.Empty;
    public List<string> Alertas { get; set; } = new();
    public string EstadoOperacional { get; set; } = "NORMAL";
}