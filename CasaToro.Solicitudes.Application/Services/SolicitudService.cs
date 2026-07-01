using CasaToro.Solicitudes.Application.DTOs;
using CasaToro.Solicitudes.Application.Exceptions;
using CasaToro.Solicitudes.Application.Interfaces;
using CasaToro.Solicitudes.Domain.Entities;
using CasaToro.Solicitudes.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CasaToro.Solicitudes.Application.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ISeguimientoRepository _seguimientoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<SolicitudService> _logger;

        // Recibe todo por Inyección de Dependencias
        public SolicitudService(
            ISolicitudRepository solicitudRepository,
            ISeguimientoRepository seguimientoRepository,
            IUsuarioRepository usuarioRepository,
            ILogger<SolicitudService> logger)
        {
            _solicitudRepository = solicitudRepository;
            _seguimientoRepository = seguimientoRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<SolicitudResponseDTO> CrearSolicitudAsync(CrearSolicitudDTO dto)
        {
            // RN-02: Solo usuarios activos pueden registrar solicitudes
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(dto.UsuarioId);
            if (usuario == null || !usuario.Activo)
                throw new UsuarioInactivoException("El usuario no está activo o no existe.");

            // RN-01: No se permiten duplicados del mismo tipo el mismo día
            var existeDuplicado = await _solicitudRepository.ExisteDuplicadoAsync(
                dto.UsuarioId, dto.TipoId, DateTime.Today);
            if (existeDuplicado)
                throw new SolicitudDuplicadaException(
                    "Ya existe una solicitud de este tipo para hoy.");

            // RN-03: Toda solicitud inicia en estado Pendiente
            var solicitud = new Solicitud
            {
                Numero = GenerarNumero(),
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId,
                TipoId = dto.TipoId,
                Estado = EstadoSolicitud.Pendiente,
                Observaciones = dto.Observaciones
            };

            await _solicitudRepository.CrearAsync(solicitud);

            // RN-04: Cada cambio de estado genera un seguimiento
            await _seguimientoRepository.RegistrarAsync(new Seguimiento
            {
                SolicitudId = solicitud.Id,
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId,
                Comentario = "Solicitud creada",
                EstadoAnterior = null,
                EstadoNuevo = EstadoSolicitud.Pendiente
            });

            _logger.LogInformation(
                "Solicitud {Numero} creada por usuario {UsuarioId}",
                solicitud.Numero, dto.UsuarioId);

            return MapearAResponseDTO(solicitud, usuario);
        }

        public async Task<SolicitudResponseDTO> CambiarEstadoAsync(CambiarEstadoDTO dto)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(dto.SolicitudId);
            if (solicitud == null)
                throw new SolicitudNoEncontradaException();

            // RN-05: No se puede cerrar sin observación
            if (dto.NuevoEstado == EstadoSolicitud.Cerrada
                && string.IsNullOrWhiteSpace(dto.Comentario))
                throw new ObservacionRequeridaException(
                    "No se puede cerrar una solicitud sin observación.");

            var estadoAnterior = solicitud.Estado;
            solicitud.Estado = dto.NuevoEstado;
            await _solicitudRepository.ActualizarAsync(solicitud);

            // RN-04: Registrar el seguimiento del cambio
            await _seguimientoRepository.RegistrarAsync(new Seguimiento
            {
                SolicitudId = solicitud.Id,
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId,
                Comentario = dto.Comentario,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = dto.NuevoEstado
            });

            _logger.LogInformation(
                "Solicitud {Id} cambió de {EstadoAnterior} a {EstadoNuevo}",
                solicitud.Id, estadoAnterior, dto.NuevoEstado);

            return MapearAResponseDTO(solicitud, null);
        }

        public async Task<List<SolicitudResponseDTO>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            var solicitudes = await _solicitudRepository.ObtenerPorUsuarioAsync(usuarioId);
            return solicitudes.Select(s => MapearAResponseDTO(s, null)).ToList();
        }

        public async Task<SolicitudDetalleDTO> ObtenerPorIdAsync(int solicitudId)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(solicitudId);
            if (solicitud == null)
                throw new SolicitudNoEncontradaException();

            var seguimientos = await _seguimientoRepository
                .ObtenerPorSolicitudAsync(solicitudId);

            return new SolicitudDetalleDTO
            {
                Solicitud = MapearAResponseDTO(solicitud, null),
                Seguimientos = seguimientos
            };
        }

        // Genera un número único legible para el usuario
        private string GenerarNumero()
        {
            return $"SOL-{DateTime.Now:yyyy}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }

        // Convierte la entidad a DTO para no exponer la entidad directamente
        private SolicitudResponseDTO MapearAResponseDTO(Solicitud s, Usuario? usuario)
        {
            return new SolicitudResponseDTO
            {
                Id = s.Id,
                Numero = s.Numero,
                Fecha = s.Fecha,
                Estado = s.Estado.ToString(),
                TipoId = s.TipoId,
                Usuario = usuario?.Nombre ?? string.Empty
            };
        }
    }
}