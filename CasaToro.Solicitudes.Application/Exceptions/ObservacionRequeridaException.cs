namespace CasaToro.Solicitudes.Application.Exceptions
{
    // Se lanza cuando se intenta cerrar una solicitud sin observación (RN-05)
    public class ObservacionRequeridaException : Exception
    {
        public ObservacionRequeridaException(string mensaje) : base(mensaje) { }
    }
}