using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class AeropuertoValidator
{
    private static readonly string[] EstadosValidos = new[]
    {
        "ACTIVO",
        "INACTIVO"
    };

    public void ValidateRequest(AeropuertoRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al crear el aeropuerto.");
    }

    public void ValidateUpdate(AeropuertoUpdateRequestDto dto)
    {
        var errors = ValidateCommon(dto);
        ThrowIfAny(errors, "Error de validación al actualizar el aeropuerto.");
    }

    public void ValidateFilter(AeropuertoFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.CodigoIata))
        {
            var codigoIata = dto.CodigoIata.Trim();
            if (codigoIata.Length != 3)
                errors.Add("El código IATA debe tener exactamente 3 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.CodigoIcao))
        {
            var codigoIcao = dto.CodigoIcao.Trim();
            if (codigoIcao.Length != 4)
                errors.Add("El código ICAO debe tener exactamente 4 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre.Trim().Length > 150)
            errors.Add("El nombre no puede exceder 150 caracteres.");

        if (dto.IdCiudad.HasValue && dto.IdCiudad.Value <= 0)
            errors.Add("El id de la ciudad debe ser mayor que 0.");

        if (dto.IdPais.HasValue && dto.IdPais.Value <= 0)
            errors.Add("El id del país debe ser mayor que 0.");

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

        ThrowIfAny(errors, "Error de validación en el filtro de aeropuertos.");
    }

    private static List<string> ValidateCommon(AeropuertoRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.CodigoIata))
        {
            errors.Add("El código IATA es obligatorio.");
        }
        else
        {
            var codigoIata = dto.CodigoIata.Trim();
            if (codigoIata.Length != 3)
                errors.Add("El código IATA debe tener exactamente 3 caracteres.");

            if (!Regex.IsMatch(codigoIata, "^[A-Za-z]{3}$"))
                errors.Add("El código IATA debe contener exactamente 3 letras.");
        }

        if (!string.IsNullOrWhiteSpace(dto.CodigoIcao))
        {
            var codigoIcao = dto.CodigoIcao.Trim();
            if (codigoIcao.Length != 4)
                errors.Add("El código ICAO debe tener exactamente 4 caracteres.");

            if (!Regex.IsMatch(codigoIcao, "^[A-Za-z]{4}$"))
                errors.Add("El código ICAO debe contener exactamente 4 letras.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            errors.Add("El nombre del aeropuerto es obligatorio.");
        }
        else if (dto.Nombre.Trim().Length > 150)
        {
            errors.Add("El nombre del aeropuerto no puede exceder 150 caracteres.");
        }

        if (dto.IdCiudad <= 0)
            errors.Add("La ciudad es obligatoria.");

        if (dto.IdPais <= 0)
            errors.Add("El país es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.ZonaHoraria) && dto.ZonaHoraria.Trim().Length > 50)
            errors.Add("La zona horaria no puede exceder 50 caracteres.");

        if (dto.Latitud.HasValue && (dto.Latitud.Value < -90m || dto.Latitud.Value > 90m))
            errors.Add("La latitud debe estar entre -90 y 90.");

        if (dto.Longitud.HasValue && (dto.Longitud.Value < -180m || dto.Longitud.Value > 180m))
            errors.Add("La longitud debe estar entre -180 y 180.");

        return errors;
    }

    private static List<string> ValidateCommon(AeropuertoUpdateRequestDto dto)
    {
        var requestEquivalent = new AeropuertoRequestDto
        {
            CodigoIata = dto.CodigoIata,
            CodigoIcao = dto.CodigoIcao,
            Nombre = dto.Nombre,
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
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