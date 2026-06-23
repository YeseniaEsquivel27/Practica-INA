using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practica_INA.Business.Interfaces;
using Practica_INA.Business.Mapping;
using Practica_INA.Business.Services;
using Practica_INA.Data.Context;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PracticaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaProductoService, CategoriaProductoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
