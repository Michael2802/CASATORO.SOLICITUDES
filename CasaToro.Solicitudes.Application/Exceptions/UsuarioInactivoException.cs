namespace CasaToro.Solicitudes.Application.Exceptions
{
    // Se lanza cuando un usuario inactivo intenta crear una solicitud (RN-02)
    public class UsuarioInactivoException : Exception
    {
        public UsuarioInactivoException(string mensaje) : base(mensaje) { }
    }
}