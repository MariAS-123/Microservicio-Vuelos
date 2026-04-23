using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class VueloService : IVueloService
{
    private readonly IVueloDataService _vueloDataService;
    private readonly IAeropuertoDataService _aeropuertoDataService;
    private readonly VueloValidator _validator;
    private readonly bool _validarHoraSalidaAntesDeEnVuelo;

    public VueloService(
        IVueloDataService vueloDataService,
        IAeropuertoDataService aeropuertoDataService,
        IConfiguration configuration)
    {
        _vueloDataService = vueloDataService;
        _aeropuertoDataService = aeropuertoDataService;
        _validator = new VueloValidator();
        _validarHoraSalidaAntesDeEnVuelo = configuration.GetValue(
            "BusinessRules:Vuelo:ValidarHoraSalidaAntesDeEnVuelo",
            true);
    }

    public async Task<DataPagedResult<VueloResponseDto>> GetPagedAsync(VueloFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = VueloBusinessMapper.ToFiltroDataModel(filter);
        var result = await _vueloDataService.GetPagedAsync(filtro);

        return new DataPagedResult<VueloResponseDto>
        {
            Items = VueloBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<VueloResponseDto?> GetByIdAsync(int idVuelo)
    {
        if (idVuelo <= 0)
            throw new ValidationException("El id del vuelo debe ser mayor que 0.");

        var data = await _vueloDataService.GetByIdAsync(idVuelo);

        return data == null ? null : VueloBusinessMapper.ToResponseDto(data);
    }

    public async Task<VueloResponseDto> CreateAsync(VueloRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var aeropuertoOrigen = await _aeropuertoDataService.GetByIdAsync(request.IdAeropuertoOrigen);
        if (aeropuertoOrigen == null)
            throw new NotFoundException("El aeropuerto de origen no existe.");
        if (aeropuertoOrigen.Eliminado || aeropuertoOrigen.Estado != "ACTIVO")
            throw new BusinessException("El aeropuerto de origen está inactivo o eliminado.");

        var aeropuertoDestino = await _aeropuertoDataService.GetByIdAsync(request.IdAeropuertoDestino);
        if (aeropuertoDestino == null)
            throw new NotFoundException("El aeropuerto de destino no existe.");
        if (aeropuertoDestino.Eliminado || aeropuertoDestino.Estado != "ACTIVO")
            throw new BusinessException("El aeropuerto de destino está inactivo o eliminado.");

        var existentes = await _vueloDataService.GetPagedAsync(new VueloFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var numeroVuelo = request.NumeroVuelo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.NumeroVuelo.Trim().ToUpperInvariant() == numeroVuelo))
            throw new BusinessException("Ya existe un vuelo con el mismo número de vuelo.");

        var dataModel = VueloBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _vueloDataService.CreateAsync(dataModel);

        return VueloBusinessMapper.ToResponseDto(creado);
    }

    public async Task<VueloResponseDto?> UpdateAsync(int idVuelo, VueloUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idVuelo <= 0)
            throw new ValidationException("El id del vuelo debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _vueloDataService.GetByIdAsync(idVuelo);
        if (actual == null)
            throw new NotFoundException("Vuelo no encontrado.");

        var aeropuertoOrigen = await _aeropuertoDataService.GetByIdAsync(request.IdAeropuertoOrigen);
        if (aeropuertoOrigen == null)
            throw new NotFoundException("El aeropuerto de origen no existe.");
        if (aeropuertoOrigen.Eliminado || aeropuertoOrigen.Estado != "ACTIVO")
            throw new BusinessException("El aeropuerto de origen está inactivo o eliminado.");

        var aeropuertoDestino = await _aeropuertoDataService.GetByIdAsync(request.IdAeropuertoDestino);
        if (aeropuertoDestino == null)
            throw new NotFoundException("El aeropuerto de destino no existe.");
        if (aeropuertoDestino.Eliminado || aeropuertoDestino.Estado != "ACTIVO")
            throw new BusinessException("El aeropuerto de destino está inactivo o eliminado.");

        var existentes = await _vueloDataService.GetPagedAsync(new VueloFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var numeroVuelo = request.NumeroVuelo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdVuelo != idVuelo && x.NumeroVuelo.Trim().ToUpperInvariant() == numeroVuelo))
            throw new BusinessException("Ya existe otro vuelo con el mismo número de vuelo.");

        var estadoActual = actual.EstadoVuelo.Trim().ToUpperInvariant();
        var estadoNuevo = request.EstadoVuelo.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "PROGRAMADO", new[] { "EN_VUELO", "CANCELADO", "DEMORADO" } },
            { "DEMORADO",   new[] { "EN_VUELO", "CANCELADO", "PROGRAMADO" } },
            { "EN_VUELO",   new[] { "ATERRIZADO" } },
            { "ATERRIZADO", Array.Empty<string>() },
            { "CANCELADO",  Array.Empty<string>() }
        };

        if (estadoActual != estadoNuevo)
        {
            if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
                throw new ValidationException($"Estado actual del vuelo desconocido: {estadoActual}.");

            if (!permitidos.Contains(estadoNuevo))
                throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");
        }

        var dataModel = VueloBusinessMapper.ToDataModel(idVuelo, request);
        dataModel.ModificadoPorUsuario = modificadoPorUsuario;

        var actualizado = await _vueloDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : VueloBusinessMapper.ToResponseDto(actualizado);
    }

    // ✅ Nuevo método para cambiar estado operativo del vuelo
    public async Task<VueloResponseDto?> UpdateEstadoAsync(int idVuelo, VueloEstadoRequestDto request, string modificadoPorUsuario)
    {
        if (idVuelo <= 0)
            throw new ValidationException("El id del vuelo debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        if (string.IsNullOrWhiteSpace(request.EstadoVuelo))
            throw new ValidationException("El estado del vuelo es requerido.");

        var estadosValidos = new[] { "PROGRAMADO", "EN_VUELO", "ATERRIZADO", "CANCELADO", "DEMORADO" };
        var nuevoEstado = request.EstadoVuelo.Trim().ToUpperInvariant();

        if (!estadosValidos.Contains(nuevoEstado))
            throw new ValidationException($"Estado inválido. Los valores permitidos son: {string.Join(", ", estadosValidos)}.");

        var actual = await _vueloDataService.GetByIdAsync(idVuelo);
        if (actual == null)
            throw new NotFoundException("Vuelo no encontrado.");

        // ✅ Validación de transiciones de estado
        var transicionesPermitidas = new Dictionary<string, string[]>
    {
        { "PROGRAMADO", new[] { "EN_VUELO", "CANCELADO", "DEMORADO" } },
        { "DEMORADO",   new[] { "EN_VUELO", "CANCELADO", "PROGRAMADO" } },
        { "EN_VUELO",   new[] { "ATERRIZADO" } },
        { "ATERRIZADO", Array.Empty<string>() }, // estado final
        { "CANCELADO",  Array.Empty<string>() }  // estado final
    };

        var estadoActual = actual.EstadoVuelo.Trim().ToUpperInvariant();

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual del vuelo desconocido: {estadoActual}.");

        if (!permitidos.Contains(nuevoEstado))
        {
            if (permitidos.Length == 0)
                throw new BusinessException(
                    $"El vuelo en estado '{estadoActual}' es un estado final y no puede cambiar a ningún otro estado.");

            throw new BusinessException(
                $"No es posible cambiar el estado de '{estadoActual}' a '{nuevoEstado}'. " +
                $"Transiciones permitidas: {string.Join(", ", permitidos)}.");
        }

        if (_validarHoraSalidaAntesDeEnVuelo &&
            nuevoEstado == "EN_VUELO" &&
            DateTime.UtcNow < actual.FechaHoraSalida)
        {
            throw new BusinessException("No se puede marcar EN_VUELO antes de la fecha y hora de salida.");
        }

        actual.EstadoVuelo = nuevoEstado;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;

        var actualizado = await _vueloDataService.UpdateAsync(actual);

        return actualizado == null ? null : VueloBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idVuelo, string modificadoPorUsuario)
    {
        if (idVuelo <= 0)
            throw new ValidationException("El id del vuelo debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _vueloDataService.GetByIdAsync(idVuelo);
        if (actual == null)
            throw new NotFoundException("Vuelo no encontrado.");

        return await _vueloDataService.DeleteAsync(idVuelo, modificadoPorUsuario);
    }

    public async Task<DataPagedResult<VueloResponseDto>> GetPagedBookingAsync(VueloFilterDto filter)
    {
        _validator.ValidateFilterBooking(filter); // ✅ validación estricta

        var filtro = VueloBusinessMapper.ToFiltroDataModel(filter);
        var result = await _vueloDataService.GetPagedAsync(filtro);

        return new DataPagedResult<VueloResponseDto>
        {
            Items = VueloBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }
}