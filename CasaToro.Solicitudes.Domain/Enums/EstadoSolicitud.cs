namespace CasaToro.Solicitudes.Domain.Enums
{
    // Define los estados posibles de una solicitud
    // Cada valor tiene un número asociado que se guarda en la base de datos
    public enum EstadoSolicitud
    {
        Pendiente = 1,      // Estado inicial de toda solicitud
        EnRevision = 2,     // Alguien la está revisando
        Aprobada = 3,       // Fue aprobada
        Rechazada = 4,      // Fue rechazada
        Cerrada = 5         // Fue cerrada (requiere observación obligatoria)
    }
}