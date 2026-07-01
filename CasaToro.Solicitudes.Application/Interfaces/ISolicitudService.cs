using CasaToro.Solicitudes.Application.DTOs;

namespace CasaToro.Solicitudes.Application.Interfaces
{
    // Contrato que define qué puede hacer el servicio de solicitudes
    public interface ISolicitudService
    {
        Task<SolicitudResponseDTO> CrearSolicitudAsync(CrearSolicitudDTO dto);
        Task<SolicitudResponseDTO> CambiarEstadoAsync(CambiarEstadoDTO dto);
        Task<List<SolicitudResponseDTO>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<SolicitudDetalleDTO> ObtenerPorIdAsync(int solicitudId);
    }
}