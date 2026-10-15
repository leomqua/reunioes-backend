using Reunioes.Application.Interfaces;
using Reunioes.Application.Models.Reservas;
using Reunioes.Domain.Entities;

namespace Reunioes.Application.Services;

public sealed class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;

    public ReservaService(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public async Task<ReservaResponse?> CriarReserva(CriarReservaRequest request)
    {
        var salaId = request.SalaId!.Value;
        var data = request.Data!.Value;
        var inicioSolicitado = ReservaHorario.Combinar(data, request.HoraInicio!);
        var fimSolicitado = ReservaHorario.Combinar(data, request.HoraFim!);
        var (inicio, fim) = ReservaHorario.Validar(inicioSolicitado, fimSolicitado);

        if (!await _reservaRepository.SalaExiste(salaId))
        {
            return null;
        }

        if (await _reservaRepository.ExisteConflitoDeReserva(
                salaId, inicio, fim))
        {
            throw new InvalidOperationException(
                "A sala já possui uma reserva nesse horário.");
        }

        var reserva = new Reserva
        {
            SalaId = salaId,
            Inicio = inicio,
            Fim = fim,
            CriadaEm = ObterAgora()
        };

        await _reservaRepository.AdicionarReserva(reserva);

        return Mapear(reserva);
    }

    public async Task<ReservaResponse?> ObterReservaPorId(Guid id)
    {
        var reserva = await _reservaRepository.ObterReservaPorId(id);

        return reserva is null ? null : Mapear(reserva);
    }

    public async Task<IReadOnlyList<ReservaResponse>> ListarReservas()
    {
        var reservas = await _reservaRepository.ListarReservas();

        return reservas.Select(Mapear).ToList();
    }

    public async Task<bool> ReagendarReserva(Guid id, ReagendarReservaRequest request)
    {
        var reserva = await _reservaRepository.ObterReservaPorId(id);

        if (reserva is null)
            return false;

        if (reserva.Inicio <= ObterAgora())
        {
            throw new InvalidOperationException(
                "Somente reservas futuras podem ser reagendadas.");
        }

        var data = request.Data!.Value;
        var novoInicio = ReservaHorario.Combinar(data, request.HoraInicio!);
        if (novoInicio <= ObterAgora())
        {
            throw new ArgumentException(
                "A nova data da reserva deve estar no futuro.");
        }

        var salaId = request.SalaId!.Value;
        var novoFim = ReservaHorario.Combinar(data, request.HoraFim!);
        var (inicio, fim) = ReservaHorario.Validar(novoInicio, novoFim);

        if (!await _reservaRepository.SalaExiste(salaId))
        {
            return false;
        }

        if (await _reservaRepository.ExisteConflitoDeReserva(
                salaId,
                inicio,
                fim,
                id))
        {
            throw new InvalidOperationException(
                "A sala já possui uma reserva nesse horário.");
        }

        reserva.SalaId = salaId;
        reserva.Inicio = inicio;
        reserva.Fim = fim;

        await _reservaRepository.AtualizarReserva(reserva);

        return true;
    }

    public async Task<bool> CancelarReserva(Guid id)
    {
        var reserva = await _reservaRepository.ObterReservaPorId(id);

        if (reserva is null)
            return false;

        await _reservaRepository.ExcluirReserva(reserva);

        return true;
    }

    private static ReservaResponse Mapear(Reserva reserva)
    {
        return new ReservaResponse(
            reserva.Id,
            reserva.SalaId,
            reserva.Inicio,
            reserva.Fim);
    }

    private static DateTime ObterAgora()
    {
        return DateTime.Now;
    }
}
