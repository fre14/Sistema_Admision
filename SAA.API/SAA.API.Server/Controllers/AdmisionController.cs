using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAA.Application.DTOs;
using SAA.Application.Services;

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
}
