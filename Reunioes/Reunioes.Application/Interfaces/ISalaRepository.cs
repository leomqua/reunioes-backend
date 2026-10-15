using Reunioes.Domain.Entities;

namespace Reunioes.Application.Interfaces;

public interface ISalaRepository
{
    Task<Sala> AdicionarSala(Sala sala);

    Task<Sala?> ObterSalaPorId(Guid id);

    Task<bool> ExisteSalaComNomeEAndar(string nome, int andar, Guid? idIgnorado = null);

    Task<(List<Sala> Salas, int Total)> ListarSalas(string? nome, int pagina, int tamanhoPagina);

    Task AtualizarSala(Sala sala);

    Task ExcluirSala(Sala sala);

    Task<bool> SalaPossuiReservas(Guid salaId);

    Task<List<Sala>> ListarTodasSalas();
}