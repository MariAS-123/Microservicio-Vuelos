using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IPaisDataService
{
    Task<DataPagedResult<PaisDataModel>> GetPagedAsync(PaisFiltroDataModel filtro);
    Task<PaisDataModel?> GetByIdAsync(int id);
    Task<PaisDataModel> CreateAsync(PaisDataModel model);
    Task<PaisDataModel?> UpdateAsync(PaisDataModel model);
    Task<PaisDataModel?> GetByCodigoIso2Async(string codigoIso2);
    Task<PaisDataModel?> GetByCodigoIso3Async(string codigoIso3);
    Task<PaisDataModel?> GetByNombreAsync(string nombre);
    Task<bool> DeleteAsync(int id);
}