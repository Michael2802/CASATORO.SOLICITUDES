using CasaToro.Solicitudes.Domain.Entities;

namespace CasaToro.Solicitudes.Application.DTOs
{
    // Detalle completo de una solicitud incluyendo sus seguimientos
    public class SolicitudDetalleDTO
    {
        public SolicitudResponseDTO Solicitud { get; set; } = new();
        public List<Seguimiento> Seguimientos { get; set; } = new();
    }
}