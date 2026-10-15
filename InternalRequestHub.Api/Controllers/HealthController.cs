using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternalRequestHub.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse("API is running"));
    }
}

public sealed record HealthResponse(string Status);
