using Microsoft.EntityFrameworkCore;
using Reunioes.Application.Interfaces;
using Reunioes.Domain.Entities;
using Reunioes.Infra.Persistence;

namespace Reunioes.Infra.Repositories;

public class SalaRepository : ISalaRepository
{
    private readonly ReunioesDbContext _context;

    public SalaRepository(ReunioesDbContext context)
    {
        _context = context;
    }

    public async Task<Sala> AdicionarSala(Sala sala)
    {
        _context.Salas.Add(sala);

        await _context.SaveChangesAsync();

        return sala;
    }

    public async Task<Sala?> ObterSalaPorId(Guid id)
    {
        return await _context.Salas
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public Task<bool> ExisteSalaComNomeEAndar(string nome, int andar, Guid? idIgnorado = null)
    {
        var nomeNormalizado = nome.Trim().ToLower();

        return _context.Salas.AnyAsync(sala =>
            sala.Andar == andar &&
            sala.Nome.Trim().ToLower() == nomeNormalizado &&
            (!idIgnorado.HasValue || sala.Id != idIgnorado.Value));
    }

    public async Task<(List<Sala> Salas, int Total)> ListarSalas(string? nome, int pagina, int tamanhoPagina)
    {
        var query = _context.Salas.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(
                s => EF.Functions.ILike(s.Nome, $"%{nome.Trim()}%"));
        }

        var total = await query.CountAsync();

        var salas = await query
            .OrderBy(s => s.Andar)
            .ThenBy(s => s.Nome)
            .ThenBy(s => s.Id)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (salas, total);
    }

    public async Task AtualizarSala(Sala sala)
    {
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirSala(Sala sala)
    {
        _context.Salas.Remove(sala);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> SalaPossuiReservas(Guid salaId)
    {
        return await _context.Reservas
            .AnyAsync(r => r.SalaId == salaId);
    }

    public async Task<List<Sala>> ListarTodasSalas()
    {
        return await _context.Salas
            .AsNoTracking()
            .OrderBy(s => s.Andar)
            .ThenBy(s => s.Nome)
            .ThenBy(s => s.Id)
            .ToListAsync();
    }
}