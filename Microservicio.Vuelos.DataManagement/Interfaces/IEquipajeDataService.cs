using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IEquipajeDataService
{
    Task<DataPagedResult<EquipajeDataModel>> GetPagedAsync(EquipajeFiltroDataModel filtro);
    Task<EquipajeDataModel?> GetByIdAsync(int id);
    Task<EquipajeDataModel> CreateAsync(EquipajeDataModel model);
    Task<EquipajeDataModel?> UpdateAsync(EquipajeDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}