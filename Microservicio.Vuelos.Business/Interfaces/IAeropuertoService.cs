using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IAeropuertoService
{
    Task<DataPagedResult<AeropuertoResponseDto>> GetPagedAsync(AeropuertoFilterDto filter);

    Task<AeropuertoResponseDto?> GetByIdAsync(int idAeropuerto);

    Task<AeropuertoResponseDto> CreateAsync(AeropuertoRequestDto request, string creadoPorUsuario);

    Task<AeropuertoResponseDto?> UpdateAsync(int idAeropuerto, AeropuertoUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idAeropuerto, string modificadoPorUsuario);
}