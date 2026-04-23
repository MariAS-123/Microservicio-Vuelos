namespace Microservicio.Vuelos.Business.DTOs.Rol;

public class RolResponseDto
{
    public int IdRol { get; set; }

    public Guid RolGuid { get; set; }

    public string NombreRol { get; set; } = null!;

    public string? DescripcionRol { get; set; }

    public string EstadoRol { get; set; } = null!;

    public bool Activo { get; set; }
}