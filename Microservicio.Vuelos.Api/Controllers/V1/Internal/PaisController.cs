using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/paises")]
[Produces("application/json")]
[Authorize]
public class PaisController : ControllerBase
{
    private readonly IPaisService _paisService;

    public PaisController(IPaisService paisService)
    {
        _paisService = paisService;
    }

    // GET /paises
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] PaisFilterDto filter)
    {
        var result = await _paisService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(result));
    }

    // GET /paises/{id_pais}
    [HttpGet("{id_pais:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaisResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaisResponseDto>>> GetById(int id_pais)
    {
        var result = await _paisService.GetByIdAsync(id_pais);

        if (result is null)
            return NotFound(ApiResponse<PaisResponseDto>.Fail("Pa?s no encontrado."));

        return Ok(ApiResponse<PaisResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<PaisResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PaisResponseDto>>> Create([FromBody] PaisRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _paisService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_pais = result.IdPais, version = "1" },
            ApiResponse<PaisResponseDto>.Ok(result, "Pa�s creado correctamente."));
    }

    [HttpPut("{id_pais:int}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<PaisResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PaisResponseDto>>> Update(int id_pais, [FromBody] PaisUpdateRequestDto request)
    {
        var usuario = GetUsuario();
        var result = await _paisService.UpdateAsync(id_pais, request, usuario);

        if (result is null)
            return NotFound(ApiResponse<PaisResponseDto>.Fail("Pa�s no encontrado."));

        return Ok(ApiResponse<PaisResponseDto>.Ok(result, "Pa�s actualizado correctamente."));
    }

    [HttpDelete("{id_pais:int}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id_pais)
    {
        var usuario = GetUsuario();
        var result = await _paisService.DeleteAsync(id_pais, usuario);

        return Ok(ApiResponse<bool>.Ok(result, "Pa�s eliminado correctamente."));
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
}
