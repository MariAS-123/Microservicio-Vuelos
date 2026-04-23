using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IUsuarioRolService
{
    Task<DataPagedResult<UsuarioRolResponseDto>> GetPagedAsync(UsuarioRolFilterDto filter);

    Task<UsuarioRolResponseDto?> GetByIdAsync(int idUsuarioRol);

    Task<UsuarioRolResponseDto> CreateAsync(UsuarioRolRequestDto request, string creadoPorUsuario);

    Task<bool> DeleteAsync(int idUsuarioRol, string modificadoPorUsuario);
}