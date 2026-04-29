using System.Text.RegularExpressions;
using System.Linq;
using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class ClienteValidator
{
    private static readonly string[] TiposIdentificacionValidos =
    [
        "CEDULA",
        "PASAPORTE",
        "RUC",
        "OTRO"
    ];

    private static readonly string[] GenerosValidos =
    [
        "MASCULINO",
        "FEMENINO",
        "OTRO"
    ];

    public void ValidateRequest(ClienteRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al crear el cliente.");
    }

    public void ValidateUpdate(ClienteUpdateRequestDto dto)
    {
        var errors = ValidateCommon(dto);

        ThrowIfAny(errors, "Error de validación al actualizar el cliente.");
    }

    public void ValidateFilter(ClienteFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.NumeroIdentificacion) && dto.NumeroIdentificacion.Trim().Length > 30)
            errors.Add("El número de identificación no puede exceder 30 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.Correo))
        {
            var correo = dto.Correo.Trim();

            if (correo.Length > 150)
                errors.Add("El correo no puede exceder 150 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo no tiene un formato válido.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de clientes.");
    }

    private static List<string> ValidateCommon(ClienteRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.TipoIdentificacion))
        {
            errors.Add("El tipo de identificación es obligatorio.");
        }
        else
        {
            var tipoIdentificacion = dto.TipoIdentificacion.Trim().ToUpperInvariant();

            if (!TiposIdentificacionValidos.Contains(tipoIdentificacion))
                errors.Add("El tipo de identificación debe ser CEDULA, PASAPORTE, RUC u OTRO.");
        }

        if (string.IsNullOrWhiteSpace(dto.NumeroIdentificacion))
        {
            errors.Add("El número de identificación es obligatorio.");
        }
        else if (dto.NumeroIdentificacion.Trim().Length > 30)
        {
            errors.Add("El número de identificación no puede exceder 30 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nombres))
        {
            errors.Add("Los nombres son obligatorios.");
        }
        else if (dto.Nombres.Trim().Length > 160)
        {
            errors.Add("Los nombres no pueden exceder 160 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Apellidos) && dto.Apellidos.Trim().Length > 160)
            errors.Add("Los apellidos no pueden exceder 160 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.RazonSocial) && dto.RazonSocial.Trim().Length > 200)
            errors.Add("La razón social no puede exceder 200 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Correo))
        {
            errors.Add("El correo es obligatorio.");
        }
        else
        {
            var correo = dto.Correo.Trim();

            if (correo.Length > 150)
                errors.Add("El correo no puede exceder 150 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo no tiene un formato válido.");
        }

        if (string.IsNullOrWhiteSpace(dto.Telefono))
        {
            errors.Add("El teléfono es obligatorio.");
        }
        else if (dto.Telefono.Trim().Length > 30)
        {
            errors.Add("El teléfono no puede exceder 30 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Direccion))
        {
            errors.Add("La dirección es obligatoria.");
        }
        else if (dto.Direccion.Trim().Length > 250)
        {
            errors.Add("La dirección no puede exceder 250 caracteres.");
        }

        if (dto.IdCiudadResidencia <= 0)
            errors.Add("La ciudad de residencia es obligatoria.");

        if (dto.IdPaisNacionalidad <= 0)
            errors.Add("El país de nacionalidad es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.Genero))
        {
            var genero = dto.Genero.Trim().ToUpperInvariant();

            if (!GenerosValidos.Contains(genero))
                errors.Add("El género debe ser MASCULINO, FEMENINO u OTRO.");
        }

        var tipo = dto.TipoIdentificacion?.Trim().ToUpperInvariant();
        if (tipo == "RUC" && string.IsNullOrWhiteSpace(dto.RazonSocial))
            errors.Add("La razón social es obligatoria cuando el tipo de identificación es RUC.");

        return errors;
    }

    private static List<string> ValidateCommon(ClienteUpdateRequestDto dto)
    {
        var requestEquivalent = new ClienteRequestDto
        {
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            RazonSocial = dto.RazonSocial,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            IdCiudadResidencia = dto.IdCiudadResidencia,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            FechaNacimiento = dto.FechaNacimiento,
            Genero = dto.Genero
        };

        return ValidateCommon(requestEquivalent);
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
