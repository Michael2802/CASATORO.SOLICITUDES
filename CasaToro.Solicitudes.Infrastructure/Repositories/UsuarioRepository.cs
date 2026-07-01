using CasaToro.Solicitudes.Application.Interfaces;
using CasaToro.Solicitudes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaToro.Solicitudes.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}