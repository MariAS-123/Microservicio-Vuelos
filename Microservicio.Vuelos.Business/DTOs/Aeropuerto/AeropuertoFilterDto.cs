namespace Microservicio.Vuelos.Business.DTOs.Aeropuerto;

public class AeropuertoFilterDto
{
    public string? CodigoIata { get; set; }

    public string? CodigoIcao { get; set; }

    public string? Nombre { get; set; }

    public int? IdCiudad { get; set; }

    public int? IdPais { get; set; }

    public string? ZonaHoraria { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}