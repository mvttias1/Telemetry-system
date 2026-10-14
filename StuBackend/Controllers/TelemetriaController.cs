using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StuBackend.Data;
using StuBackend.Models;

namespace StuBackend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TelemetriaController : ControllerBase
{
    private readonly AppDbContext _context;

    // Inyección de dependencias del DbContext
    public TelemetriaController(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated(); // Asegura la creación del archivo .db al iniciar
    }

    // POST: api/v1/telemetria
    // Recibe la telemetría, aplica reglas de negocio y persiste en SQL
    [HttpPost]
    public async Task<ActionResult<RespuestaTelemetria>> ProcesarTelemetria([FromBody] TelemetriaBus datos)
    {
        var alertas = new List<string>();

        // Regla 1: Detenido fuera de paradero asignado
        if (datos.Velocidad == 0 && !datos.EnParadero)
        {
            alertas.Add("ALERTA: Bus detenido fuera de paradero asignado");
        }

        // Regla 2: Exceso de velocidad en ruta urbana (> 60 km/h)
        if (datos.Velocidad > 60)
        {
            alertas.Add("ALERTA CRITICA: Exceso de velocidad");
        }

        // Regla 3: Autonomía crítica de combustible
        if (datos.CombustiblePct < 20)
        {
            alertas.Add($"AVISO: Nivel de combustible bajo ({datos.CombustiblePct}%)");
        }

        var alertaRegistrada = alertas.Count > 0 ? string.Join("; ", alertas) : null;

        // Guardar en la base de datos relacional (SQLite)
        var registro = new RegistroTelemetria
        {
            Patente = datos.Patente,
            Recorrido = datos.Recorrido,
            Velocidad = datos.Velocidad,
            EnParadero = datos.EnParadero,
            CombustiblePct = datos.CombustiblePct,
            Alerta = alertaRegistrada,
            FechaRegistro = DateTime.UtcNow
        };

        _context.Telemetrias.Add(registro);
        await _context.SaveChangesAsync();

        var respuesta = new RespuestaTelemetria
        {
            BusPatente = datos.Patente,
            Recorrido = datos.Recorrido,
            Alertas = alertas,
            EstadoOperacional = alertas.Count > 0 ? "ANOMALIA" : "NORMAL"
        };

        return Ok(respuesta);
    }

    // GET: api/v1/telemetria/ultimos
    // Retorna los últimos 10 registros ingresados a la base de datos
    [HttpGet("ultimos")]
    public async Task<ActionResult<IEnumerable<RegistroTelemetria>>> ObtenerUltimos()
    {
        var ultimos = await _context.Telemetrias
            .OrderByDescending(t => t.Id)
            .Take(10)
            .ToListAsync();

        return Ok(ultimos);
    }

    // GET: api/v1/telemetria/alertas
    // Retorna solo los registros que generaron infracciones o anomalías
    [HttpGet("alertas")]
    public async Task<ActionResult<IEnumerable<RegistroTelemetria>>> ObtenerAlertas()
    {
        var incidentes = await _context.Telemetrias
            .Where(t => t.Alerta != null)
            .OrderByDescending(t => t.Id)
            .Take(20)
            .ToListAsync();

        return Ok(incidentes);
    }

    // GET: api/v1/telemetria/historial/{patente}
    // Auditoría de ruta de un bus específico
    [HttpGet("historial/{patente}")]
    public async Task<ActionResult<IEnumerable<RegistroTelemetria>>> ObtenerHistorialPorBus(string patente)
    {
        var historial = await _context.Telemetrias
            .Where(t => t.Patente.ToUpper() == patente.ToUpper())
            .OrderByDescending(t => t.FechaRegistro)
            .Take(50)
            .ToListAsync();

        return Ok(historial);
    }
}