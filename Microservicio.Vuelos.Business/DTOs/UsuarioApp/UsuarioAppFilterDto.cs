namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppFilterDto
{
    public string? Username { get; set; }

    public string? Correo { get; set; }

    public bool? Activo { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}