namespace Microservicio.Vuelos.Business.DTOs.Pais;

public class PaisFilterDto
{
    public string? Nombre { get; set; }

    public string? CodigoIso2 { get; set; }

    public string? CodigoIso3 { get; set; }

    public string? Continente { get; set; }

    public string? Estado { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}