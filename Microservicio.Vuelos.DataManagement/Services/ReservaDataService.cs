using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class ReservaDataService : IReservaDataService
{
    private readonly IReservaRepository _repo;
    private readonly IUnitOfWork _uow;

    public ReservaDataService(IReservaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(ReservaFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.CodigoReserva))
        {
            var codigo = filtro.CodigoReserva.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoReserva.Contains(codigo));
        }

        if (filtro.IdCliente.HasValue)
            query = query.Where(x => x.IdCliente == filtro.IdCliente.Value);

        if (filtro.IdPasajero.HasValue)
            query = query.Where(x => x.IdPasajero == filtro.IdPasajero.Value);

        if (filtro.IdVuelo.HasValue)
            query = query.Where(x => x.IdVuelo == filtro.IdVuelo.Value);

        if (!string.IsNullOrWhiteSpace(filtro.EstadoReserva))
        {
            var estado = filtro.EstadoReserva.Trim().ToUpperInvariant();
            query = query.Where(x => x.EstadoReserva == estado);
        }

        query = query.OrderByDescending(x => x.FechaReservaUtc);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(ReservaDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<ReservaDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<ReservaDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<ReservaDataModel> CreateAsync(ReservaDataModel model)
    {
        var entity = ReservaDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.FechaRegistroUtc = DateTime.UtcNow;
        entity.FechaReservaUtc = DateTime.UtcNow;
        entity.EstadoReserva = "PEN";

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<ReservaDataModel?> UpdateAsync(ReservaDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdReserva); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return null;

        ReservaDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.FechaInhabilitacionUtc = DateTime.UtcNow;
        entity.MotivoInhabilitacion = "Eliminación lógica";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}