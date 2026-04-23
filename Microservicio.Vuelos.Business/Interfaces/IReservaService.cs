using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IReservaService
{
    Task<DataPagedResult<ReservaResponseDto>> GetPagedAsync(ReservaFilterDto filter);

    Task<ReservaResponseDto?> GetByIdAsync(int idReserva, int? idClienteDelToken, string rolDelToken); // ✅

    Task<ReservaResponseDto> CreateAsync(ReservaRequestDto request, string creadoPorUsuario);

    Task<ReservaResponseDto?> UpdateEstadoAsync(
        int idReserva,
        ReservaUpdateRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken);

    Task<bool> DeleteAsync(int idReserva, string modificadoPorUsuario);
}