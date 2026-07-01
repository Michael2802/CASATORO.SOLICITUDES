using CasaToro.Solicitudes.Domain.Enums;

namespace CasaToro.Solicitudes.Application.DTOs
{
    // Datos para cambiar el estado de una solicitud existente
    public class CambiarEstadoDTO
    {
        public int SolicitudId { get; set; }
        public EstadoSolicitud NuevoEstado { get; set; }

        // Obligatorio cuando el nuevo estado es Cerrada (RN-05)
        public string Comentario { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}