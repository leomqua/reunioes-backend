using Microsoft.EntityFrameworkCore;
using Reunioes.Application.Interfaces;
using Reunioes.Domain.Entities;
using Reunioes.Infra.Persistence;

namespace Reunioes.Infra.Repositories;

public sealed class ReservaRepository : IReservaRepository
{
    private readonly ReunioesDbContext _context;

    public ReservaRepository(ReunioesDbContext context)
    {
        _context = context;
    }

    public Task<bool> SalaExiste(Guid salaId)
    {
        return _context.Salas.AnyAsync(
            sala => sala.Id == salaId);
    }

    public Task<Reserva?> ObterReservaPorId(Guid id)
    {
        return _context.Reservas.FirstOrDefaultAsync(
            reserva => reserva.Id == id);
    }

    public Task<bool> ExisteConflitoDeReserva(Guid salaId, DateTime inicio, DateTime fim, Guid? reservaIgnoradaId = null)
    {
        return _context.Reservas.AnyAsync(
            reserva =>
                reserva.SalaId == salaId &&
                reserva.Inicio < fim &&
                reserva.Fim > inicio &&
                (!reservaIgnoradaId.HasValue ||
                 reserva.Id != reservaIgnoradaId.Value));
    }

    public async Task<Reserva> AdicionarReserva(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        return reserva;
    }

    public async Task AtualizarReserva(Reserva reserva)
    {
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirReserva(Reserva reserva)
    {
        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
    }

    public Task<List<Reserva>> ListarReservas()
    {
        return _context.Reservas
            .AsNoTracking()
            .OrderBy(reserva => reserva.Inicio)
            .ThenBy(reserva => reserva.Id)
            .ToListAsync();
    }

    public Task<List<Reserva>> ListarReservasQueSobrepoemPeriodo(DateTime inicio, DateTime fim)
    {
        return _context.Reservas
            .AsNoTracking()
            .Where(reserva => reserva.Inicio < fim && reserva.Fim > inicio)
            .ToListAsync();
    }

    public Task<List<Reserva>> ListarReservasCriadasNoPeriodo(DateTime inicio, DateTime fim)
    {
        return _context.Reservas
            .AsNoTracking()
            .Where(reserva => reserva.CriadaEm >= inicio && reserva.CriadaEm <= fim)
            .ToListAsync();
    }
}
