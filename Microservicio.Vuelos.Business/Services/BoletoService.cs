using System;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class BoletoService : IBoletoService
{
    private readonly IBoletoDataService _boletoDataService;
    private readonly IReservaDataService _reservaDataService;
    private readonly IVueloDataService _vueloDataService;
    private readonly IAsientoDataService _asientoDataService;
    private readonly IFacturaDataService _facturaDataService;
    private readonly BoletoValidator _validator;

    public BoletoService(
        IBoletoDataService boletoDataService,
        IReservaDataService reservaDataService,
        IVueloDataService vueloDataService,
        IAsientoDataService asientoDataService,
        IFacturaDataService facturaDataService)
    {
        _boletoDataService = boletoDataService;
        _reservaDataService = reservaDataService;
        _vueloDataService = vueloDataService;
        _asientoDataService = asientoDataService;
        _facturaDataService = facturaDataService;
        _validator = new BoletoValidator();
    }

    public async Task<DataPagedResult<BoletoResponseDto>> GetPagedAsync(BoletoFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = BoletoBusinessMapper.ToFiltroDataModel(filter);
        var result = await _boletoDataService.GetPagedAsync(filtro);

        return new DataPagedResult<BoletoResponseDto>
        {
            Items = BoletoBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<BoletoResponseDto?> GetByIdAsync(int idBoleto, int? idClienteDelToken, string rolDelToken)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        var data = await _boletoDataService.GetByIdAsync(idBoleto);

        if (data == null) return null;

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken is null)
                throw new UnauthorizedBusinessException("No se pudo identificar el cliente del token.");

            var reserva = await _reservaDataService.GetByIdAsync(data.IdReserva);
            if (reserva is null || reserva.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para ver este boleto.");
        }

        return BoletoBusinessMapper.ToResponseDto(data);
    }

    public async Task<BoletoResponseDto> CreateAsync(BoletoRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva);
        if (reserva == null)
            throw new NotFoundException("La reserva indicada no existe.");

        var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo indicado no existe.");
        if (vuelo.Estado != "ACTIVO" || vuelo.EstadoVuelo is "CANCELADO")
            throw new BusinessException("No se puede emitir boleto para un vuelo cancelado o inactivo.");

        var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);
        if (asiento == null)
            throw new NotFoundException("El asiento indicado no existe.");
        if (asiento.Estado != "ACTIVO" || asiento.Eliminado)
            throw new BusinessException("El asiento indicado está inactivo o eliminado.");

        var factura = await _facturaDataService.GetByIdAsync(request.IdFactura);
        if (factura == null)
            throw new NotFoundException("La factura indicada no existe.");

        if (reserva.IdVuelo != request.IdVuelo)
            throw new BusinessException("La reserva no pertenece al vuelo indicado.");

        if (reserva.IdAsiento != request.IdAsiento)
            throw new BusinessException("La reserva no pertenece al asiento indicado.");

        if (factura.IdReserva != request.IdReserva)
            throw new BusinessException("La factura no pertenece a la reserva indicada.");

        if (reserva.EstadoReserva is not ("CON" or "EMI"))
            throw new BusinessException("Solo se puede emitir boleto para reservas en estado CON o EMI.");

        if (factura.Estado != "APR")
            throw new BusinessException("Solo se puede emitir boleto con factura aprobada (APR).");

        var existentes = await _boletoDataService.GetPagedAsync(new BoletoFiltroDataModel
        {
            IdReserva = request.IdReserva,
            PageNumber = 1,
            PageSize = 10000
        });

        if (existentes.Items.Any(x => x.IdReserva == request.IdReserva))
            throw new BusinessException("Ya existe un boleto para la reserva indicada.");

        var dataModel = BoletoBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _boletoDataService.CreateAsync(dataModel);

        return BoletoBusinessMapper.ToResponseDto(creado);
    }

    public async Task<BoletoResponseDto?> UpdateEstadoAsync(int idBoleto, BoletoUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _boletoDataService.GetByIdAsync(idBoleto);
        if (actual == null)
            throw new NotFoundException("Boleto no encontrado.");

        var estadoActual = actual.EstadoBoleto.Trim().ToUpperInvariant();
        var estadoNuevo = request.EstadoBoleto.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "ACTIVO", new[] { "USADO", "CANCELADO" } },
            { "USADO", Array.Empty<string>() },
            { "CANCELADO", Array.Empty<string>() }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de boleto desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        actual.EstadoBoleto = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;

        var actualizado = await _boletoDataService.UpdateAsync(actual);

        return actualizado == null ? null : BoletoBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idBoleto, string modificadoPorUsuario)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _boletoDataService.GetByIdAsync(idBoleto);
        if (actual == null)
            throw new NotFoundException("Boleto no encontrado.");

        return await _boletoDataService.DeleteAsync(idBoleto, modificadoPorUsuario);
    }
}