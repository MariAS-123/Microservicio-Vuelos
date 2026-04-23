using Microservicio.Vuelos.Business.DTOs.UsuarioApp;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IUsuarioAppService
{
    Task<DataPagedResult<UsuarioAppResponseDto>> GetPagedAsync(UsuarioAppFilterDto filter);

    Task<UsuarioAppResponseDto?> GetByIdAsync(int idUsuario);

    Task<UsuarioAppResponseDto> CreateAsync(UsuarioAppRequestDto request, string creadoPorUsuario);

    Task<UsuarioAppResponseDto?> UpdateAsync(int idUsuario, UsuarioAppUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idUsuario, string modificadoPorUsuario);
}