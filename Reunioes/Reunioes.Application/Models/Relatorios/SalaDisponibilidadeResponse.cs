namespace Reunioes.Application.Models.Relatorios;

/// <summary>Disponibilidade e utilização recente de uma sala.</summary>
public sealed record SalaDisponibilidadeResponse(
    Guid SalaId,
    string Nome,
    int Andar,
    int QuantidadeAssentos,
    decimal HorasLivresNoPeriodo,
    int ReservasNosUltimos7Dias);
