using System.ComponentModel.DataAnnotations;

namespace Reunioes.Application.Models.Reservas;

public sealed record CriarReservaRequest
{
    /// <summary>Identificador da sala que será reservada.</summary>
    [Required]
    public Guid? SalaId { get; init; }

    /// <summary>Data da reserva, no formato yyyy-MM-dd.</summary>
    [Required]
    public DateOnly? Data { get; init; }

    /// <summary>Horário de início no formato HH:mm.</summary>
    [Required]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$")]
    public string? HoraInicio { get; init; }

    /// <summary>Horário de término no formato HH:mm.</summary>
    [Required]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$")]
    public string? HoraFim { get; init; }
}
