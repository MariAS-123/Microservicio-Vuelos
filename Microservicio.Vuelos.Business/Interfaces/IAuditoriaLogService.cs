using Microservicio.Vuelos.Business.DTOs.AuditoriaLog;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IAuditoriaLogService
{
    Task<DataPagedResult<AuditoriaLogResponseDto>> GetPagedAsync(AuditoriaLogFilterDto filter);

    Task<AuditoriaLogResponseDto?> GetByIdAsync(long idAuditoria);

    Task<AuditoriaLogResponseDto> CreateAsync(AuditoriaLogRequestDto request);

    Task<AuditoriaLogResponseDto?> UpdateAsync(long idAuditoria, AuditoriaLogUpdateRequestDto request);

    Task<bool> DeleteAsync(long idAuditoria);
}