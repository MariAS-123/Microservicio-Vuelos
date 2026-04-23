using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/boletos")]
[Produces("application/json")]
[Authorize]
public class BoletoAdminController : ControllerBase
{
    private readonly IBoletoService _boletoService;

    public BoletoAdminController(IBoletoService boletoService)
    {
        _boletoService = boletoService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] BoletoFilterDto filter)
    {
        var result = await _boletoService.GetPagedAsync(filter);
        return Ok(ApiResponse<object>.Ok(result, "Consulta de boletos realizada correctamente."));
    }

    [HttpGet("{id_boleto:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<BoletoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponseDto>>> GetById(int id_boleto)
    {
        var result = await _boletoService.GetByIdAsync(id_boleto, GetIdCliente(), GetRol());

        if (result is null)
            return NotFound(ApiResponse<BoletoResponseDto>.Fail("Boleto no encontrado."));

        return Ok(ApiResponse<BoletoResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<BoletoResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponseDto>>> Create([FromBody] BoletoRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _boletoService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_boleto = result.IdBoleto, version = "1" },
            ApiResponse<BoletoResponseDto>.Ok(result, "Boleto creado correctamente."));
    }

    private string GetUsuario()
    {
        var name = User?.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name.Trim();

        var username = User?.FindFirst("username")?.Value;
        if (!string.IsNullOrWhiteSpace(username))
            return username.Trim();

        return "SYSTEM";
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
