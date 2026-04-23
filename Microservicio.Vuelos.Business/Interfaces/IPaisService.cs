using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IPaisService
{
    Task<DataPagedResult<PaisResponseDto>> GetPagedAsync(PaisFilterDto filter);

    Task<PaisResponseDto?> GetByIdAsync(int idPais);

    Task<PaisResponseDto> CreateAsync(PaisRequestDto request, string creadoPorUsuario);

    Task<PaisResponseDto?> UpdateAsync(int idPais, PaisUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idPais, string modificadoPorUsuario);
}