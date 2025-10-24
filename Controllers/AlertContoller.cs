using CryptoAlertApi.Models;
using CryptoAlertApi.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly AlertService _alertService;
    private readonly IConfiguration _config;
    private readonly TelemetryClient _telemetry;

    public AlertsController(AlertService alertService, IConfiguration config, TelemetryClient telemetry)
    {
        _alertService = alertService;
        _config = config;
        _telemetry = telemetry;
    }

    // ✅ POST /api/alerts
    [HttpPost]
    public async Task<IActionResult> SendAlert([FromBody] AlertRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.ToEmail))
        {
            return BadRequest("Invalid alert request.");
        }

        try
        {
            await _alertService.SendEmailAlertAsync(request);
            return Ok("Alert sent successfully.");
        }
        catch (Exception ex)
        {
            _telemetry.TrackException(ex);
            return StatusCode(500, $"Error sending alerts: {ex.Message}");
        }
    }

    // ✅ GET /api/alerts/debug-config
    [HttpGet("debug-config")]
    public IActionResult DebugConfig()
    {
        var gmailPassword = _config["GMAIL_PASSWORD"];
        var loaded = string.IsNullOrEmpty(gmailPassword) ? "Missing" : "Loaded";

        _telemetry.TrackEvent("DebugConfigCheck", new Dictionary<string, string>
        {
            { "GMAIL_PASSWORD", loaded }
        });

        return Ok(new
        {
            GmailPassword = loaded
        });
    }
}