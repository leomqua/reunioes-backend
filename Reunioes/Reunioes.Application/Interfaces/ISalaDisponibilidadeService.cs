using Reunioes.Application.Models.Relatorios;

namespace Reunioes.Application.Interfaces;

public interface ISalaDisponibilidadeService
{
    Task<IReadOnlyList<SalaDisponibilidadeResponse>> ObterDisponibilidadeSalas();
}
