using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SAA.Application.DTOs;
using SAA.Application.Services;

namespace SAA.API.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostulantesController : ControllerBase
{
    private readonly PostulanteService _postulanteService;

    public PostulantesController(PostulanteService postulanteService)
    {
        _postulanteService = postulanteService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearPostulante([FromBody] CrearPostulanteDto dto)
    {
        try
        {
            var result = await _postulanteService.CrearPostulanteAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPostulantes()
    {
        var result = await _postulanteService.ObtenerTodosAsync();
        return Ok(result);
    }

    [HttpGet("mi-resultado")]
    [Authorize(Roles = "Postulante")]
    public async Task<IActionResult> MiResultado()
    {
        var dni = ObtenerDniDelToken();
        if (string.IsNullOrEmpty(dni)) return Unauthorized(new { mensaje = "DNI no encontrado en el token." });

        var resultado = await _postulanteService.ObtenerMiResultadoAsync(dni);
        if (resultado == null) return NotFound("Resultado no encontrado.");
        return Ok(resultado);
    }

    /// <summary>Retorna las 100 respuestas del examen del postulante autenticado</summary>
    [HttpGet("mi-detalle")]
    [Authorize(Roles = "Postulante")]
    public async Task<IActionResult> MiDetalle()
    {
        var dni = ObtenerDniDelToken();
        if (string.IsNullOrEmpty(dni)) return Unauthorized(new { mensaje = "DNI no encontrado en el token." });

        var detalle = await _postulanteService.ObtenerMiDetalleAsync(dni);
        if (detalle == null) return NotFound("Detalle de examen no encontrado.");
        return Ok(detalle);
    }

    private string? ObtenerDniDelToken()
    {
        return User.Claims.FirstOrDefault(c =>
            c.Type == System.Security.Claims.ClaimTypes.Name ||
            c.Type == "unique_name" ||
            c.Type == "name" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
    }
}
