namespace Microservicio.Vuelos.Business.DTOs.Boleto;

public class BoletoFilterDto
{
    public int? IdReserva { get; set; }

    public int? IdVuelo { get; set; }

    public string? CodigoBoleto { get; set; }

    public string? EstadoBoleto { get; set; } // ACTIVO / USADO / CANCELADO

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}