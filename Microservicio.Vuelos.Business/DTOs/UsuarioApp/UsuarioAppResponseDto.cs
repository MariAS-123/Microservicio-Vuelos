namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppResponseDto
{
    public int IdUsuario { get; set; }

    public Guid UsuarioGuid { get; set; }

    public int? IdCliente { get; set; }

    public string Username { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime? FechaUltimoLogin { get; set; }

    public string EstadoUsuario { get; set; } = null!;

    public bool Activo { get; set; }
}