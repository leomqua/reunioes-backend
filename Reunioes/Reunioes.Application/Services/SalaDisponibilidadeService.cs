using Reunioes.Application.Interfaces;
using Reunioes.Application.Models.Relatorios;

namespace Reunioes.Application.Services;

public sealed class SalaDisponibilidadeService : ISalaDisponibilidadeService
{
    private readonly ISalaRepository _salaRepository;
    private readonly IReservaRepository _reservaRepository;

    public SalaDisponibilidadeService(ISalaRepository salaRepository, IReservaRepository reservaRepository)
    {
        _salaRepository = salaRepository;
        _reservaRepository = reservaRepository;
    }

    public async Task<IReadOnlyList<SalaDisponibilidadeResponse>> ObterDisponibilidadeSalas()
    {
        var agora = DateTime.Now;
        var janelaHoje = ReservaHorario.JanelaDoDia(agora);
        var inicioUltimos7Dias = agora.AddDays(-7);

        var salas = await _salaRepository.ListarTodasSalas();
        var reservas = await _reservaRepository.ListarReservasQueSobrepoemPeriodo(janelaHoje.Inicio, janelaHoje.Fim);
        var reservasRecentes = await _reservaRepository.ListarReservasCriadasNoPeriodo(inicioUltimos7Dias, agora);

        return salas.Select(sala =>
        {
            var reservasDaSala = reservas
                .Where(reserva => reserva.SalaId == sala.Id)
                .ToList();

            var horasDisponiveis = (janelaHoje.Fim - janelaHoje.Inicio).TotalHours -
                reservasDaSala.Sum(reserva =>
                    SobreposicaoEmHoras(janelaHoje.Inicio, janelaHoje.Fim, reserva.Inicio, reserva.Fim));
            var quantidadeReservasRecentes = reservasRecentes.Count(reserva =>
                reserva.SalaId == sala.Id);

            return new SalaDisponibilidadeResponse(
                sala.Id,
                sala.Nome,
                sala.Andar,
                sala.QuantidadeAssentos,
                decimal.Round((decimal)Math.Max(0, horasDisponiveis), 2),
                quantidadeReservasRecentes);
        }).ToList();
    }

    private static double SobreposicaoEmHoras(DateTime inicioJanela, DateTime fimJanela, DateTime inicioReserva, DateTime fimReserva)
    {
        var inicio = inicioJanela > inicioReserva
            ? inicioJanela
            : inicioReserva;
        var fim = fimJanela < fimReserva
            ? fimJanela
            : fimReserva;

        return fim > inicio ? (fim - inicio).TotalHours : 0;
    }
}
