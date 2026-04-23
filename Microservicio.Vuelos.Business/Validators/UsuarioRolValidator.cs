using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators;

public class UsuarioRolValidator
{
    private static readonly string[] EstadosValidos = new[]
    {
        "ACT",
        "INA"
    };

    public void ValidateRequest(UsuarioRolRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdUsuario <= 0)
            errors.Add("El usuario es obligatorio.");

        if (dto.IdRol <= 0)
            errors.Add("El rol es obligatorio.");

        ThrowIfAny(errors, "Error de validación al asignar el rol al usuario.");
    }

    // ✅ ValidateUpdate eliminado — UpdateAsync ya no existe en el service

    public void ValidateFilter(UsuarioRolFilterDto dto)
    {
        var errors = new List<string>();

        if (dto.IdUsuario.HasValue && dto.IdUsuario.Value <= 0)
            errors.Add("El id del usuario debe ser mayor que 0.");

        if (dto.IdRol.HasValue && dto.IdRol.Value <= 0)
            errors.Add("El id del rol debe ser mayor que 0.");

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 || dto.PageSize > 200)
            errors.Add("El tamaño de página debe estar entre 1 y 200.");

        ThrowIfAny(errors, "Error de validación en el filtro de usuario-rol.");
    }

    private static void ThrowIfAny(List<string> errors, string message)
    {
        if (errors.Count > 0)
            throw new ValidationException(message, errors);
    }
}