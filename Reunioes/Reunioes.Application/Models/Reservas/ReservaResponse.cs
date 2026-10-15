namespace Reunioes.Application.Models.Reservas;

/// <summary>Dados de uma reserva de sala.</summary>
public sealed record ReservaResponse(
    Guid Id,
    Guid SalaId,
    DateTime Inicio,
    DateTime Fim);
