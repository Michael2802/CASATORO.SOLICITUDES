namespace CasaToro.Solicitudes.Application.Exceptions
{
    // Se lanza cuando no existe la solicitud que se está buscando
    public class SolicitudNoEncontradaException : Exception
    {
        public SolicitudNoEncontradaException() : base("La solicitud no existe.") { }
    }
}