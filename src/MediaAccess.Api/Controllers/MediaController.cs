using MediaAccess.Api.Contracts;
using MediaAccess.Application.Ports.Inbound;
using MediaAccess.Application.Ports.Inbound.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MediaAccess.Api.Controllers;

[ApiController]
[Route("internal/v0/media")]
public class MediaController : ControllerBase
{
  private readonly IRequestImageAccess _requestImageAccess;

  public MediaController(IRequestImageAccess requestImageAccess) => _requestImageAccess = requestImageAccess;

  /// <summary>Internal endpoint that generates presigned GET URLs for existing media objects.</summary>
  [HttpPost("get-url")]
  public async Task<IActionResult> GetPresignedUrl([FromBody] GetUrlRequest req, CancellationToken ct)
  {
    var result = await _requestImageAccess.ExecuteAsync(
      new RequestImageAccessCommand(req.ObjectKey), ct);

    return result switch
    {
      RequestImageAccessResult.Success s => Ok(new GetUrlResponse(s.Url, s.ExpiresAt)),
      RequestImageAccessResult.Invalid i => BadRequest(new { errors = i.Errors }),
      RequestImageAccessResult.NotFound nf => NotFound(new { nf.ObjectKey }),
      _ => StatusCode(500)
    };
  }
}
