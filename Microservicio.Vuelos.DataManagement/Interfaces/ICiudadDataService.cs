using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface ICiudadDataService
{
    Task<DataPagedResult<CiudadDataModel>> GetPagedAsync(CiudadFiltroDataModel filtro);

    Task<CiudadDataModel?> GetByIdAsync(int id);

    Task<CiudadDataModel?> GetByNombreAndPaisAsync(int idPais, string nombre);

    Task<IReadOnlyList<CiudadDataModel>> GetByPaisAsync(int idPais);

    Task<CiudadDataModel> CreateAsync(CiudadDataModel model);

    Task<CiudadDataModel?> UpdateAsync(CiudadDataModel model);

    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}