using Reunioes.Application.Models.Salas;
using Reunioes.Application.Interfaces;
using Reunioes.Domain.Entities;

namespace Reunioes.Application.Services;

public class SalaService : ISalaService
{
    private readonly ISalaRepository _repository;

    public SalaService(ISalaRepository repository)
    {
        _repository = repository;
    }

    public async Task<SalaResponse> CriarSala(CriarSalaRequest request)
    {
        var nome = request.Nome.Trim();
        var andar = request.Andar!.Value;

        if (await _repository.ExisteSalaComNomeEAndar(nome, andar))
            throw new InvalidOperationException(
                "Já existe uma sala com esse nome nesse andar.");

        var sala = new Sala
        {
            Nome = nome,
            Andar = andar,
            QuantidadeAssentos = request.QuantidadeAssentos!.Value
        };

        await _repository.AdicionarSala(sala);

        return Mapear(sala);
    }

    public async Task<SalaResponse?> ObterSalaPorId(Guid id)
    {
        var sala = await _repository.ObterSalaPorId(id);

        return sala is null ? null : Mapear(sala);
    }

    public async Task<object> ListarSalas(string? nome, int pagina)
    {
        const int tamanhoPagina = 10;

        if (pagina < 1)
            throw new ArgumentException(
                "A página deve ser maior que zero.");

        var resultado = await _repository.ListarSalas(nome, pagina, tamanhoPagina);

        return new
        {
            pagina,
            tamanhoPagina,
            totalRegistros = resultado.Total,
            totalPaginas = (int)Math.Ceiling(
                resultado.Total / (double)tamanhoPagina),
            dados = resultado.Salas.Select(Mapear).ToList()
        };
    }

    public async Task<bool> AtualizarSala(Guid id, AtualizarSalaRequest request)
    {
        var sala = await _repository.ObterSalaPorId(id);

        if (sala is null)
            return false;

        var nome = request.Nome.Trim();
        var andar = request.Andar!.Value;

        if (await _repository.ExisteSalaComNomeEAndar(nome, andar, id))
            throw new InvalidOperationException(
                "Já existe uma sala com esse nome nesse andar.");

        sala.Nome = nome;
        sala.Andar = andar;
        sala.QuantidadeAssentos = request.QuantidadeAssentos!.Value;

        await _repository.AtualizarSala(sala);

        return true;
    }

    public async Task<bool> ExcluirSala(Guid id)
    {
        var sala = await _repository.ObterSalaPorId(id);

        if (sala is null)
            return false;

        var possuiReservas = await _repository.SalaPossuiReservas(id);

        if (possuiReservas)
            throw new InvalidOperationException(
                "Não é possível excluir uma sala que possui reservas.");

        await _repository.ExcluirSala(sala);

        return true;
    }

    private static SalaResponse Mapear(Sala sala)
    {
        return new SalaResponse
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Andar = sala.Andar,
            QuantidadeAssentos = sala.QuantidadeAssentos
        };
    }
}