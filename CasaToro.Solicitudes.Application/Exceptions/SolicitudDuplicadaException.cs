namespace CasaToro.Solicitudes.Application.Exceptions
{
    // Se lanza cuando ya existe una solicitud del mismo tipo el mismo día (RN-01)
    public class SolicitudDuplicadaException : Exception
    {
        public SolicitudDuplicadaException(string mensaje) : base(mensaje) { }
    }
}