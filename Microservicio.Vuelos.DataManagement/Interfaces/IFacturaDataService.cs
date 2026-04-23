using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IFacturaDataService
{
    Task<DataPagedResult<FacturaDataModel>> GetPagedAsync(FacturaFiltroDataModel filtro);
    Task<FacturaDataModel?> GetByIdAsync(int id);
    Task<FacturaDataModel> CreateAsync(FacturaDataModel model);
    Task<FacturaDataModel?> UpdateAsync(FacturaDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}