namespace Microservicio.Vuelos.Business.DTOs.Rol;

public class RolFilterDto
{
    public string? NombreRol { get; set; }

    public string? EstadoRol { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}