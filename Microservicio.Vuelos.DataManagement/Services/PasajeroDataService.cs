using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class PasajeroDataService : IPasajeroDataService
{
    private readonly IPasajeroRepository _repo;
    private readonly IUnitOfWork _uow;

    public PasajeroDataService(IPasajeroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<PasajeroDataModel>> GetPagedAsync(PasajeroFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (filtro.IdCliente.HasValue)
            query = query.Where(x => x.IdCliente == filtro.IdCliente.Value);

        if (!string.IsNullOrWhiteSpace(filtro.NombrePasajero))
        {
            var nombre = filtro.NombrePasajero.Trim();
            query = query.Where(x => x.NombrePasajero.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.ApellidoPasajero))
        {
            var apellido = filtro.ApellidoPasajero.Trim();
            query = query.Where(x => x.ApellidoPasajero.Contains(apellido, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.TipoDocumentoPasajero))
        {
            var tipoDocumento = filtro.TipoDocumentoPasajero.Trim().ToUpperInvariant();
            query = query.Where(x => x.TipoDocumentoPasajero == tipoDocumento);
        }

        if (!string.IsNullOrWhiteSpace(filtro.NumeroDocumentoPasajero))
        {
            var numeroDocumento = filtro.NumeroDocumentoPasajero.Trim();
            query = query.Where(x => x.NumeroDocumentoPasajero.Contains(numeroDocumento));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            var estado = filtro.Estado.Trim().ToUpperInvariant();
            query = query.Where(x => x.Estado == estado);
        }

        if (filtro.RequiereAsistencia.HasValue)
            query = query.Where(x => x.RequiereAsistencia == filtro.RequiereAsistencia.Value);

        query = query
            .OrderBy(x => x.NombrePasajero)
            .ThenBy(x => x.ApellidoPasajero)
            .ThenBy(x => x.IdPasajero);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(PasajeroDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<PasajeroDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<PasajeroDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity is null || entity.EsEliminado)
            return null;

        return PasajeroDataMapper.ToDataModel(entity);
    }

    public async Task<PasajeroDataModel> CreateAsync(PasajeroDataModel model)
    {
        var entity = PasajeroDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Estado = "ACTIVO";
        entity.FechaRegistroUtc = DateTime.UtcNow;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return PasajeroDataMapper.ToDataModel(entity);
    }

    public async Task<PasajeroDataModel?> UpdateAsync(PasajeroDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdPasajero); // ✅ con tracking

        if (entity is null || entity.EsEliminado)
            return null;

        PasajeroDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return PasajeroDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking

        if (entity is null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Estado = "INACTIVO";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}