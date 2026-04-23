using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class AeropuertoDataService : IAeropuertoDataService
{
    private readonly IAeropuertoRepository _repo;
    private readonly IUnitOfWork _uow;

    public AeropuertoDataService(IAeropuertoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<AeropuertoDataModel>> GetPagedAsync(AeropuertoFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable().Where(x => !x.Eliminado);

        if (!string.IsNullOrWhiteSpace(filtro.CodigoIata))
        {
            var codigoIata = filtro.CodigoIata.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoIata.Contains(codigoIata, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.CodigoIcao))
        {
            var codigoIcao = filtro.CodigoIcao.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoIcao != null &&
                                     x.CodigoIcao.Contains(codigoIcao, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
        {
            var nombre = filtro.Nombre.Trim();
            query = query.Where(x => x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        }

        if (filtro.IdCiudad.HasValue)
            query = query.Where(x => x.IdCiudad == filtro.IdCiudad.Value);

        if (filtro.IdPais.HasValue)
            query = query.Where(x => x.IdPais == filtro.IdPais.Value);

        if (!string.IsNullOrWhiteSpace(filtro.ZonaHoraria))
        {
            var zonaHoraria = filtro.ZonaHoraria.Trim();
            query = query.Where(x => x.ZonaHoraria != null &&
                                     x.ZonaHoraria.Contains(zonaHoraria, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            query = query.Where(x => x.Estado == filtro.Estado.Trim().ToUpperInvariant());

        query = query.OrderBy(x => x.Nombre).ThenBy(x => x.CodigoIata);

        var totalRecords = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(AeropuertoDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<AeropuertoDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = totalRecords
        };
    }

    public async Task<AeropuertoDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);
        return entity is null || entity.Eliminado ? null : AeropuertoDataMapper.ToDataModel(entity);
    }

    public async Task<AeropuertoDataModel?> GetByCodigoIataAsync(string codigoIata)
    {
        if (string.IsNullOrWhiteSpace(codigoIata))
            return null;

        var entity = await _repo.ObtenerPorCodigoIataAsync(codigoIata.Trim().ToUpperInvariant());
        return entity is null || entity.Eliminado ? null : AeropuertoDataMapper.ToDataModel(entity);
    }

    public async Task<AeropuertoDataModel> CreateAsync(AeropuertoDataModel model)
    {
        var entity = AeropuertoDataMapper.ToEntity(model);
        entity.Eliminado = false;
        entity.Estado = "ACTIVO";

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return AeropuertoDataMapper.ToDataModel(entity);
    }

    public async Task<AeropuertoDataModel?> UpdateAsync(AeropuertoDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdAeropuerto); // ✅
        if (entity is null || entity.Eliminado)
            return null;

        AeropuertoDataMapper.UpdateEntity(entity, model);
        await _uow.SaveChangesAsync();

        return AeropuertoDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅
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