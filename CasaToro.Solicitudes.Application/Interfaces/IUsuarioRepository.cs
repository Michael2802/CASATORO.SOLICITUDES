using CasaToro.Solicitudes.Domain.Entities;

namespace CasaToro.Solicitudes.Application.Interfaces
{
    // Contrato para consultar usuarios
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
    }
}