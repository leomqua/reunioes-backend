namespace Reunioes.Application.Models.Salas;

public class SalaResponse
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int Andar { get; set; }

    public int QuantidadeAssentos { get; set; }
}