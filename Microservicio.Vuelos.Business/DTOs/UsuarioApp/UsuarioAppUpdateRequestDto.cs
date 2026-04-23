// UsuarioAppUpdateRequestDto.cs
namespace Microservicio.Vuelos.Business.DTOs.UsuarioApp;

public class UsuarioAppUpdateRequestDto
{
    public int? IdCliente { get; set; }
    public string Username { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Password { get; set; }
    // ✅ Sin EstadoUsuario ni Activo
}