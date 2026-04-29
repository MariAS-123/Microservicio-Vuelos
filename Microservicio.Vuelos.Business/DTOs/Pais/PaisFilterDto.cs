using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.Pais;

public class PaisFilterDto
{
    [FromQuery(Name = "nombre")]
    public string? Nombre { get; set; }

    [FromQuery(Name = "codigo_iso2")]
    public string? CodigoIso2 { get; set; }

    [FromQuery(Name = "codigo_iso3")]
    public string? CodigoIso3 { get; set; }

    [FromQuery(Name = "continente")]
    public string? Continente { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
