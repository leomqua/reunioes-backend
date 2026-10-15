using Microsoft.AspNetCore.Mvc;
using Reunioes.Application.Interfaces;
using Reunioes.Application.Models.Reservas;

namespace Reunioes.API.Controllers;

[ApiController]
[Route("api/reservas")]
public sealed class ReservasController : ControllerBase
{
    private readonly IReservaService _service;

    public ReservasController(IReservaService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<ReservaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarReservaRequest request)
    {
        try
        {
            var reserva = await _service.CriarReserva(request);

            if (reserva is null)
                return NotFound(CriarProblema(StatusCodes.Status404NotFound,"Sala não encontrada."));

            return CreatedAtAction(nameof(ObterPorId), new { id = reserva.Id }, reserva);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(CriarProblema(StatusCodes.Status400BadRequest, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(CriarProblema(StatusCodes.Status409Conflict, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ReservaResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarTodas()
    {
        var reservas = await _service.ListarReservas();

        return Ok(reservas);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ReservaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var reserva = await _service.ObterReservaPorId(id);

        return reserva is null ? NotFound() : Ok(reserva);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reagendar(Guid id, [FromBody] ReagendarReservaRequest request)
    {
        try
        {
            var reagendada = await _service.ReagendarReserva(id, request);

            return reagendada ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(CriarProblema(StatusCodes.Status400BadRequest, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(CriarProblema(StatusCodes.Status409Conflict, ex.Message));
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        var cancelada = await _service.CancelarReserva(id);

        return cancelada ? NoContent() : NotFound();
    }

    private ProblemDetails CriarProblema(int status, string detalhe)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = status == StatusCodes.Status409Conflict
                ? "Conflito"
                : status == StatusCodes.Status404NotFound
                    ? "Recurso não encontrado"
                    : "Requisição inválida",
            Detail = detalhe,
            Instance = Request.Path
        };
    }
}
