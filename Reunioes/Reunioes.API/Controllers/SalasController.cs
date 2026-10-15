using Microsoft.AspNetCore.Mvc;
using Reunioes.Application.Models.Salas;
using Reunioes.Application.Interfaces;

namespace Reunioes.API.Controllers;

[ApiController]
[Route("api/salas")]
public class SalasController : ControllerBase
{
    private readonly ISalaService _service;

    public SalasController(ISalaService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<SalaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarSalaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("O nome da sala é obrigatório.");

        try
        {
            var sala = await _service.CriarSala(request);

            return CreatedAtAction(nameof(ObterPorId), new { id = sala.Id }, sala);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(CriarProblemaDeDuplicidade(ex.Message));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var sala = await _service.ObterSalaPorId(id);

        if (sala is null)
            return NotFound();

        return Ok(sala);
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? nome, [FromQuery] int pagina = 1)
    {
        if (pagina < 1)
            return BadRequest("A página deve ser maior que zero.");

        var resultado = await _service.ListarSalas(nome, pagina);

        return Ok(resultado);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarSalaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("O nome da sala é obrigatório.");

        try
        {
            var atualizado = await _service.AtualizarSala(id, request);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(CriarProblemaDeDuplicidade(ex.Message));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        try
        {
            var excluido = await _service.ExcluirSala(id);

            if (!excluido)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    private ProblemDetails CriarProblemaDeDuplicidade(string detalhe)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Sala duplicada",
            Detail = detalhe,
            Instance = Request.Path
        };
    }
}