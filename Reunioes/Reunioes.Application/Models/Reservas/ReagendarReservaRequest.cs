using System.ComponentModel.DataAnnotations;

namespace Reunioes.Application.Models.Reservas;

public sealed record ReagendarReservaRequest
{
    /// <summary>Identificador da sala para a qual a reserva será agendada.</summary>
    [Required]
    public Guid? SalaId { get; init; }

    /// <summary>Nova data da reserva, no formato yyyy-MM-dd.</summary>
    [Required]
    public DateOnly? Data { get; init; }

    /// <summary>Novo horário de início no formato HH:mm.</summary>
    [Required]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$")]
    public string? HoraInicio { get; init; }

    /// <summary>Novo horário de término no formato HH:mm.</summary>
    [Required]
    [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d$")]
    public string? HoraFim { get; init; }
}
