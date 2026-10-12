namespace Reunioes.Domain.Entities;

public class Sala
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nome { get; set; } = string.Empty;

    public int Andar { get; set; }

    public int QuantidadeAssentos { get; set; }

    public ICollection<Reserva> Reservas { get; set; }
        = new List<Reserva>();
}