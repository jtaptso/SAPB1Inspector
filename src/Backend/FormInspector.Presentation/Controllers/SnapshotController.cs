using FormInspector.Application.DTOs;
using FormInspector.Application.UseCases.GetLatestSnapshot;
using FormInspector.Application.UseCases.ReceiveSnapshot;
using Microsoft.AspNetCore.Mvc;

namespace FormInspector.Presentation.Controllers;

/// <summary>
/// REST API controller for snapshot operations.
/// Accepts snapshots from the SAP add-on and serves queries from the Blazor viewer.
/// Contains no business logic — delegates to application use cases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SnapshotController : ControllerBase
{
    private readonly ReceiveSnapshotHandler _receiveHandler;
    private readonly GetLatestSnapshotHandler _queryHandler;

    public SnapshotController(
        ReceiveSnapshotHandler receiveHandler,
        GetLatestSnapshotHandler queryHandler)
    {
        _receiveHandler = receiveHandler;
        _queryHandler = queryHandler;
    }

    /// <summary>
    /// Receives a snapshot from the SAP add-on via HTTP POST.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SnapshotInputDto snapshotDto)
    {
        if (snapshotDto is null)
            return BadRequest("Snapshot payload is required.");

        var command = new ReceiveSnapshotCommand { Snapshot = snapshotDto };

        var snapshotId = await _receiveHandler.HandleAsync(command);

        return Ok(new { SnapshotId = snapshotId });
    }

    /// <summary>
    /// Gets the latest snapshot for a given form type.
    /// </summary>
    [HttpGet("latest/{formType}")]
    public async Task<IActionResult> GetLatest(string formType)
    {
        var query = new GetLatestSnapshotQuery { FormType = formType };
        var result = await _queryHandler.HandleAsync(query);

        if (result is null)
            return NotFound($"No snapshot found for form type '{formType}'.");

        return Ok(result);
    }

    /// <summary>
    /// Gets all stored snapshots as summaries.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllSnapshotsQuery();
        var results = await _queryHandler.HandleAsync(query);
        return Ok(results);
    }

    /// <summary>
    /// Gets a specific snapshot by its ID.
    /// </summary>
    [HttpGet("{snapshotId}")]
    public async Task<IActionResult> GetById(string snapshotId)
    {
        var query = new GetSnapshotByIdQuery { SnapshotId = snapshotId };
        var result = await _queryHandler.HandleAsync(query);

        if (result is null)
            return NotFound($"No snapshot found with ID '{snapshotId}'.");

        return Ok(result);
    }
}
