using System;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaDataService _reservaDataService;
    private readonly IClienteDataService _clienteDataService;
    private readonly IPasajeroDataService _pasajeroDataService;
    private readonly IVueloDataService _vueloDataService;
    private readonly IAsientoDataService _asientoDataService;
    private readonly ReservaValidator _validator;

    public ReservaService(
        IReservaDataService reservaDataService,
        IClienteDataService clienteDataService,
        IPasajeroDataService pasajeroDataService,
        IVueloDataService vueloDataService,
        IAsientoDataService asientoDataService)
    {
        _reservaDataService = reservaDataService;
        _clienteDataService = clienteDataService;
        _pasajeroDataService = pasajeroDataService;
        _vueloDataService = vueloDataService;
        _asientoDataService = asientoDataService;
        _validator = new ReservaValidator();
    }

    public async Task<DataPagedResult<ReservaResponseDto>> GetPagedAsync(ReservaFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = ReservaBusinessMapper.ToFiltroDataModel(filter);
        var result = await _reservaDataService.GetPagedAsync(filtro);

        return new DataPagedResult<ReservaResponseDto>
        {
            Items = ReservaBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<ReservaResponseDto?> GetByIdAsync(int idReserva, int? idClienteDelToken, string rolDelToken)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        var data = await _reservaDataService.GetByIdAsync(idReserva);

        if (data == null) return null;

        // ✅ CLIENTE solo puede ver sus propias reservas
        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || data.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para ver esta reserva.");
        }

        return ReservaBusinessMapper.ToResponseDto(data);
    }

    public async Task<ReservaResponseDto> CreateAsync(ReservaRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var cliente = await _clienteDataService.GetByIdAsync(request.IdCliente);
        if (cliente == null)
            throw new NotFoundException("El cliente indicado no existe.");
        if (cliente.EsEliminado || cliente.Estado != "ACT")
            throw new BusinessException("El cliente indicado está inactivo o eliminado.");

        var pasajero = await _pasajeroDataService.GetByIdAsync(request.IdPasajero);
        if (pasajero == null)
            throw new NotFoundException("El pasajero indicado no existe.");
        if (pasajero.EsEliminado || pasajero.Estado != "ACTIVO")
            throw new BusinessException("El pasajero indicado está inactivo o eliminado.");
        if (pasajero.IdCliente.HasValue && pasajero.IdCliente.Value != request.IdCliente)
            throw new BusinessException("El pasajero indicado no pertenece al cliente indicado.");

        var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo indicado no existe.");
        if (vuelo.Estado != "ACTIVO" || vuelo.EstadoVuelo is "CANCELADO" or "ATERRIZADO")
            throw new BusinessException("El vuelo no está disponible para nuevas reservas.");

        var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);
        if (asiento == null)
            throw new NotFoundException("El asiento indicado no existe.");
        if (asiento.Eliminado || asiento.Estado != "ACTIVO")
            throw new BusinessException("El asiento indicado está inactivo o eliminado.");

        if (asiento.IdVuelo != request.IdVuelo)
            throw new BusinessException("El asiento no pertenece al vuelo indicado.");

        if (!asiento.Disponible)
            throw new BusinessException("El asiento seleccionado no está disponible.");

        var existentes = await _reservaDataService.GetPagedAsync(new ReservaFiltroDataModel
        {
            IdVuelo = request.IdVuelo,
            PageNumber = 1,
            PageSize = 10000
        });

        if (existentes.Items.Any(x => x.IdVuelo == request.IdVuelo && x.IdAsiento == request.IdAsiento))
            throw new BusinessException("Ya existe una reserva para ese asiento en el vuelo indicado.");

        if (existentes.Items.Any(x => x.IdVuelo == request.IdVuelo && x.IdPasajero == request.IdPasajero))
            throw new BusinessException("Ya existe una reserva para ese pasajero en el vuelo indicado.");

        var dataModel = ReservaBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creada = await _reservaDataService.CreateAsync(dataModel);

        return ReservaBusinessMapper.ToResponseDto(creada);
    }

    public async Task<ReservaResponseDto?> UpdateEstadoAsync(
        int idReserva,
        ReservaUpdateRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _reservaDataService.GetByIdAsync(idReserva);
        if (actual == null)
            throw new NotFoundException("Reserva no encontrada.");

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || actual.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para modificar el estado de esta reserva.");

            var estadoNuevoCliente = request.EstadoReserva.Trim().ToUpperInvariant();
            if (estadoNuevoCliente is not ("CON" or "CAN"))
                throw new BusinessException("Como cliente solo puedes confirmar (CON) o cancelar (CAN) tu reserva.");
        }

        var estadoActual = actual.EstadoReserva.Trim().ToUpperInvariant();
        var estadoNuevo = request.EstadoReserva.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "PEN", new[] { "CON", "CAN", "EXP" } },
            { "CON", new[] { "EMI", "CAN", "FIN" } },
            { "EMI", new[] { "FIN", "CAN" } },
            { "CAN", Array.Empty<string>() },
            { "EXP", Array.Empty<string>() },
            { "FIN", Array.Empty<string>() }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de la reserva desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        var vuelo = await _vueloDataService.GetByIdAsync(actual.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo asociado a la reserva no existe.");

        if (estadoNuevo == "CON" && vuelo.EstadoVuelo is not ("PROGRAMADO" or "DEMORADO"))
            throw new BusinessException("Solo se puede confirmar una reserva si el vuelo está PROGRAMADO o DEMORADO.");

        actual.EstadoReserva = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;
        actual.MotivoCancelacion = estadoNuevo == "CAN" ? request.MotivoCancelacion?.Trim() : null;
        actual.FechaCancelacionUtc = estadoNuevo == "CAN" ? DateTime.UtcNow : null;
        actual.FechaConfirmacionUtc = estadoNuevo == "CON" ? DateTime.UtcNow : actual.FechaConfirmacionUtc;

        var actualizado = await _reservaDataService.UpdateAsync(actual);

        return actualizado == null ? null : ReservaBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idReserva, string modificadoPorUsuario)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _reservaDataService.GetByIdAsync(idReserva);
        if (actual == null)
            throw new NotFoundException("Reserva no encontrada.");

        return await _reservaDataService.DeleteAsync(idReserva, modificadoPorUsuario);
    }
}