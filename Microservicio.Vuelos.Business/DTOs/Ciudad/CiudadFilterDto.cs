namespace Microservicio.Vuelos.Business.DTOs.Ciudad;

public class CiudadFilterDto
{
    public int? IdPais { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoPostal { get; set; }

    public string? ZonaHoraria { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}