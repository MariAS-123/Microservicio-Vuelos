using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Services.Policies;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class EquipajeService : IEquipajeService
{
    private readonly IEquipajeDataService _equipajeDataService;
    private readonly IBoletoDataService _boletoDataService;
    private readonly IReservaDataService _reservaDataService;
    private readonly IFacturaDataService _facturaDataService;
    private readonly EquipajeValidator _validator;

    public EquipajeService(
        IEquipajeDataService equipajeDataService,
        IBoletoDataService boletoDataService,
        IReservaDataService reservaDataService,
        IFacturaDataService facturaDataService)
    {
        _equipajeDataService = equipajeDataService;
        _boletoDataService = boletoDataService;
        _reservaDataService = reservaDataService;
        _facturaDataService = facturaDataService;
        _validator = new EquipajeValidator();
    }

    public async Task<DataPagedResult<EquipajeResponseDto>> GetPagedAsync(
        EquipajeFilterDto filter,
        int? idClienteDelToken,
        string rolDelToken)
    {
        _validator.ValidateFilter(filter);

        var filtro = EquipajeBusinessMapper.ToFiltroDataModel(filter);

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken is null)
                throw new UnauthorizedBusinessException("No se pudo identificar el cliente del token.");

            var reservasCliente = (await _reservaDataService.GetPagedAsync(
                new ReservaFiltroDataModel
                {
                    IdCliente = idClienteDelToken,
                    PageNumber = 1,
                    PageSize = 100_000,
                    IncluirEliminados = false
                })).Items;

            var reservaIds = new HashSet<int>(reservasCliente.Select(x => x.IdReserva));
            if (reservaIds.Count == 0)
                return EmptyEquipajePage(filter);

            var boletos = (await _boletoDataService.GetPagedAsync(
                new BoletoFiltroDataModel
                {
                    PageNumber = 1,
                    PageSize = 200_000,
                    IncluirEliminados = false
                })).Items;

            var permitidos = boletos
                .Where(b => reservaIds.Contains(b.IdReserva))
                .Select(b => b.IdBoleto)
                .Distinct()
                .ToList();

            if (permitidos.Count == 0)
                return EmptyEquipajePage(filter);

            if (filter.IdBoleto.HasValue && !permitidos.Contains(filter.IdBoleto.Value))
                return EmptyEquipajePage(filter);

            filtro.IdsBoletoPermitidos = permitidos;
        }

        var result = await _equipajeDataService.GetPagedAsync(filtro);

        return new DataPagedResult<EquipajeResponseDto>
        {
            Items = EquipajeBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<EquipajeResponseDto?> GetByIdAsync(
        int idEquipaje,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idEquipaje <= 0)
            throw new ValidationException("El id del equipaje debe ser mayor que 0.");

        var data = await _equipajeDataService.GetByIdAsync(idEquipaje);

        if (data == null) return null;

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken is null)
                throw new UnauthorizedBusinessException("No se pudo identificar el cliente del token.");

            var boleto = await _boletoDataService.GetByIdAsync(data.IdBoleto);
            if (boleto == null)
                return null;

            var reserva = await _reservaDataService.GetByIdAsync(boleto.IdReserva);
            if (reserva is null || reserva.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para ver este equipaje.");
        }

        return EquipajeBusinessMapper.ToResponseDto(data);
    }

    public async Task<EquipajeResponseDto> CreateAsync(
        EquipajeRequestDto request,
        string creadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var boleto = await _boletoDataService.GetByIdAsync(request.IdBoleto);
        if (boleto == null)
            throw new NotFoundException("El boleto indicado no existe.");

        if (boleto.EstadoBoleto != "ACTIVO")
            throw new BusinessException("Solo se puede registrar equipaje para boletos en estado ACTIVO.");

        var factura = await _facturaDataService.GetByIdAsync(boleto.IdFactura);
        if (factura == null)
            throw new NotFoundException("La factura asociada al boleto no existe.");

        var estadoFactura = factura.Estado.Trim().ToUpperInvariant();
        if (estadoFactura != "ABI")
            throw new BusinessException("Solo se puede registrar equipaje cuando la factura está ABI.");

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken is null)
                throw new UnauthorizedBusinessException("No se pudo identificar el cliente del token.");

            var reserva = await _reservaDataService.GetByIdAsync(boleto.IdReserva);
            if (reserva is null || reserva.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No puedes registrar equipaje para un boleto que no es tuyo.");
        }

        var dataModel = EquipajeBusinessMapper.ToDataModel(request, creadoPorUsuario);
        dataModel.PrecioExtra = EquipajePricingPolicy.CalcularPrecioExtra(request.Tipo, request.PesoKg);
        dataModel.DimensionesCm = EquipajePricingPolicy.ObtenerDimensionesEstandar(request.Tipo);
        var creado = await _equipajeDataService.CreateAsync(dataModel);

        // Sincroniza el acumulado de equipaje en boleto para mantener consistencia visual/operativa.
        boleto.CargoEquipaje = Math.Round(boleto.CargoEquipaje + creado.PrecioExtra, 2, MidpointRounding.AwayFromZero);
        boleto.PrecioFinal = Math.Round(
            boleto.PrecioVueloBase + boleto.PrecioAsientoExtra + boleto.ImpuestosBoleto + boleto.CargoEquipaje,
            2,
            MidpointRounding.AwayFromZero);
        boleto.ModificadoPorUsuario = creadoPorUsuario;
        boleto.FechaModificacionUtc = DateTime.UtcNow;
        await _boletoDataService.UpdateAsync(boleto);

        // Recalcula la factura abierta incorporando el cargo por equipaje.
        factura.CargoServicio = Math.Round(factura.CargoServicio + creado.PrecioExtra, 2, MidpointRounding.AwayFromZero);
        factura.Total = Math.Round(factura.Subtotal + factura.ValorIva + factura.CargoServicio, 2, MidpointRounding.AwayFromZero);
        factura.ModificadoPorUsuario = creadoPorUsuario;
        factura.FechaModificacionUtc = DateTime.UtcNow;
        await _facturaDataService.UpdateAsync(factura);

        return EquipajeBusinessMapper.ToResponseDto(creado);
    }

    public async Task<EquipajeResponseDto?> UpdateEstadoAsync(
        int idEquipaje,
        EquipajeUpdateRequestDto request,
        string modificadoPorUsuario)
    {
        if (idEquipaje <= 0)
            throw new ValidationException("El id del equipaje debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _equipajeDataService.GetByIdAsync(idEquipaje);
        if (actual == null)
            throw new NotFoundException("Equipaje no encontrado.");

        var boleto = await _boletoDataService.GetByIdAsync(actual.IdBoleto);
        if (boleto == null)
            throw new NotFoundException("El boleto asociado al equipaje no existe.");

        var factura = await _facturaDataService.GetByIdAsync(boleto.IdFactura);
        if (factura == null)
            throw new NotFoundException("La factura asociada al boleto no existe.");

        var estadoFactura = factura.Estado.Trim().ToUpperInvariant();
        if (estadoFactura is "APR" or "INA")
            throw new BusinessException("No se puede modificar equipaje cuando la factura está APR o INA.");

        var estadoActual = actual.EstadoEquipaje.Trim().ToUpperInvariant();
        var estadoNuevo = request.EstadoEquipaje.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "REGISTRADO", new[] { "EMBARCADO", "CANCELADO", "PERDIDO", "DAÑADO" } },
            { "EMBARCADO", new[] { "EN_TRANSITO", "CANCELADO", "PERDIDO", "DAÑADO" } },
            { "EN_TRANSITO", new[] { "ENTREGADO", "PERDIDO", "DAÑADO" } },
            { "ENTREGADO", Array.Empty<string>() },
            { "CANCELADO", Array.Empty<string>() },
            { "PERDIDO", Array.Empty<string>() },
            { "DAÑADO", Array.Empty<string>() }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de equipaje desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        actual.EstadoEquipaje = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;

        var actualizado = await _equipajeDataService.UpdateAsync(actual);

        return actualizado == null ? null : EquipajeBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idEquipaje, string modificadoPorUsuario)
    {
        if (idEquipaje <= 0)
            throw new ValidationException("El id del equipaje debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _equipajeDataService.GetByIdAsync(idEquipaje);
        if (actual == null)
            throw new NotFoundException("Equipaje no encontrado.");

        return await _equipajeDataService.DeleteAsync(idEquipaje, modificadoPorUsuario);
    }

    private static DataPagedResult<EquipajeResponseDto> EmptyEquipajePage(EquipajeFilterDto filter)
    {
        var page = filter.Page <= 0 ? 1 : filter.Page;
        var size = filter.PageSize <= 0 ? 20 : filter.PageSize;

        return new DataPagedResult<EquipajeResponseDto>
        {
            Items = new List<EquipajeResponseDto>(),
            PageNumber = page,
            PageSize = size,
            TotalRecords = 0
        };
    }
}
