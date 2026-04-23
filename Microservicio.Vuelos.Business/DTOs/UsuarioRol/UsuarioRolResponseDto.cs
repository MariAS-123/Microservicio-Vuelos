namespace Microservicio.Vuelos.Business.DTOs.UsuarioRol;

public class UsuarioRolResponseDto
{
    public int IdUsuarioRol { get; set; }

    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string EstadoUsuarioRol { get; set; } = null!;

    public bool Activo { get; set; }
}