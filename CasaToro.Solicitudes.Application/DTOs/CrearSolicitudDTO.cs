namespace CasaToro.Solicitudes.Application.DTOs
{
    // Datos que el cliente envía para crear una solicitud
    public class CrearSolicitudDTO
    {
        public int UsuarioId { get; set; }
        public int TipoId { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}