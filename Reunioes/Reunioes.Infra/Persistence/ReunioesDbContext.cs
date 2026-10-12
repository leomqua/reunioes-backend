using Microsoft.EntityFrameworkCore;
using Reunioes.Domain.Entities;

namespace Reunioes.Infra.Persistence;

public class ReunioesDbContext : DbContext
{
    public ReunioesDbContext(
        DbContextOptions<ReunioesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sala> Salas => Set<Sala>();

    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Nome)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(s => s.Andar)
                .IsRequired();

            entity.Property(s => s.QuantidadeAssentos)
                .IsRequired();

            entity.HasMany(s => s.Reservas)
                .WithOne(r => r.Sala)
                .HasForeignKey(r => r.SalaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Inicio)
                .IsRequired();

            entity.Property(r => r.Fim)
                .IsRequired();

            entity.HasIndex(r => new
            {
                r.SalaId,
                r.Inicio,
                r.Fim
            });
        });
    }
}