using Microsoft.EntityFrameworkCore;
using Reunioes.Application.Interfaces;
using Reunioes.Application.Services;
using Reunioes.Infra.Persistence;
using Reunioes.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados
builder.Services.AddDbContext<ReunioesDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// Repositories
builder.Services.AddScoped<ISalaRepository, SalaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// Services
builder.Services.AddScoped<ISalaService, SalaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<ISalaDisponibilidadeService, SalaDisponibilidadeService>();

var app = builder.Build();

// Swagger habilitado em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();