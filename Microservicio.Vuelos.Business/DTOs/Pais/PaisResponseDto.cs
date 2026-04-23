namespace Microservicio.Vuelos.Business.DTOs.Pais;

public class PaisResponseDto
{
    public int IdPais { get; set; }

    public string CodigoIso2 { get; set; } = null!;

    public string? CodigoIso3 { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Continente { get; set; }

    public string Estado { get; set; } = null!;
}