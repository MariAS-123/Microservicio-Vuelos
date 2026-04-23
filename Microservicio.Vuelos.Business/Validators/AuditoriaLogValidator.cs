using Microservicio.Vuelos.Business.DTOs.AuditoriaLog;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class AuditoriaLogValidator
{
    private static readonly string[] OperacionesValidas =
    [
        "INSERT",
        "UPDATE",
        "DELETE",
        "LOGIN",
        "LOGOUT"
    ];

    public void ValidateRequest(AuditoriaLogRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.TablaAfectada))
        {
            errors.Add("La tabla afectada es obligatoria.");
        }
        else if (dto.TablaAfectada.Trim().Length > 100)
        {
            errors.Add("La tabla afectada no puede exceder 100 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Operacion))
        {
            errors.Add("La operación es obligatoria.");
        }
        else
        {
            var operacion = dto.Operacion.Trim().ToUpperInvariant();

            if (!OperacionesValidas.Contains(operacion))
                errors.Add("La operación debe ser INSERT, UPDATE, DELETE, LOGIN o LOGOUT.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IdRegistroAfectado) &&
            dto.IdRegistroAfectado.Trim().Length > 100)
        {
            errors.Add("El id del registro afectado no puede exceder 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.UsuarioEjecutor))
        {
            if (dto.UsuarioEjecutor.Trim().Length > 100)
                errors.Add("El usuario ejecutor no puede exceder 100 caracteres.");
        }
        else
        {
            errors.Add("El usuario ejecutor es obligatorio.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IpOrigen) &&
            dto.IpOrigen.Trim().Length > 45)
        {
            errors.Add("La IP de origen no puede exceder 45 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al registrar auditoría.");
    }

    public void ValidateUpdate(AuditoriaLogUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.DatosAnteriores) &&
            dto.DatosAnteriores.Length > 4000)
        {
            errors.Add("Los datos anteriores no pueden exceder 4000 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.DatosNuevos) &&
            dto.DatosNuevos.Length > 4000)
        {
            errors.Add("Los datos nuevos no pueden exceder 4000 caracteres.");
        }

        ThrowIfAny(errors, "Error de validación al actualizar auditoría.");
    }

    public void ValidateFilter(AuditoriaLogFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.TablaAfectada) &&
            dto.TablaAfectada.Trim().Length > 100)
        {
            errors.Add("La tabla afectada no puede exceder 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Operacion))
        {
            var operacion = dto.Operacion.Trim().ToUpperInvariant();

            if (!OperacionesValidas.Contains(operacion))
                errors.Add("La operación debe ser INSERT, UPDATE, DELETE, LOGIN o LOGOUT.");
        }

        if (!string.IsNullOrWhiteSpace(dto.UsuarioEjecutor) &&
            dto.UsuarioEjecutor.Trim().Length > 100)
        {
            errors.Add("El usuario ejecutor no puede exceder 100 caracteres.");
        }

        if (dto.FechaDesde.HasValue && dto.FechaHasta.HasValue &&
            dto.FechaHasta.Value < dto.FechaDesde.Value)
        {
            errors.Add("La fecha hasta no puede ser menor que la fecha desde.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de auditoría.");
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}