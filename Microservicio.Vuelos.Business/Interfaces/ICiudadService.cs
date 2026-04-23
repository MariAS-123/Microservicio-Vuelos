using Microservicio.Vuelos.Business.DTOs.Ciudad;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface ICiudadService
{
    Task<DataPagedResult<CiudadResponseDto>> GetPagedAsync(CiudadFilterDto filter);

    Task<CiudadResponseDto?> GetByIdAsync(int idCiudad);

    Task<CiudadResponseDto> CreateAsync(CiudadRequestDto request, string creadoPorUsuario);

    Task<CiudadResponseDto?> UpdateAsync(int idCiudad, CiudadUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idCiudad, string modificadoPorUsuario);
}