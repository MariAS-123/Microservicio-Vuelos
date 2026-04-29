using System.Text.RegularExpressions;
using System.Linq;
using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class PasajeroValidator
{
    private static readonly string[] TiposDocumentoValidos =
    [
        "CEDULA",
        "PASAPORTE",
        "RUC",
        "OTRO"
    ];

    public void ValidateRequest(PasajeroRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al crear el pasajero.");
    }

    public void ValidateUpdate(PasajeroUpdateRequestDto dto)
    {
        var errors = ValidateCommon(dto);

        ThrowIfAny(errors, "Error de validación al actualizar el pasajero.");
    }

    public void ValidateFilter(PasajeroFilterDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente.HasValue && dto.IdCliente.Value <= 0)
            errors.Add("El id del cliente debe ser mayor que 0.");

        if (!string.IsNullOrWhiteSpace(dto.NombrePasajero) && dto.NombrePasajero.Trim().Length > 100)
            errors.Add("El nombre del pasajero no puede exceder 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.ApellidoPasajero) && dto.ApellidoPasajero.Trim().Length > 100)
            errors.Add("El apellido del pasajero no puede exceder 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.TipoDocumentoPasajero))
        {
            var tipo = dto.TipoDocumentoPasajero.Trim().ToUpperInvariant();

            if (!TiposDocumentoValidos.Contains(tipo))
                errors.Add("El tipo de documento del pasajero debe ser CEDULA, PASAPORTE, RUC u OTRO.");
        }

        if (!string.IsNullOrWhiteSpace(dto.NumeroDocumentoPasajero) && dto.NumeroDocumentoPasajero.Trim().Length > 30)
            errors.Add("El número de documento del pasajero no puede exceder 30 caracteres.");

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de pasajeros.");
    }

    private static List<string> ValidateCommon(PasajeroRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente.HasValue && dto.IdCliente.Value <= 0)
            errors.Add("El id del cliente debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(dto.NombrePasajero))
        {
            errors.Add("El nombre del pasajero es obligatorio.");
        }
        else if (dto.NombrePasajero.Trim().Length > 100)
        {
            errors.Add("El nombre del pasajero no puede exceder 100 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.ApellidoPasajero))
        {
            errors.Add("El apellido del pasajero es obligatorio.");
        }
        else if (dto.ApellidoPasajero.Trim().Length > 100)
        {
            errors.Add("El apellido del pasajero no puede exceder 100 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.TipoDocumentoPasajero))
        {
            errors.Add("El tipo de documento del pasajero es obligatorio.");
        }
        else
        {
            var tipo = dto.TipoDocumentoPasajero.Trim().ToUpperInvariant();

            if (!TiposDocumentoValidos.Contains(tipo))
                errors.Add("El tipo de documento del pasajero debe ser CEDULA, PASAPORTE, RUC u OTRO.");
        }

        if (string.IsNullOrWhiteSpace(dto.NumeroDocumentoPasajero))
        {
            errors.Add("El número de documento del pasajero es obligatorio.");
        }
        else if (dto.NumeroDocumentoPasajero.Trim().Length > 30)
        {
            errors.Add("El número de documento del pasajero no puede exceder 30 caracteres.");
        }

        if (dto.IdPaisNacionalidad.HasValue && dto.IdPaisNacionalidad.Value <= 0)
            errors.Add("El país de nacionalidad del pasajero debe ser mayor que 0.");

        if (!string.IsNullOrWhiteSpace(dto.EmailContactoPasajero))
        {
            var correo = dto.EmailContactoPasajero.Trim();

            if (correo.Length > 150)
                errors.Add("El correo de contacto del pasajero no puede exceder 150 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo de contacto del pasajero no tiene un formato válido.");
        }

        if (!string.IsNullOrWhiteSpace(dto.TelefonoContactoPasajero) && dto.TelefonoContactoPasajero.Trim().Length > 20)
            errors.Add("El teléfono de contacto del pasajero no puede exceder 20 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.GeneroPasajero) && dto.GeneroPasajero.Trim().Length > 20)
            errors.Add("El género del pasajero no puede exceder 20 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.ObservacionesPasajero) && dto.ObservacionesPasajero.Trim().Length > 255)
            errors.Add("Las observaciones del pasajero no pueden exceder 255 caracteres.");

        return errors;
    }

    private static List<string> ValidateCommon(PasajeroUpdateRequestDto dto)
    {
        var requestEquivalent = new PasajeroRequestDto
        {
            IdCliente = dto.IdCliente,
            NombrePasajero = dto.NombrePasajero,
            ApellidoPasajero = dto.ApellidoPasajero,
            TipoDocumentoPasajero = dto.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = dto.NumeroDocumentoPasajero,
            FechaNacimientoPasajero = dto.FechaNacimientoPasajero,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            EmailContactoPasajero = dto.EmailContactoPasajero,
            TelefonoContactoPasajero = dto.TelefonoContactoPasajero,
            GeneroPasajero = dto.GeneroPasajero,
            RequiereAsistencia = dto.RequiereAsistencia,
            ObservacionesPasajero = dto.ObservacionesPasajero
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
