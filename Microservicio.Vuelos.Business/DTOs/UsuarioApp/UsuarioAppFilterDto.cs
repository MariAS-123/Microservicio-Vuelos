using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppFilterDto
{
    [FromQuery(Name = "username")]
    public string? Username { get; set; }

    [FromQuery(Name = "correo")]
    public string? Correo { get; set; }

    [FromQuery(Name = "activo")]
    public bool? Activo { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
