using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Api.Model.Common;
using Microservicio.Vuelos.Business.DTOs.Asiento;
using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.DTOs.Equipaje;
using Microservicio.Vuelos.Business.DTOs.Factura;
using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cliente")]
[Produces("application/json")]
[Authorize(Roles = "CLIENTE")]
public class ClientePortalController : ControllerBase
{
    private readonly IFacturaService _facturaService;
    private readonly IReservaService _reservaService;
    private readonly IBoletoService _boletoService;
    private readonly IPasajeroService _pasajeroService;
    private readonly IVueloService _vueloService;
    private readonly IAsientoService _asientoService;
    private readonly IEquipajeService _equipajeService;

    public ClientePortalController(
        IFacturaService facturaService,
        IReservaService reservaService,
        IBoletoService boletoService,
        IPasajeroService pasajeroService,
        IVueloService vueloService,
        IAsientoService asientoService,
        IEquipajeService equipajeService)
    {
        _facturaService = facturaService;
        _reservaService = reservaService;
        _boletoService = boletoService;
        _pasajeroService = pasajeroService;
        _vueloService = vueloService;
        _asientoService = asientoService;
        _equipajeService = equipajeService;
    }

    [HttpGet("facturas")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> GetMisFacturas(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? estado = null)
    {
        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<object>.Fail("No se pudo identificar el cliente de la sesión."));

        var result = await _facturaService.GetPagedAsync(new FacturaFilterDto
        {
            IdCliente = idCliente.Value,
            Estado = estado,
            Page = page,
            PageSize = pageSize
        });

        return Ok(ApiResponse<object>.Ok(result, "Consulta de facturas del cliente realizada correctamente."));
    }

