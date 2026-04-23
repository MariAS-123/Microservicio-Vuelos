// CiudadRequestDto.cs
namespace Microservicio.Vuelos.Business.DTOs.Ciudad;

public class CiudadRequestDto
{
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    // ✅ Sin Estado — se asigna ACTIVO automáticamente
}