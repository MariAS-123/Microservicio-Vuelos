namespace Microservicio.Vuelos.DataManagement.Models;

public class EscalaFiltroDataModel
{
    public int? IdVuelo { get; set; }

    public int? IdAeropuerto { get; set; }

    public int? Orden { get; set; }

    public string? TipoEscala { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaLlegadaDesde { get; set; }

    public DateTime? FechaLlegadaHasta { get; set; }

    public DateTime? FechaSalidaDesde { get; set; }

    public DateTime? FechaSalidaHasta { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}