    [HttpGet("reservas/{id_reserva:int}/factura")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> GetFacturaPorReserva(int id_reserva)
    {
        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<FacturaResponseDto>.Fail("No se pudo identificar el cliente de la sesión."));

        var reserva = await _reservaService.GetByIdAsync(id_reserva, idCliente, "CLIENTE");
        if (reserva is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Reserva no encontrada."));

        var result = await _facturaService.GetPagedAsync(new FacturaFilterDto
        {
            IdReserva = id_reserva,
            IdCliente = idCliente.Value,
            Page = 1,
            PageSize = 50
        });

        var factura = result.Items.FirstOrDefault(x => !string.Equals(x.Estado, "INA", StringComparison.OrdinalIgnoreCase))
                      ?? result.Items.FirstOrDefault();

        if (factura is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("No existe factura asociada a la reserva indicada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(factura));
    }

    [HttpGet("reservas/by-codigo/{codigo_reserva}")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> GetReservaByCodigo(string codigo_reserva)
    {
        if (string.IsNullOrWhiteSpace(codigo_reserva))
            return BadRequest(ApiResponse<ReservaResponseDto>.Fail("El código de reserva es obligatorio."));

        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<ReservaResponseDto>.Fail("No se pudo identificar el cliente de la sesión."));

        var result = await _reservaService.GetPagedAsync(new ReservaFilterDto
        {
            IdCliente = idCliente.Value,
            CodigoReserva = codigo_reserva.Trim(),
            Page = 1,
            PageSize = 200
        });

        var reserva = result.Items.FirstOrDefault(r =>
            string.Equals(
                (r.CodigoReserva ?? string.Empty).Trim(),
                codigo_reserva.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (reserva is null)
            return NotFound(ApiResponse<ReservaResponseDto>.Fail("No existe una reserva con ese código para el cliente autenticado."));

        return Ok(ApiResponse<ReservaResponseDto>.Ok(reserva, "Reserva encontrada correctamente."));
    }

    [HttpGet("reservas/{id_reserva:int}/detalle")]
    [ProducesResponseType(typeof(ApiResponse<ClienteReservaDetalleResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ClienteReservaDetalleResponseDto>>> GetDetalleReserva(int id_reserva)
    {
        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<ClienteReservaDetalleResponseDto>.Fail("No se pudo identificar el cliente de la sesión."));

        var reserva = await _reservaService.GetByIdAsync(id_reserva, idCliente, "CLIENTE");
        if (reserva is null)
            return NotFound(ApiResponse<ClienteReservaDetalleResponseDto>.Fail("Reserva no encontrada."));

        var pasajero = await _pasajeroService.GetByIdAsync(reserva.IdPasajero, idCliente, "CLIENTE");
        var vuelo = await _vueloService.GetByIdAsync(reserva.IdVuelo);
        var asiento = await _asientoService.GetByIdAsync(reserva.IdAsiento);

        var facturasResult = await _facturaService.GetPagedAsync(new FacturaFilterDto
        {
            IdReserva = reserva.IdReserva,
            IdCliente = idCliente.Value,
            Page = 1,
            PageSize = 50
        });
        var factura = facturasResult.Items.FirstOrDefault(x => !string.Equals(x.Estado, "INA", StringComparison.OrdinalIgnoreCase))
                   ?? facturasResult.Items.FirstOrDefault();

        var boletosResult = await _boletoService.GetPagedAsync(new BoletoFilterDto
        {
            IdReserva = reserva.IdReserva,
            Page = 1,
            PageSize = 50
        });
        var boleto = boletosResult.Items.FirstOrDefault();

        var equipajes = new List<EquipajeResponseDto>();
        if (boleto is not null)
        {
            var equipajesResult = await _equipajeService.GetPagedAsync(
                new EquipajeFilterDto
                {
                    IdBoleto = boleto.IdBoleto,
                    Page = 1,
                    PageSize = 200
                },
                idCliente,
                "CLIENTE");
            equipajes = equipajesResult.Items.ToList();
        }

        var detalle = new ClienteReservaDetalleResponseDto
        {
            IdReserva = reserva.IdReserva,
            CodigoReserva = reserva.CodigoReserva,
            EstadoReserva = reserva.EstadoReserva,
            FechaReservaUtc = reserva.FechaReservaUtc,
            Cliente = new ClienteResumenDto { IdCliente = reserva.IdCliente },
            Pasajero = pasajero is null
                ? null
                : new PasajeroResumenDto
                {
                    IdPasajero = pasajero.IdPasajero,
                    NombreCompleto = $"{pasajero.NombrePasajero} {pasajero.ApellidoPasajero}".Trim(),
                    TipoDocumento = pasajero.TipoDocumentoPasajero,
                    NumeroDocumento = pasajero.NumeroDocumentoPasajero
                },
            Vuelo = vuelo is null
                ? null
                : new VueloResumenDto
                {
                    IdVuelo = vuelo.IdVuelo,
                    NumeroVuelo = vuelo.NumeroVuelo,
                    FechaHoraSalida = vuelo.FechaHoraSalida,
                    FechaHoraLlegada = vuelo.FechaHoraLlegada,
                    EstadoVuelo = vuelo.EstadoVuelo
                },
            Asiento = asiento is null
                ? null
                : new AsientoResumenDto
                {
                    IdAsiento = asiento.IdAsiento,
                    NumeroAsiento = asiento.NumeroAsiento,
                    Clase = asiento.Clase,
                    Posicion = asiento.Posicion
                },
            Factura = factura,
            Boleto = boleto,
            Equipajes = equipajes
        };

        return Ok(ApiResponse<ClienteReservaDetalleResponseDto>.Ok(detalle, "Detalle de reserva obtenido correctamente."));
    }

    [HttpGet("reservas/{id_reserva:int}/boleto")]
    [ProducesResponseType(typeof(ApiResponse<BoletoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BoletoResponseDto>>> GetBoletoPorReserva(int id_reserva)
    {
        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<BoletoResponseDto>.Fail("No se pudo identificar el cliente de la sesión."));

        var reserva = await _reservaService.GetByIdAsync(id_reserva, idCliente, "CLIENTE");
        if (reserva is null)
            return NotFound(ApiResponse<BoletoResponseDto>.Fail("Reserva no encontrada."));

        var result = await _boletoService.GetPagedAsync(new BoletoFilterDto
        {
            IdReserva = id_reserva,
            Page = 1,
            PageSize = 50
        });

        var boleto = result.Items.FirstOrDefault();

        if (boleto is null)
            return NotFound(ApiResponse<BoletoResponseDto>.Fail("No existe boleto asociado a la reserva indicada."));

        return Ok(ApiResponse<BoletoResponseDto>.Ok(boleto));
    }

    [HttpGet("boletos")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetMisBoletos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? estado = null,
        [FromQuery] int? id_reserva = null)
    {
        var idCliente = GetIdCliente();
        if (idCliente is null)
            return Unauthorized(ApiResponse<object>.Fail("No se pudo identificar el cliente de la sesión."));

        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var reservasCliente = await _reservaService.GetPagedAsync(new ReservaFilterDto
        {
            IdCliente = idCliente.Value,
            Page = 1,
            PageSize = 200
        });

        var reservasFiltradas = reservasCliente.Items;
        if (id_reserva.HasValue)
        {
            reservasFiltradas = reservasFiltradas
                .Where(r => r.IdReserva == id_reserva.Value)
                .ToList();

            if (!reservasFiltradas.Any())
                return NotFound(ApiResponse<object>.Fail("La reserva indicada no pertenece al cliente o no existe."));
        }

        var boletos = new List<BoletoResponseDto>();
        foreach (var reserva in reservasFiltradas)
        {
            var result = await _boletoService.GetPagedAsync(new BoletoFilterDto
            {
                IdReserva = reserva.IdReserva,
                EstadoBoleto = estado,
                Page = 1,
                PageSize = 50
            });

            if (result.Items.Any())
                boletos.AddRange(result.Items);
        }

        var total = boletos.Count;
        var pagedItems = boletos
            .OrderByDescending(b => b.IdBoleto)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = new
        {
            items = pagedItems,
            pageNumber = page,
            pageSize,
            totalRecords = total
        };

        return Ok(ApiResponse<object>.Ok(response, "Consulta de boletos del cliente realizada correctamente."));
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    public class ClienteReservaDetalleResponseDto
    {
        public int IdReserva { get; set; }
        public string CodigoReserva { get; set; } = null!;
        public string EstadoReserva { get; set; } = null!;
        public DateTime FechaReservaUtc { get; set; }
        public ClienteResumenDto Cliente { get; set; } = null!;
        public PasajeroResumenDto? Pasajero { get; set; }
        public VueloResumenDto? Vuelo { get; set; }
        public AsientoResumenDto? Asiento { get; set; }
        public FacturaResponseDto? Factura { get; set; }
        public BoletoResponseDto? Boleto { get; set; }
        public List<EquipajeResponseDto> Equipajes { get; set; } = new();
    }

    public class ClienteResumenDto
    {
        public int IdCliente { get; set; }
    }

    public class PasajeroResumenDto
    {
        public int IdPasajero { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string TipoDocumento { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
    }

    public class VueloResumenDto
    {
        public int IdVuelo { get; set; }
        public string NumeroVuelo { get; set; } = null!;
        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraLlegada { get; set; }
        public string EstadoVuelo { get; set; } = null!;
    }

    public class AsientoResumenDto
    {
        public int IdAsiento { get; set; }
        public string NumeroAsiento { get; set; } = null!;
        public string Clase { get; set; } = null!;
        public string? Posicion { get; set; }
    }
}
