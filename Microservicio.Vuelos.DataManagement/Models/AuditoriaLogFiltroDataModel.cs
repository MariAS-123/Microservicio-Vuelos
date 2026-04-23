namespace Microservicio.Vuelos.DataManagement.Models;

public class AuditoriaLogFiltroDataModel
{
    public string? TablaAfectada { get; set; }

    public string? Operacion { get; set; }

    public string? UsuarioEjecutor { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}