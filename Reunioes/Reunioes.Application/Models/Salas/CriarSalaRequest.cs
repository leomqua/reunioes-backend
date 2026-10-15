using System.ComponentModel.DataAnnotations;

namespace Reunioes.Application.Models.Salas;

public class CriarSalaRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O andar é obrigatório.")]
    public int? Andar { get; set; }

    [Required(ErrorMessage = "A quantidade de assentos é obrigatória.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "A sala deve possuir pelo menos um assento.")]
    public int? QuantidadeAssentos { get; set; }
}