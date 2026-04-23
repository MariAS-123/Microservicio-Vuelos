using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IRolDataService
{
    Task<DataPagedResult<RolDataModel>> GetPagedAsync(RolFiltroDataModel filtro);

    Task<RolDataModel?> GetByIdAsync(int id);

    Task<RolDataModel?> GetByNombreAsync(string nombre);

    Task<RolDataModel> CreateAsync(RolDataModel model);

    Task<RolDataModel?> UpdateAsync(RolDataModel model);

    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}