using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.AuditoriaLog;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auditoria")]
[Produces("application/json")]
[Authorize(Roles = "ADMINISTRADOR")]
public class AuditoriaLogController : ControllerBase
{
    private readonly IAuditoriaLogService _service;

    public AuditoriaLogController(IAuditoriaLogService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] AuditoriaLogFilterDto filter)
    {
        var result = await _service.GetPagedAsync(filter);
        return Ok(ApiResponse<object>.Ok(result, "Consulta de auditoría realizada correctamente."));
    }

    [HttpGet("{id_auditoria:long}")]
    [ProducesResponseType(typeof(ApiResponse<AuditoriaLogResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AuditoriaLogResponseDto>>> GetById(long id_auditoria)
    {
        var result = await _service.GetByIdAsync(id_auditoria);
        if (result is null)
            return NotFound(ApiResponse<AuditoriaLogResponseDto>.Fail("Registro de auditoría no encontrado."));

        return Ok(ApiResponse<AuditoriaLogResponseDto>.Ok(result, "Registro de auditoría obtenido correctamente."));
    }

}
