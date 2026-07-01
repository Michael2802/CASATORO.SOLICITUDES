namespace CasaToro.Solicitudes.Application.DTOs
{
    // Datos que la API devuelve al cliente
    // Nunca se expone la entidad directamente
    public class SolicitudResponseDTO
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int TipoId { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}