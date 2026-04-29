using System;
using System.Linq;
using System.Text.RegularExpressions;
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
    private const int FilasCabinaEstandar = 28;
    private static readonly char[] ColumnasCabinaEstandar = ['A', 'B', 'C', 'D', 'E', 'F'];
    private const int CapacidadCabinaEstandar = FilasCabinaEstandar * 6;
    private const int UltimaFilaPrimeraClase = 4;
    private const int UltimaFilaEjecutiva = 10;
    private const decimal PrecioExtraPrimeraClase = 80m;
    private const decimal PrecioExtraEjecutiva = 40m;
    private const decimal PrecioExtraEconomica = 0m;

    private readonly IVueloDataService _vueloDataService;
    private readonly IReservaDataService _reservaDataService;
    private readonly IAsientoDataService _asientoDataService;
    private readonly IAeropuertoDataService _aeropuertoDataService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VueloValidator _validator;
    private readonly bool _validarHoraSalidaAntesDeEnVuelo;

    public VueloService(
        IVueloDataService vueloDataService,
        IReservaDataService reservaDataService,
        IAsientoDataService asientoDataService,
        IAeropuertoDataService aeropuertoDataService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _vueloDataService = vueloDataService;
        _reservaDataService = reservaDataService;
        _asientoDataService = asientoDataService;
        _aeropuertoDataService = aeropuertoDataService;
        _unitOfWork = unitOfWork;
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

        request.CapacidadTotal = CapacidadCabinaEstandar;
        request.FechaHoraLlegada = request.FechaHoraSalida.AddMinutes(request.DuracionMin);
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

        var numeroVueloGenerado = await GenerarNumeroVueloAsync();

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var dataModel = VueloBusinessMapper.ToDataModel(request, creadoPorUsuario);
            dataModel.NumeroVuelo = numeroVueloGenerado;
            dataModel.CapacidadTotal = CapacidadCabinaEstandar;

            var creado = await _vueloDataService.CreateAsync(dataModel);
            await EnsureSeatMapAsync(creado.IdVuelo, creadoPorUsuario);

            return VueloBusinessMapper.ToResponseDto(creado);
        });
    }

    public async Task<VueloResponseDto?> UpdateAsync(int idVuelo, VueloUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idVuelo <= 0)
            throw new ValidationException("El id del vuelo debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        request.CapacidadTotal = CapacidadCabinaEstandar;
        request.FechaHoraLlegada = request.FechaHoraSalida.AddMinutes(request.DuracionMin);
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
        dataModel.CapacidadTotal = CapacidadCabinaEstandar;

        var actualizado = await _vueloDataService.UpdateAsync(dataModel);
        if (actualizado != null)
            await EnsureSeatMapAsync(actualizado.IdVuelo, modificadoPorUsuario);

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

        // Para portal cliente, filtramos ANTES de paginar para evitar desalineación de totalRecords.
        var filtroCompleto = VueloBusinessMapper.ToFiltroDataModel(filter);
        filtroCompleto.PageNumber = 1;
        filtroCompleto.PageSize = 100000;
        var resultCompleto = await _vueloDataService.GetPagedAsync(filtroCompleto);
        var vuelosFiltrados = resultCompleto.Items
            .Where(v =>
                string.Equals(v.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) &&
                v.EstadoVuelo is "PROGRAMADO" or "DEMORADO" &&
                v.FechaHoraSalida > DateTime.UtcNow)
            .ToList();

        if (vuelosFiltrados.Count > 0)
        {
            var idsVuelos = vuelosFiltrados.Select(x => x.IdVuelo).ToHashSet();

            var reservas = await _reservaDataService.GetPagedAsync(new ReservaFiltroDataModel
            {
                PageNumber = 1,
                PageSize = 100000
            });

            var reservasActivas = reservas.Items
                .Where(r =>
                    idsVuelos.Contains(r.IdVuelo) &&
                    r.EstadoReserva is "PEN" or "CON" or "EMI")
                .ToList();

            var asientosDisponibles = await _asientoDataService.GetPagedAsync(new AsientoFiltroDataModel
            {
                Disponible = true,
                Estado = "ACTIVO",
                PageNumber = 1,
                PageSize = 100000
            });

            var asientosReservadosActivos = reservasActivas
                .SelectMany(r => r.Detalles.Where(d => !d.EsEliminado).Select(d => d.IdAsiento))
                .ToHashSet();

            var asientosRealmenteDisponiblesPorVuelo = asientosDisponibles.Items
                .Where(a =>
                    idsVuelos.Contains(a.IdVuelo) &&
                    !a.Eliminado &&
                    !asientosReservadosActivos.Contains(a.IdAsiento))
                .GroupBy(a => a.IdVuelo)
                .ToDictionary(g => g.Key, g => g.Count());

            vuelosFiltrados = vuelosFiltrados
                .Where(v =>
                {
                    var asientosDisponibles = asientosRealmenteDisponiblesPorVuelo.GetValueOrDefault(v.IdVuelo, 0);
                    return asientosDisponibles > 0;
                })
                .ToList();
        }

        var page = filter.Page <= 0 ? 1 : filter.Page;
        var pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;
        var totalRecords = vuelosFiltrados.Count;
        var vuelosPagina = vuelosFiltrados
            .OrderBy(v => v.FechaHoraSalida)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new DataPagedResult<VueloResponseDto>
        {
            Items = VueloBusinessMapper.ToResponseDtoList(vuelosPagina),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };
    }

    private async Task<string> GenerarNumeroVueloAsync()
    {
        var existentes = await _vueloDataService.GetPagedAsync(new VueloFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 100000
        });

        var correlativoMaximo = 0;
        var regex = new Regex("^AV(\\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        foreach (var vuelo in existentes.Items)
        {
            var numeroVuelo = (vuelo.NumeroVuelo ?? string.Empty).Trim().ToUpperInvariant();
            var match = regex.Match(numeroVuelo);
            if (!match.Success)
                continue;

            if (int.TryParse(match.Groups[1].Value, out var valor) && valor > correlativoMaximo)
                correlativoMaximo = valor;
        }

        return $"AV{(correlativoMaximo + 1):D4}";
    }

    private async Task EnsureSeatMapAsync(int idVuelo, string usuario)
    {
        var existentes = await _asientoDataService.GetByVueloAsync(idVuelo);
        var numerosExistentes = existentes
            .Where(x => !x.Eliminado)
            .Select(x => x.NumeroAsiento.Trim().ToUpperInvariant())
            .ToHashSet();

        foreach (var fila in Enumerable.Range(1, FilasCabinaEstandar))
        {
            foreach (var columna in ColumnasCabinaEstandar)
            {
                var numeroAsiento = $"{fila}{columna}";
                if (numerosExistentes.Contains(numeroAsiento))
                    continue;

                var posicion = columna switch
                {
                    'A' or 'F' => "VENTANA",
                    'B' or 'E' => "CENTRO",
                    _ => "PASILLO"
                };

                var (clase, precioExtra) = ObtenerConfiguracionAsiento(fila);

                await _asientoDataService.CreateAsync(new AsientoDataModel
                {
                    IdVuelo = idVuelo,
                    NumeroAsiento = numeroAsiento,
                    Clase = clase,
                    Disponible = true,
                    PrecioExtra = precioExtra,
                    Posicion = posicion,
                    Estado = "ACTIVO",
                    Eliminado = false,
                    CreadoPorUsuario = usuario
                });
            }
        }
    }

    private static (string Clase, decimal PrecioExtra) ObtenerConfiguracionAsiento(int fila)
    {
        if (fila <= UltimaFilaPrimeraClase)
            return ("PRIMERA", PrecioExtraPrimeraClase);

        if (fila <= UltimaFilaEjecutiva)
            return ("EJECUTIVA", PrecioExtraEjecutiva);

        return ("ECONOMICA", PrecioExtraEconomica);
    }
}
