using CasaToro.Solicitudes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaToro.Solicitudes.Infrastructure
{
    // Representa la conexión a la base de datos
    // Cada DbSet es una tabla en SQL Server
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla Solicitudes
            modelBuilder.Entity<Solicitud>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Numero).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Observaciones).HasMaxLength(500);

                // Relación: una Solicitud tiene muchos Seguimientos
                entity.HasMany(s => s.Seguimientos)
                      .WithOne(seg => seg.Solicitud)
                      .HasForeignKey(seg => seg.SolicitudId);

                // Relación: una Solicitud pertenece a un Usuario
                entity.HasOne(s => s.Usuario)
                      .WithMany()
                      .HasForeignKey(s => s.UsuarioId);
            });

            // Configuración de la tabla Seguimientos
            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Comentario).IsRequired().HasMaxLength(500);
            });

            // Configuración de la tabla Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            });
        }
    }
}