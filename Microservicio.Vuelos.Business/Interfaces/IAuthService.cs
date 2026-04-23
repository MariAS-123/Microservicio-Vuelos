using Microservicio.Vuelos.Business.DTOs.Auth;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
