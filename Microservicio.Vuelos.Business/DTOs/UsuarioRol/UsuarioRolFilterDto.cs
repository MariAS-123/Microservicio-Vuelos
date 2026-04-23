namespace Microservicio.Vuelos.Business.DTOs.UsuarioRol;

public class UsuarioRolFilterDto
{
    public int? IdUsuario { get; set; }

    public int? IdRol { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}