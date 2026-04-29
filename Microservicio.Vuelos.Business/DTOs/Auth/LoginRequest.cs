using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.Auth;

public class LoginRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}
