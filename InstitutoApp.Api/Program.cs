using InstitutoApp.Common.Interfaces;
using InstitutoApp.Data.Context;
using InstitutoApp.Data.Semilla;
using InstitutoApp.Repository.Repositorios;
using InstitutoApp.Services.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    var cadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection");

    opciones.UseSqlServer(cadenaConexion);
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(opciones =>
{
    opciones.InvalidModelStateResponseFactory = contexto =>
    {
        var errores = contexto.ModelState
            .Where(campo => campo.Value?.Errors.Count > 0)
            .ToDictionary(
                campo => campo.Key,
                campo => campo.Value!.Errors
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        var respuesta = new
        {
            esExitosa = false,
            mensaje = "La información enviada no es válida.",
            datos = errores
        };

        return new BadRequestObjectResult(respuesta);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioEstudiante, RepositorioEstudiante>();
builder.Services.AddScoped<IRepositorioCurso, RepositorioCurso>();
builder.Services.AddScoped<IRepositorioMatricula, RepositorioMatricula>();
builder.Services.AddScoped<IServicioEstudiante, ServicioEstudiante>();
builder.Services.AddScoped<IServicioCurso, ServicioCurso>();
builder.Services.AddScoped<IServicioMatricula, ServicioMatricula>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await InicializadorBaseDatos.CargarDatosInicialesAsync(context);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
