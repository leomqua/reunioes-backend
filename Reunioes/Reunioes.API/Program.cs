using Microsoft.EntityFrameworkCore;
using Reunioes.Infra.Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<ReunioesDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.Run();