using System.Text.RegularExpressions;
using System.Linq;
using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class UsuarioAppValidator
{
    public void ValidateRequest(UsuarioAppRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente.HasValue && dto.IdCliente.Value <= 0)
            errors.Add("El id del cliente debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            errors.Add("El username es obligatorio.");
        }
        else
        {
            var username = dto.Username.Trim();

            if (username.Length < 3 || username.Length > 50)
                errors.Add("El username debe tener entre 3 y 50 caracteres.");

            if (!Regex.IsMatch(username, "^[A-Za-z0-9._-]+$"))
                errors.Add("El username solo puede contener letras, números, punto, guion y guion bajo.");
        }

        if (string.IsNullOrWhiteSpace(dto.Correo))
        {
            errors.Add("El correo es obligatorio.");
        }
        else
        {
            var correo = dto.Correo.Trim();

            if (correo.Length > 120)
                errors.Add("El correo no puede exceder 120 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo no tiene un formato válido.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            errors.Add("La contraseña es obligatoria.");
        }
        else
        {
            var password = dto.Password.Trim();

            if (password.Length < 8 || password.Length > 100)
                errors.Add("La contraseña debe tener entre 8 y 100 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al crear el usuario.");
    }

    public void ValidateUpdate(UsuarioAppUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente.HasValue && dto.IdCliente.Value <= 0)
            errors.Add("El id del cliente debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            errors.Add("El username es obligatorio.");
        }
        else
        {
            var username = dto.Username.Trim();

            if (username.Length < 3 || username.Length > 50)
                errors.Add("El username debe tener entre 3 y 50 caracteres.");

            if (!Regex.IsMatch(username, "^[A-Za-z0-9._-]+$"))
                errors.Add("El username solo puede contener letras, números, punto, guion y guion bajo.");
        }

        if (string.IsNullOrWhiteSpace(dto.Correo))
        {
            errors.Add("El correo es obligatorio.");
        }
        else
        {
            var correo = dto.Correo.Trim();

            if (correo.Length > 120)
                errors.Add("El correo no puede exceder 120 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo no tiene un formato válido.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            var password = dto.Password.Trim();

            if (password.Length < 8 || password.Length > 100)
                errors.Add("La contraseña debe tener entre 8 y 100 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al actualizar el usuario.");
    }

    public void ValidateFilter(UsuarioAppFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.Username) &&
            dto.Username.Trim().Length > 50)
        {
            errors.Add("El username no puede exceder 50 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Correo))
        {
            var correo = dto.Correo.Trim();

            if (correo.Length > 120)
                errors.Add("El correo no puede exceder 120 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo no tiene un formato válido.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de usuarios.");
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}