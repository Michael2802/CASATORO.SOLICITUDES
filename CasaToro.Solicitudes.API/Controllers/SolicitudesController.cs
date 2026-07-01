using CasaToro.Solicitudes.Application.DTOs;
using CasaToro.Solicitudes.Application.Exceptions;
using CasaToro.Solicitudes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CasaToro.Solicitudes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudService _service;
        private readonly ILogger<SolicitudesController> _logger;

        // Recibe el servicio por Inyección de Dependencias
        public SolicitudesController(
            ISolicitudService service,
            ILogger<SolicitudesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva solicitud aplicando todas las reglas de negocio
        /// RN-01: No duplicados mismo tipo mismo día
        /// RN-02: Solo usuarios activos
        /// RN-03: Inicia en estado Pendiente
        /// RN-04: Genera seguimiento automáticamente
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearSolicitud([FromBody] CrearSolicitudDTO dto)
        {
            try
            {
                // El Controller solo delega al Service
                // No tiene lógica de negocio aquí
                var resultado = await _service.CrearSolicitudAsync(dto);

                // HTTP 201 = Created
                return CreatedAtAction(
                    nameof(ObtenerPorId),
                    new { id = resultado.Id },
                    resultado);
            }
            catch (UsuarioInactivoException ex)
            {
                // HTTP 403 = Forbidden
                _logger.LogWarning("Usuario inactivo intentó crear solicitud: {Mensaje}", ex.Message);
                return StatusCode(403, new { mensaje = ex.Message });
            }
            catch (SolicitudDuplicadaException ex)
            {
                // HTTP 409 = Conflict
                _logger.LogWarning("Solicitud duplicada: {Mensaje}", ex.Message);
                return StatusCode(409, new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                // HTTP 500 = Error interno
                // Nunca se expone el detalle técnico al cliente
                _logger.LogError(ex, "Error inesperado al crear solicitud");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el detalle de una solicitud por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var resultado = await _service.ObtenerPorIdAsync(id);
                return Ok(resultado); // HTTP 200
            }
            catch (SolicitudNoEncontradaException ex)
            {
                // HTTP 404 = Not Found
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitud {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Cambia el estado de una solicitud existente
        /// RN-04: Genera seguimiento automáticamente
        /// RN-05: No se puede cerrar sin observación
        /// </summary>
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDTO dto)
        {
            try
            {
                dto.SolicitudId = id;
                var resultado = await _service.CambiarEstadoAsync(dto);
                return Ok(resultado); // HTTP 200
            }
            catch (SolicitudNoEncontradaException ex)
            {
                return NotFound(new { mensaje = ex.Message }); // HTTP 404
            }
            catch (ObservacionRequeridaException ex)
            {
                return BadRequest(new { mensaje = ex.Message }); // HTTP 400
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado solicitud {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene todas las solicitudes de un usuario
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
        {
            try
            {
                var resultado = await _service.ObtenerPorUsuarioAsync(usuarioId);
                return Ok(resultado); // HTTP 200
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitudes del usuario {UsuarioId}", usuarioId);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }
    }
}