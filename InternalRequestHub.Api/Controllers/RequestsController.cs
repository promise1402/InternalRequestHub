using InternalRequestHub.Api.Data;
using InternalRequestHub.Api.DTOs.Requests;
using InternalRequestHub.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternalRequestHub.Api.Controllers;

[ApiController]
[Route("api/requests")]
public sealed class RequestsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public RequestsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Request), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Request>> Create(
        [FromBody] CreateRequestDto dto,
        CancellationToken cancellationToken)
    {
        var request = new Request
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Requests.Add(request);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/requests/{request.Id}", request);
    }
}
