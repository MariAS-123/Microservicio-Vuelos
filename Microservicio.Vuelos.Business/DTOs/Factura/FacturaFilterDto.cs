namespace Microservicio.Vuelos.Business.DTOs.Factura;

public class FacturaFilterDto
{
    public string? NumeroFactura { get; set; }

    public int? IdCliente { get; set; }

    public int? IdReserva { get; set; }

    public string? Estado { get; set; } // ABI / APR / INA

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}