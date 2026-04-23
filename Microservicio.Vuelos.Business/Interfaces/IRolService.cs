using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IRolService
{
    Task<DataPagedResult<RolResponseDto>> GetPagedAsync(RolFilterDto filter);

    Task<RolResponseDto?> GetByIdAsync(int idRol);

    Task<RolResponseDto> CreateAsync(RolRequestDto request, string creadoPorUsuario);

    Task<RolResponseDto?> UpdateAsync(int idRol, RolUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idRol, string modificadoPorUsuario);
}