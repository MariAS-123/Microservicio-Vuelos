using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IReservaDataService
{
    Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(ReservaFiltroDataModel filtro);
    Task<ReservaDataModel?> GetByIdAsync(int id);
    Task<ReservaDataModel> CreateAsync(ReservaDataModel model);
    Task<ReservaDataModel?> UpdateAsync(ReservaDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}