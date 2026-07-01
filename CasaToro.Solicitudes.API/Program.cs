using CasaToro.Solicitudes.Application.Interfaces;
using CasaToro.Solicitudes.Application.Services;
using CasaToro.Solicitudes.Infrastructure;
using CasaToro.Solicitudes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════
// 1. BASE DE DATOS
// Registra el DbContext con la cadena de conexión
// ═══════════════════════════════════════════════
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ═══════════════════════════════════════════════
// 2. REPOSITORIOS
// Cada vez que alguien pida ISolicitudRepository
// .NET entrega un SolicitudRepository
// ═══════════════════════════════════════════════
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<ISeguimientoRepository, SeguimientoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// ═══════════════════════════════════════════════
// 3. SERVICIOS
// Cada vez que alguien pida ISolicitudService
// .NET entrega un SolicitudService
// ═══════════════════════════════════════════════
builder.Services.AddScoped<ISolicitudService, SolicitudService>();

// ═══════════════════════════════════════════════
// 4. CONTROLLERS Y SWAGGER
// Swagger es la interfaz visual para probar la API
// ═══════════════════════════════════════════════
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ═══════════════════════════════════════════════
// 5. MIDDLEWARE
// Swagger solo disponible en desarrollo
// ═══════════════════════════════════════════════
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();