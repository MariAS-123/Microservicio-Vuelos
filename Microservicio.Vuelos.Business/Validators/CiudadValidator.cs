using System;
using System.Collections.Generic;
using Microservicio.Vuelos.Business.DTOs.Ciudad;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class CiudadValidator
{
    private static readonly string[] EstadosValidos = new[]
    {
        "ACTIVO",
        "INACTIVO"
    };

    public void ValidateRequest(CiudadRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al crear la ciudad.");
    }

    public void ValidateUpdate(CiudadUpdateRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al actualizar la ciudad.");
    }

    public void ValidateFilter(CiudadFilterDto dto)
    {
        var errors = new List<string>();

        if (dto.IdPais.HasValue && dto.IdPais.Value <= 0)
            errors.Add("El id del país debe ser mayor que 0.");

        if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre.Trim().Length > 100)
            errors.Add("El nombre no puede exceder 100 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.CodigoPostal) && dto.CodigoPostal.Trim().Length > 20)
            errors.Add("El código postal no puede exceder 20 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.ZonaHoraria) && dto.ZonaHoraria.Trim().Length > 50)
            errors.Add("La zona horaria no puede exceder 50 caracteres.");

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

        ThrowIfAny(errors, "Error de validación en el filtro de ciudades.");
    }

    private static List<string> ValidateCommon(CiudadRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdPais <= 0)
            errors.Add("El país es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            errors.Add("El nombre de la ciudad es obligatorio.");
        }
        else if (dto.Nombre.Trim().Length > 100)
        {
            errors.Add("El nombre de la ciudad no puede exceder 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.CodigoPostal) && dto.CodigoPostal.Trim().Length > 20)
            errors.Add("El código postal no puede exceder 20 caracteres.");

        if (!string.IsNullOrWhiteSpace(dto.ZonaHoraria) && dto.ZonaHoraria.Trim().Length > 50)
            errors.Add("La zona horaria no puede exceder 50 caracteres.");

        if (dto.Latitud.HasValue && (dto.Latitud.Value < -90m || dto.Latitud.Value > 90m))
            errors.Add("La latitud debe estar entre -90 y 90.");

        if (dto.Longitud.HasValue && (dto.Longitud.Value < -180m || dto.Longitud.Value > 180m))
            errors.Add("La longitud debe estar entre -180 y 180.");

        return errors;
    }

    private static List<string> ValidateCommon(CiudadUpdateRequestDto dto)
    {
        var requestEquivalent = new CiudadRequestDto
        {
            IdPais = dto.IdPais,
            Nombre = dto.Nombre,
            CodigoPostal = dto.CodigoPostal,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud
        };

        return ValidateCommon(requestEquivalent);
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}