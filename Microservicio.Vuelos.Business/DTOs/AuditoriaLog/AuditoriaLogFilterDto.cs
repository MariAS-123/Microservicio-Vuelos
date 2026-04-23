namespace Microservicio.Vuelos.Business.DTOs.AuditoriaLog;

public class AuditoriaLogFilterDto
{
    public string? TablaAfectada { get; set; }

    public string? Operacion { get; set; }

    public string? UsuarioEjecutor { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}