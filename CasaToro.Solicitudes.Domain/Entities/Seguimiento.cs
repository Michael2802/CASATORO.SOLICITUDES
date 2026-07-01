using CasaToro.Solicitudes.Domain.Enums;

namespace CasaToro.Solicitudes.Domain.Entities
{
    // Registra cada cambio de estado que sufre una solicitud (RN-04)
    public class Seguimiento
    {
        public int Id { get; set; }

        // A qué solicitud pertenece este seguimiento
        public int SolicitudId { get; set; }
        public Solicitud? Solicitud { get; set; }

        // Cuándo ocurrió el cambio
        public DateTime Fecha { get; set; }

        // Quién hizo el cambio
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Comentario obligatorio al cambiar estado
        public string Comentario { get; set; } = string.Empty;

        // De qué estado venía
        public EstadoSolicitud? EstadoAnterior { get; set; }

        // A qué estado pasó
        public EstadoSolicitud EstadoNuevo { get; set; }
    }
}