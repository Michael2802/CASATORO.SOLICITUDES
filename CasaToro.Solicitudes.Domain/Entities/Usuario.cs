namespace CasaToro.Solicitudes.Domain.Entities
{
    // Representa un usuario del sistema
    // Solo usuarios activos pueden crear solicitudes (RN-02)
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Si es false, no puede registrar solicitudes
        public bool Activo { get; set; }
    }
}