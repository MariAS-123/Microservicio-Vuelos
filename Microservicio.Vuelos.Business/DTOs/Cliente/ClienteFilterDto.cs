using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.Cliente;

public class ClienteFilterDto
{
    [FromQuery(Name = "numero_identificacion")]
    public string? NumeroIdentificacion { get; set; }

    [FromQuery(Name = "correo")]
    public string? Correo { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
