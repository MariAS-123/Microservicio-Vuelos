namespace Microservicio.Vuelos.DataManagement.Models;

public class BoletoFiltroDataModel
{
    public string? CodigoBoleto { get; set; }

    public int? IdReserva { get; set; }

    public int? IdVuelo { get; set; }

    public string? EstadoBoleto { get; set; } // ACTIVO / USADO / CANCELADO

    public bool IncluirEliminados { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}