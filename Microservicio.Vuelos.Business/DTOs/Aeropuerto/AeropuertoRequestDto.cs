// AeropuertoRequestDto.cs
namespace Microservicio.Vuelos.Business.DTOs.Aeropuerto;

public class AeropuertoRequestDto
{
    public string CodigoIata { get; set; } = null!;
    public string? CodigoIcao { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    // ✅ Sin Estado — se asigna ACTIVO automáticamente
}