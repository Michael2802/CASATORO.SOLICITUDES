using CasaToro.Solicitudes.Domain.Entities;

namespace CasaToro.Solicitudes.Application.Interfaces
{
    // Contrato que define qué operaciones de base de datos
    // puede hacer cualquier repositorio de solicitudes
    public interface ISolicitudRepository
    {
        Task<Solicitud> CrearAsync(Solicitud solicitud);
        Task<Solicitud?> ObtenerPorIdAsync(int id);
        Task<List<Solicitud>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<bool> ExisteDuplicadoAsync(int usuarioId, int tipoId, DateTime fecha);
        Task ActualizarAsync(Solicitud solicitud);
    }
}