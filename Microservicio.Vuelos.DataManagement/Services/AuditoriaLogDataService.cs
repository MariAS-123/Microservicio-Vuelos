using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class AuditoriaLogDataService : IAuditoriaLogDataService
{
    private readonly IAuditoriaLogRepository _repo;
    private readonly IUnitOfWork _uow;

    public AuditoriaLogDataService(IAuditoriaLogRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<AuditoriaLogDataModel>> GetPagedAsync(AuditoriaLogFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.TablaAfectada))
        {
            var tabla = filtro.TablaAfectada.Trim().ToUpperInvariant();
            query = query.Where(x => x.TablaAfectada.Contains(tabla));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Operacion))
        {
            var operacion = filtro.Operacion.Trim().ToUpperInvariant();
            query = query.Where(x => x.Operacion == operacion);
        }

        if (!string.IsNullOrWhiteSpace(filtro.UsuarioEjecutor))
        {
            var usuario = filtro.UsuarioEjecutor.Trim().ToUpperInvariant();
            query = query.Where(x => x.UsuarioEjecutor.Contains(usuario));
        }

        if (filtro.FechaDesde.HasValue)
            query = query.Where(x => x.FechaEventoUtc >= filtro.FechaDesde.Value);

        if (filtro.FechaHasta.HasValue)
            query = query.Where(x => x.FechaEventoUtc <= filtro.FechaHasta.Value);

        query = query.OrderByDescending(x => x.FechaEventoUtc);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(AuditoriaLogDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<AuditoriaLogDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<AuditoriaLogDataModel?> GetByIdAsync(long id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        return entity == null
            ? null
            : AuditoriaLogDataMapper.ToDataModel(entity);
    }

    public async Task<AuditoriaLogDataModel> CreateAsync(AuditoriaLogDataModel model)
    {
        var entity = AuditoriaLogDataMapper.ToEntity(model);

        entity.AuditoriaGuid = Guid.NewGuid();
        entity.FechaEventoUtc = DateTime.UtcNow;
        entity.Activo = true;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return AuditoriaLogDataMapper.ToDataModel(entity);
    }

    public async Task<AuditoriaLogDataModel?> UpdateAsync(AuditoriaLogDataModel model)
    {
        var entity = await _repo.ObtenerPorIdAsync(model.IdAuditoria);

        if (entity == null)
            return null;

        AuditoriaLogDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return AuditoriaLogDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        // ❌ PROHIBIDO eliminar auditoría
        throw new InvalidOperationException("No se permite eliminar registros de auditoría.");
    }
}