using Microsoft.AspNetCore.Mvc;
using ZoomMeetings;
using ZoomMeetings.Models;

namespace AspNetCoreMvcExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeetingsController : ControllerBase
{
    private readonly ZoomClient _client;

    public MeetingsController(ZoomClient client)
    {
        _client = client;
    }

    /// <summary>GET /api/meetings - list the signed-in user's upcoming meetings.</summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var meetings = await _client.ListUpcomingMeetingsAsync("me");
        return Ok(meetings);
    }

    /// <summary>POST /api/meetings - create a meeting.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMeetingRequest request)
    {
        var meeting = await _client.CreateMeetingAsync("me", request);
        return CreatedAtAction(nameof(List), meeting);
    }

    /// <summary>DELETE /api/meetings/{meetingId} - cancel a meeting.</summary>
    [HttpDelete("{meetingId}")]
    public async Task<IActionResult> Delete(string meetingId)
    {
        await _client.DeleteMeetingAsync(meetingId);
        return NoContent();
    }
}
