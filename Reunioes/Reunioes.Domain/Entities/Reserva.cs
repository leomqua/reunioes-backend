namespace Reunioes.Domain.Entities;

public class Reserva
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime Inicio { get; set; }

    public DateTime Fim { get; set; }

    public DateTime CriadaEm { get; set; } = DateTime.Now;

    public Guid SalaId { get; set; }

    public Sala Sala { get; set; } = null!;
}