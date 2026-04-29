using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.Ciudad;

public class CiudadFilterDto
{
    [FromQuery(Name = "id_pais")]
    public int? IdPais { get; set; }

    [FromQuery(Name = "nombre")]
    public string? Nombre { get; set; }

    [FromQuery(Name = "codigo_postal")]
    public string? CodigoPostal { get; set; }

    [FromQuery(Name = "zona_horaria")]
    public string? ZonaHoraria { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
