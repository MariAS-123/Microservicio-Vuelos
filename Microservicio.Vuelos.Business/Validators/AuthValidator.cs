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

    public void ValidateRegisterCliente(RegisterClienteRequest request)
    {
        var errors = new List<string>();

        if (request == null)
        {
            errors.Add("La solicitud de registro es obligatoria.");
            ThrowIfAny(errors, "Error de validación en registro.");
            return;
        }

        if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
            errors.Add("El tipo de identificación es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
            errors.Add("El número de identificación es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Nombres))
            errors.Add("Los nombres son obligatorios.");

        if (string.IsNullOrWhiteSpace(request.Correo))
            errors.Add("El correo es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Telefono))
            errors.Add("El teléfono es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Direccion))
            errors.Add("La dirección es obligatoria.");

        if (request.IdCiudadResidencia <= 0)
            errors.Add("La ciudad de residencia es obligatoria.");

        if (request.IdPaisNacionalidad <= 0)
            errors.Add("El país de nacionalidad es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Username))
            errors.Add("El username es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("La contraseña es obligatoria.");
        }
        else if (request.Password.Length < 8)
        {
            errors.Add("La contraseña debe tener al menos 8 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación en registro.");
    }

    private static void ThrowIfAny(List<string> errors, string message = "Error de validación en login.")
    {
        if (errors.Any())
            throw new ValidationException(message, errors);
    }
}