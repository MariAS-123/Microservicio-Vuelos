using Microservicio.Vuelos.Business.DTOs.Auth;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class AuthValidator
{
    public void ValidateLogin(LoginRequest request)
    {
        var errors = new List<string>();

        if (request == null)
        {
            errors.Add("La solicitud de login es obligatoria.");
            ThrowIfAny(errors);
            return;
        }

        if (string.IsNullOrWhiteSpace(request.Usuario))
        {
            errors.Add("El usuario es obligatorio.");
        }
        else if (request.Usuario.Length > 50)
        {
            errors.Add("El usuario no puede exceder 50 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("La contraseña es obligatoria.");
        }
        else if (request.Password.Length > 100)
        {
            errors.Add("La contraseña no puede exceder 100 caracteres.");
        }

        ThrowIfAny(errors);
    }

    private static void ThrowIfAny(List<string> errors)
    {
        if (errors.Any())
            throw new ValidationException("Error de validación en login.", errors);
    }
}