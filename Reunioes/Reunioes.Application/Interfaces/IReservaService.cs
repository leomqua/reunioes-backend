using Reunioes.Application.Models.Reservas;

namespace Reunioes.Application.Interfaces;

public interface IReservaService
{
    Task<ReservaResponse?> CriarReserva(CriarReservaRequest request);

    Task<IReadOnlyList<ReservaResponse>> ListarReservas();

    Task<ReservaResponse?> ObterReservaPorId(Guid id);

    Task<bool> ReagendarReserva(Guid id, ReagendarReservaRequest request);

    Task<bool> CancelarReserva(Guid id);
}
