using CloudTrack.Competitions.Application.Competitions;
using CloudTrack.Competitions.Application.Competitions.Commands;
using CloudTrack.Competitions.Application.Competitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudTrack.Competitions.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetitionController(
    IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody]CreateCompetitionDto dto)
    {
        var command = new CreateCompetitionCommand(dto.Name, dto.StartAt, dto.Distance, dto.Place, dto.MaxCompetitors);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAsync), new { id }, null);
    }

    [HttpGet]
    [ProducesResponseType(typeof(CompetitionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery]string? search)
    {
        var query = new GetCompetitionListQuery(search);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CompetitionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var query = new GetCompetitionQuery(id);
        var dto = await _mediator.Send(query);
        return Ok(dto);
    }

    [HttpPost("{id:Guid}/Registration/Open")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> OpenRegistrationAsync(Guid id)
    {
        var command = new OpenRegistrationCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:Guid}/Registration/Complete")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteRegistrationAsync(Guid id)
    {
        var command = new CompleteRegistrationCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangeMaxCompetitors(Guid id, [FromBody]ChangeMaxCompetitorsRequestDto dto)
    {
        var command = new ChangeMaxCompetitorsCommand(id, dto.MaxCompetitors);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:Guid}/Checkpoints")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddCheckpoint(Guid id, [FromBody] AddCheckpointRequestDto dto)
    {
        var command = new AddCheckpointRequestCommand(id, dto.TrackPointAmount, dto.TrackPointUnit);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:Guid}/Checkpoints/{checkpointId:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveCheckpoint(Guid id, Guid checkpointId)
    {
        var command = new RemoveCheckpointCommand(id, checkpointId);
        await _mediator.Send(command);
        return NoContent();
    }
}
