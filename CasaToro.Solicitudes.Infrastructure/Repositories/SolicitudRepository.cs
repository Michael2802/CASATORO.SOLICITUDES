using CasaToro.Solicitudes.Application.Interfaces;
using CasaToro.Solicitudes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaToro.Solicitudes.Infrastructure.Repositories
{
    // Implementación real del repositorio de solicitudes
    // Aquí es donde se ejecutan las consultas contra SQL Server
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly ApplicationDbContext _context;

        public SolicitudRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Solicitud> CrearAsync(Solicitud solicitud)
        {
            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(int id)
        {
            return await _context.Solicitudes
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Solicitud>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Solicitudes
                .Where(s => s.UsuarioId == usuarioId)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
        }

        public async Task<bool> ExisteDuplicadoAsync(int usuarioId, int tipoId, DateTime fecha)
        {
            // Valida RN-01: no duplicados del mismo tipo el mismo día
            return await _context.Solicitudes
                .AnyAsync(s => s.UsuarioId == usuarioId
                            && s.TipoId == tipoId
                            && s.Fecha.Date == fecha.Date);
        }

        public async Task ActualizarAsync(Solicitud solicitud)
        {
            _context.Solicitudes.Update(solicitud);
            await _context.SaveChangesAsync();
        }
    }
}