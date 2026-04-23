namespace Microservicio.Vuelos.DataManagement.Models;

public class AsientoFiltroDataModel
{
    public int? IdVuelo { get; set; }

    public string? NumeroAsiento { get; set; }

    public string? Clase { get; set; }

    public bool? Disponible { get; set; }

    public string? Posicion { get; set; }

    public string? Estado { get; set; }

    public decimal? PrecioExtraDesde { get; set; }

    public decimal? PrecioExtraHasta { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}