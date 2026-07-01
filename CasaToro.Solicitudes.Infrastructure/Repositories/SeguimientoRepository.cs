using CasaToro.Solicitudes.Application.Interfaces;
using CasaToro.Solicitudes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaToro.Solicitudes.Infrastructure.Repositories
{
    public class SeguimientoRepository : ISeguimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public SeguimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Seguimiento seguimiento)
        {
            _context.Seguimientos.Add(seguimiento);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Seguimiento>> ObtenerPorSolicitudAsync(int solicitudId)
        {
            return await _context.Seguimientos
                .Where(s => s.SolicitudId == solicitudId)
                .OrderBy(s => s.Fecha)
                .ToListAsync();
        }
    }
}