using System.Collections.Generic;
using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class PaisValidator
{
    private static readonly string[] EstadosValidos = new[]
    {
        "ACTIVO",
        "INACTIVO"
    };

    public void ValidateRequest(PaisRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al crear el país.");
    }

    public void ValidateUpdate(PaisUpdateRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al actualizar el país.");
    }

    public void ValidateFilter(PaisFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.CodigoIso2) && dto.CodigoIso2.Trim().Length != 2)
            errors.Add("El código ISO2 debe tener exactamente 2 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.CodigoIso3) && dto.CodigoIso3.Trim().Length != 3)
            errors.Add("El código ISO3 debe tener exactamente 3 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre.Trim().Length > 100)
            errors.Add("El nombre no puede exceder 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.Continente) && dto.Continente.Trim().Length > 50)
            errors.Add("El continente no puede exceder 50 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            var estado = dto.Estado.Trim().ToUpperInvariant();
            if (Array.IndexOf(EstadosValidos, estado) < 0)
                errors.Add("El estado debe ser ACTIVO o INACTIVO.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de países.");
    }

    private static List<string> ValidateCommon(PaisRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.CodigoIso2))
        {
            errors.Add("El código ISO2 es obligatorio.");
        }
        else
        {
            var codigoIso2 = dto.CodigoIso2.Trim();
            if (codigoIso2.Length != 2)
                errors.Add("El código ISO2 debe tener exactamente 2 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.CodigoIso3))
        {
            var codigoIso3 = dto.CodigoIso3.Trim();
            if (codigoIso3.Length != 3)
                errors.Add("El código ISO3 debe tener exactamente 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            errors.Add("El nombre es obligatorio.");
        }
        else if (dto.Nombre.Trim().Length > 100)
        {
            errors.Add("El nombre no puede exceder 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Continente) && dto.Continente.Trim().Length > 50)
            errors.Add("El continente no puede exceder 50 caracteres.");

        return errors;
    }

    private static List<string> ValidateCommon(PaisUpdateRequestDto dto)
    {
        var requestEquivalent = new PaisRequestDto
        {
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Nombre = dto.Nombre,
            Continente = dto.Continente
        };

        return ValidateCommon(requestEquivalent);
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}