using CasaToro.Solicitudes.Domain.Enums;

namespace CasaToro.Solicitudes.Domain.Entities
{
    // Representa una solicitud registrada en el sistema
    public class Solicitud
    {
        public int Id { get; set; }

        // Número legible para el usuario, ej: SOL-2024-ABC123
        public string Numero { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        // Quién creó la solicitud
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Qué tipo de solicitud es (catálogo)
        public int TipoId { get; set; }

        // Estado actual (inicia siempre en Pendiente)
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;

        public string Observaciones { get; set; } = string.Empty;

        // Lista de todos los cambios de estado que ha tenido
        public List<Seguimiento> Seguimientos { get; set; } = new();
    }
}