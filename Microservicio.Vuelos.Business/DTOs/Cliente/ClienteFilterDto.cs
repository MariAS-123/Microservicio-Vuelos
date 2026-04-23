namespace Microservicio.Vuelos.Business.DTOs.Cliente;

public class ClienteFilterDto
{
    public string? NumeroIdentificacion { get; set; }

    public string? Correo { get; set; }

    public string? Estado { get; set; } // ACT / INA

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}