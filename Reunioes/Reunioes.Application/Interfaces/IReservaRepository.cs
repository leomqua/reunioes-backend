using Reunioes.Domain.Entities;

namespace Reunioes.Application.Interfaces;

public interface IReservaRepository
{
    Task<bool> SalaExiste(Guid salaId);

    Task<Reserva?> ObterReservaPorId(Guid id);

    Task<bool> ExisteConflitoDeReserva(Guid salaId, DateTime inicio, DateTime fim, Guid? reservaIgnoradaId = null);

    Task<Reserva> AdicionarReserva(Reserva reserva);

    Task AtualizarReserva(Reserva reserva);

    Task ExcluirReserva(Reserva reserva);

    Task<List<Reserva>> ListarReservas();

    Task<List<Reserva>> ListarReservasQueSobrepoemPeriodo(DateTime inicio, DateTime fim);

    Task<List<Reserva>> ListarReservasCriadasNoPeriodo(DateTime inicio, DateTime fim);
}
