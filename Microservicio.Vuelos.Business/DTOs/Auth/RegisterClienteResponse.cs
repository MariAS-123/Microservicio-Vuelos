namespace Microservicio.Vuelos.Business.DTOs.Auth;

public class RegisterClienteResponse
{
    public int IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public string Username { get; set; } = null!;
    public string RolAsignado { get; set; } = null!;
}
