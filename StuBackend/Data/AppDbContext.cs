using Microsoft.EntityFrameworkCore;
using StuBackend.Models;

namespace StuBackend.Data;

public class RegistroTelemetria
{
    public int Id { get; set; }
    public string Patente { get; set; } = string.Empty;
    public string Recorrido { get; set; } = string.Empty;
    public double Velocidad { get; set; }
    public bool EnParadero { get; set; }
    public double CombustiblePct { get; set; }
    public string? Alerta { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<RegistroTelemetria> Telemetrias => Set<RegistroTelemetria>();
}