using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Business.DTOs.Aeropuerto;

public class AeropuertoFilterDto
{
    [FromQuery(Name = "codigo_iata")]
    public string? CodigoIata { get; set; }

    [FromQuery(Name = "codigo_icao")]
    public string? CodigoIcao { get; set; }

    [FromQuery(Name = "nombre")]
    public string? Nombre { get; set; }

    [FromQuery(Name = "id_ciudad")]
    public int? IdCiudad { get; set; }

    [FromQuery(Name = "id_pais")]
    public int? IdPais { get; set; }

    [FromQuery(Name = "zona_horaria")]
    public string? ZonaHoraria { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}
