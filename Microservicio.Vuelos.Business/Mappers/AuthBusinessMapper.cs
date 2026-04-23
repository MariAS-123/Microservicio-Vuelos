using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers;

public static class AuthBusinessMapper
{
    public static LoginResponse ToLoginResponse(
        UsuarioAppDataModel user,
        string token,
        DateTime expiracion)
    {
        return new LoginResponse
        {
            Usuario = user.Username, // ajusta si tu campo es distinto
            Token = token,
            Expiracion = expiracion
        };
    }
}