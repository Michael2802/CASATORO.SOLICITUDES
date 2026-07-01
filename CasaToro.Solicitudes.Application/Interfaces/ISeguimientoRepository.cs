using CasaToro.Solicitudes.Domain.Entities;

namespace CasaToro.Solicitudes.Application.Interfaces
{
    // Contrato para operaciones de seguimientos
    public interface ISeguimientoRepository
    {
        Task RegistrarAsync(Seguimiento seguimiento);
        Task<List<Seguimiento>> ObtenerPorSolicitudAsync(int solicitudId);
    }
}