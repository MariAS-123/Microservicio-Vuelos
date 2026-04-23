using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class PaisDataService : IPaisDataService
{
    private readonly IPaisRepository _repo;
    private readonly IUnitOfWork _uow;

    public PaisDataService(IPaisRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<PaisDataModel>> GetPagedAsync(PaisFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.Eliminado);

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
        {
            var nombre = filtro.Nombre.Trim();
            query = query.Where(x => x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.CodigoIso2))
        {
            var iso2 = filtro.CodigoIso2.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoIso2 == iso2);
        }

        if (!string.IsNullOrWhiteSpace(filtro.CodigoIso3))
        {
            var iso3 = filtro.CodigoIso3.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoIso3 == iso3);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Continente))
        {
            var continente = filtro.Continente.Trim();
            query = query.Where(x => x.Continente != null &&
                                     x.Continente.Contains(continente, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            var estado = filtro.Estado.Trim().ToUpperInvariant();
            query = query.Where(x => x.Estado == estado);
        }

        query = query.OrderBy(x => x.Nombre).ThenBy(x => x.IdPais);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(PaisDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<PaisDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<PaisDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);
        return entity is null || entity.Eliminado ? null : PaisDataMapper.ToDataModel(entity);
    }

    public async Task<PaisDataModel?> GetByCodigoIso2Async(string codigoIso2)
    {
        if (string.IsNullOrWhiteSpace(codigoIso2))
            return null;

        var iso2 = codigoIso2.Trim().ToUpperInvariant();
        var data = await _repo.ObtenerTodosAsync();
        var entity = data.FirstOrDefault(x => !x.Eliminado && x.CodigoIso2 == iso2);

        return entity is null ? null : PaisDataMapper.ToDataModel(entity);
    }

    public async Task<PaisDataModel?> GetByCodigoIso3Async(string codigoIso3)
    {
        if (string.IsNullOrWhiteSpace(codigoIso3))
            return null;

        var iso3 = codigoIso3.Trim().ToUpperInvariant();
        var data = await _repo.ObtenerTodosAsync();
        var entity = data.FirstOrDefault(x => !x.Eliminado && x.CodigoIso3 == iso3);

        return entity is null ? null : PaisDataMapper.ToDataModel(entity);
    }

    public async Task<PaisDataModel?> GetByNombreAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return null;

        var nombreNormalizado = nombre.Trim();
        var data = await _repo.ObtenerTodosAsync();
        var entity = data.FirstOrDefault(x =>
            !x.Eliminado &&
            x.Nombre.Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase));

        return entity is null ? null : PaisDataMapper.ToDataModel(entity);
    }

    public async Task<PaisDataModel> CreateAsync(PaisDataModel model)
    {
        var entity = PaisDataMapper.ToEntity(model);
        entity.Eliminado = false;
        entity.Estado = "ACTIVO";

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return PaisDataMapper.ToDataModel(entity);
    }

    public async Task<PaisDataModel?> UpdateAsync(PaisDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdPais); // ✅ con tracking
        if (entity is null || entity.Eliminado)
            return null;

        PaisDataMapper.UpdateEntity(entity, model);
        await _uow.SaveChangesAsync();

        return PaisDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking
        if (entity is null || entity.Eliminado)
            return false;

        entity.Eliminado = true;
        entity.Estado = "INACTIVO";

        await _uow.SaveChangesAsync();
        return true;
    }
}