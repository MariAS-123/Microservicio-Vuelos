namespace Microservicio.Vuelos.DataManagement.Models;

public class FacturaFiltroDataModel
{
    public string? NumeroFactura { get; set; }

    public int? IdCliente { get; set; }

    public int? IdReserva { get; set; }

    public string? Estado { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}