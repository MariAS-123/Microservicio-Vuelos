using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/clientes")]
[Produces("application/json")]
[Authorize]
public class ClienteAdminController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClienteAdminController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    // GET PAGINADO ? Solo ADMINISTRADOR y AEROLINEA pueden ver el listado completo
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] ClienteFilterDto filter)
    {
        var result = await _clienteService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result, "Consulta de clientes realizada correctamente."));
    }

    // GET BY ID ? ADMINISTRADOR, AEROLINEA y CLIENTE (solo el propio en el service)
    [HttpGet("{id_cliente:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> GetById(int id_cliente)
    {
        var result = await _clienteService.GetByIdAsync(id_cliente, GetIdCliente(), GetRol()); // ?

        if (result is null)
            return NotFound(ApiResponse<ClienteResponseDto>.Fail("Cliente no encontrado."));

        return Ok(ApiResponse<ClienteResponseDto>.Ok(result));
    }

    // POST ? ADMINISTRADOR, AEROLINEA y CLIENTE pueden registrar clientes
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> Create([FromBody] ClienteRequestDto request)
    {
        var result = await _clienteService.CreateAsync(request, GetUsuario());

        return CreatedAtAction(
            nameof(GetById),
            new { id_cliente = result.IdCliente, version = "1" },
            ApiResponse<ClienteResponseDto>.Ok(result, "Cliente creado correctamente."));
    }

    // PUT ? Solo ADMINISTRADOR y CLIENTE (solo el propio en el service)
    [HttpPut("{id_cliente:int}")]
    [Authorize(Roles = "ADMINISTRADOR,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> Update(int id_cliente, [FromBody] ClienteUpdateRequestDto request)
    {
        var result = await _clienteService.UpdateAsync(id_cliente, request, GetUsuario(), GetIdCliente(), GetRol()); // ?

        if (result is null)
            return NotFound(ApiResponse<ClienteResponseDto>.Fail("Cliente no encontrado."));

        return Ok(ApiResponse<ClienteResponseDto>.Ok(result, "Cliente actualizado correctamente."));
    }

    // DELETE ? Solo ADMINISTRADOR
    [HttpDelete("{id_cliente:int}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_cliente)
    {
        var result = await _clienteService.DeleteAsync(id_cliente, GetUsuario());

        return Ok(ApiResponse<bool>.Ok(result, "Cliente eliminado correctamente."));
    }

    private string GetUsuario() =>
        User?.Identity?.Name ?? "SYSTEM";

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string GetRol() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
}
