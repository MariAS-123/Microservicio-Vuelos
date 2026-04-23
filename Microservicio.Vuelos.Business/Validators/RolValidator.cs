using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.Business.Exceptions;
using System.Linq;

namespace Microservicio.Vuelos.Business.Validators;

public class RolValidator
{
    public void ValidateRequest(RolRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.NombreRol))
        {
            errors.Add("El nombre del rol es obligatorio.");
        }
        else if (dto.NombreRol.Trim().Length > 50)
        {
            errors.Add("El nombre del rol no puede exceder 50 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.DescripcionRol) &&
            dto.DescripcionRol.Trim().Length > 200)
        {
            errors.Add("La descripción del rol no puede exceder 200 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al crear el rol.");
    }

    public void ValidateUpdate(RolUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.NombreRol))
        {
            errors.Add("El nombre del rol es obligatorio.");
        }
        else if (dto.NombreRol.Trim().Length > 50)
        {
            errors.Add("El nombre del rol no puede exceder 50 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.DescripcionRol) &&
            dto.DescripcionRol.Trim().Length > 200)
        {
            errors.Add("La descripción del rol no puede exceder 200 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al actualizar el rol.");
    }

    public void ValidateFilter(RolFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.NombreRol) &&
            dto.NombreRol.Trim().Length > 50)
        {
            errors.Add("El nombre del rol no puede exceder 50 caracteres.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de roles.");
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}