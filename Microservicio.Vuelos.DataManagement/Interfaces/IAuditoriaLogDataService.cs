using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IAuditoriaLogDataService
{
    Task<DataPagedResult<AuditoriaLogDataModel>> GetPagedAsync(AuditoriaLogFiltroDataModel filtro);

    Task<AuditoriaLogDataModel?> GetByIdAsync(long id);

    Task<AuditoriaLogDataModel> CreateAsync(AuditoriaLogDataModel model);

    Task<AuditoriaLogDataModel?> UpdateAsync(AuditoriaLogDataModel model);

    Task<bool> DeleteAsync(long id); // seguirá pero lanzará excepción
}