using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAA.Application.DTOs;
using SAA.Application.Services;
using System.Text;

namespace SAA.API.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class AdmisionController : ControllerBase
{
    private readonly MotorAdmisionService _motorAdmisionService;

    public AdmisionController(MotorAdmisionService motorAdmisionService)
    {
        _motorAdmisionService = motorAdmisionService;
    }

    [HttpPost("examen")]
    public async Task<IActionResult> RegistrarExamen([FromBody] RegistrarExamenDto dto)
    {
        try
        {
            await _motorAdmisionService.RegistrarExamenAsync(dto);
            return Ok(new { mensaje = "Examen registrado exitosamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("procesar")]
    public async Task<IActionResult> ProcesarResultados()
    {
        try
        {
            await _motorAdmisionService.ProcesarResultadosAsync();
            return Ok(new { mensaje = "Resultados de admisión procesados exitosamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al procesar resultados: " + ex.Message });
        }
    }

    [HttpGet("reporte-ingresantes")]
    public async Task<IActionResult> ObtenerReporteIngresantes()
    {
        try
        {
            var reporte = await _motorAdmisionService.ObtenerReporteIngresantesAsync();
            return Ok(reporte);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al obtener reporte: " + ex.Message });
        }
    }

    [HttpGet("reporte-todos")]
    public async Task<IActionResult> ObtenerReporteTodos()
    {
        try
        {
            var reporte = await _motorAdmisionService.ObtenerReporteTodosAsync();
            return Ok(reporte);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al obtener reporte: " + ex.Message });
        }
    }

    /// <summary>Buscar postulante por DNI y obtener su reporte detallado de respuestas</summary>
    [HttpGet("buscar/{dni}")]
    public async Task<IActionResult> BuscarPostulante(string dni)
    {
        try
        {
            var detalle = await _motorAdmisionService.ObtenerDetallePostulanteAsync(dni);
            if (detalle == null)
                return NotFound(new { mensaje = $"No se encontró ningún postulante con DNI: {dni}" });
            return Ok(detalle);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al buscar postulante: " + ex.Message });
        }
    }

    /// <summary>Obtener estadísticas globales del proceso de admisión</summary>
    [HttpGet("estadisticas")]
    public async Task<IActionResult> ObtenerEstadisticas()
    {
        try
        {
            var estadisticas = await _motorAdmisionService.ObtenerEstadisticasAsync();
            return Ok(estadisticas);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al obtener estadísticas: " + ex.Message });
        }
    }

    /// <summary>Exportar todos los resultados como CSV</summary>
    [HttpGet("exportar-csv")]
    public async Task<IActionResult> ExportarCsv()
    {
        try
        {
            var csv = await _motorAdmisionService.ExportarResultadosCsvAsync();
            var bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"resultados_admision_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al exportar: " + ex.Message });
        }
    }
}
