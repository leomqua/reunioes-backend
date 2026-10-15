using Microsoft.AspNetCore.Mvc;
using Reunioes.Application.Interfaces;
using Reunioes.Application.Models.Relatorios;

namespace Reunioes.API.Controllers;

[ApiController]
[Route("api/relatorios")]
public sealed class RelatoriosController : ControllerBase
{
    private readonly ISalaDisponibilidadeService _service;

    public RelatoriosController(ISalaDisponibilidadeService service)
    {
        _service = service;
    }

    [HttpGet("salas/disponibilidade")]
    [ProducesResponseType<IReadOnlyList<SalaDisponibilidadeResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterDisponibilidade()
    {
        var resultado = await _service.ObterDisponibilidadeSalas();

        return Ok(resultado);
    }
}
