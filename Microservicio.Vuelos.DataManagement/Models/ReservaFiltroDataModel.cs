namespace Microservicio.Vuelos.DataManagement.Models;

public class ReservaFiltroDataModel
{
    public string? CodigoReserva { get; set; }
    public int? IdCliente { get; set; }
    public int? IdPasajero { get; set; }
    public int? IdVuelo { get; set; }
    public string? EstadoReserva { get; set; }

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}