// PaisRequestDto.cs
namespace Microservicio.Vuelos.Business.DTOs.Pais;

public class PaisRequestDto
{
    public string CodigoIso2 { get; set; } = null!;
    public string? CodigoIso3 { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Continente { get; set; }
    // ✅ Sin Estado — se asigna ACTIVO automáticamente
}