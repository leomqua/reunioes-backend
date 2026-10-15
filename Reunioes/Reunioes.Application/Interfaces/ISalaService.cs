using Reunioes.Application.Models.Salas;

namespace Reunioes.Application.Interfaces;

public interface ISalaService
{
    Task<SalaResponse> CriarSala(CriarSalaRequest request);

    Task<SalaResponse?> ObterSalaPorId(Guid id);

    Task<object> ListarSalas(string? nome, int pagina);

    Task<bool> AtualizarSala(Guid id, AtualizarSalaRequest request);

    Task<bool> ExcluirSala(Guid id);
}