// RolUpdateRequestDto.cs
namespace Microservicio.Vuelos.Business.DTOs.Rol;

public class RolUpdateRequestDto
{
    public string NombreRol { get; set; } = null!;
    public string? DescripcionRol { get; set; }
    // ✅ Sin EstadoRol ni Activo — estos los maneja el DELETE
}