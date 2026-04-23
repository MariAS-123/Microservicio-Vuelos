namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppRequestDto
{
    public int? IdCliente { get; set; }

    public string Username { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Password { get; set; } = null!;
}