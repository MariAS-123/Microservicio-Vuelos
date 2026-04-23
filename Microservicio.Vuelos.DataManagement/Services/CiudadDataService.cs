using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class CiudadDataService : ICiudadDataService
{
    private readonly ICiudadRepository _repo;
    private readonly IUnitOfWork _uow;

    public CiudadDataService(ICiudadRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<CiudadDataModel>> GetPagedAsync(CiudadFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable().Where(x => !x.Eliminado);

        if (filtro.IdPais.HasValue)
            query = query.Where(x => x.IdPais == filtro.IdPais.Value);

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
            query = query.Where(x => x.Nombre.Contains(filtro.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.CodigoPostal))
            query = query.Where(x => x.CodigoPostal != null &&
                                     x.CodigoPostal.Contains(filtro.CodigoPostal.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.ZonaHoraria))
            query = query.Where(x => x.ZonaHoraria != null &&
                                     x.ZonaHoraria.Contains(filtro.ZonaHoraria.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            query = query.Where(x => x.Estado == filtro.Estado.Trim().ToUpperInvariant());

        query = query.OrderBy(x => x.Nombre).ThenBy(x => x.IdCiudad);

        var totalRecords = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(CiudadDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<CiudadDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = totalRecords
        };
    }

    public async Task<CiudadDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);
        return entity is null || entity.Eliminado ? null : CiudadDataMapper.ToDataModel(entity);
    }

    public async Task<CiudadDataModel?> GetByNombreAndPaisAsync(int idPais, string nombre)
    {
        var nombreNormalizado = nombre.Trim();
        var data = await _repo.ObtenerTodosAsync();

        var entity = data.FirstOrDefault(x =>
            !x.Eliminado &&
            x.IdPais == idPais &&
            x.Nombre.Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase));

        return entity is null ? null : CiudadDataMapper.ToDataModel(entity);
    }

    public async Task<IReadOnlyList<CiudadDataModel>> GetByPaisAsync(int idPais)
    {
        var data = await _repo.ObtenerTodosAsync();

        return data
            .Where(x => !x.Eliminado && x.IdPais == idPais)
            .OrderBy(x => x.Nombre)
            .Select(CiudadDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<CiudadDataModel> CreateAsync(CiudadDataModel model)
    {
        var entity = CiudadDataMapper.ToEntity(model);
        entity.Eliminado = false;
        entity.Estado = "ACTIVO";

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return CiudadDataMapper.ToDataModel(entity);
    }

    public async Task<CiudadDataModel?> UpdateAsync(CiudadDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdCiudad); // ✅ con tracking
        if (entity is null || entity.Eliminado)
            return null;

        CiudadDataMapper.UpdateEntity(entity, model);
        await _uow.SaveChangesAsync();

        return CiudadDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking
        if (entity is null || entity.Eliminado)
            return false;

        entity.Eliminado = true;
        entity.Estado = "INACTIVO";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();
        return true;
    }
}