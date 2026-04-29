using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.AuditoriaLog;

public class AuditoriaLogFilterDto
{
    [FromQuery(Name = "tabla_afectada")]
    public string? TablaAfectada { get; set; }

    [FromQuery(Name = "operacion")]
    public string? Operacion { get; set; }

    [FromQuery(Name = "usuario_ejecutor")]
    public string? UsuarioEjecutor { get; set; }

    [FromQuery(Name = "fecha_desde")]
    public DateTime? FechaDesde { get; set; }

    [FromQuery(Name = "fecha_hasta")]
    public DateTime? FechaHasta { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
