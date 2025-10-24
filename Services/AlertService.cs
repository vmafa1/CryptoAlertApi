using System.Net;
using System.Net.Mail;
using Microsoft.ApplicationInsights;
using CryptoAlertApi.Models;

public class AlertService
{
    private readonly TelemetryClient _telemetry;
    private readonly IConfiguration _config;

    public AlertService(TelemetryClient telemetry, IConfiguration config)
    {
        _telemetry = telemetry;
        _config = config;
    }

    public async Task SendEmailAlertAsync(AlertRequest request)
    {
        var gmailPassword = _config["GMAIL_PASSWORD"];

        _telemetry.TrackEvent("GmailPasswordDiagnostics", new Dictionary<string, string>
        {
            { "LoadedFromConfig", string.IsNullOrEmpty(gmailPassword) ? "No" : "Yes" }
        });

        if (string.IsNullOrEmpty(gmailPassword))
        {
            throw new Exception("GMAIL_PASSWORD not found in configuration.");
        }

        // ✅ Combine email body with Telegram message if present
        var emailBody = request.Body;
        if (!string.IsNullOrEmpty(request.TelegramMessage))
        {
            emailBody += $"\n\nTelegram Message:\n{request.TelegramMessage}";
        }

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("eatestingexpert@gmail.com", gmailPassword),
            EnableSsl = true,
            Timeout = 10000
        };

        var mail = new MailMessage
        {
            From = new MailAddress("eatestingexpert@gmail.com", "Crypto Alerts"),
            Subject = request.Subject,
            Body = emailBody
        };

        mail.To.Add(request.ToEmail);

        try
        {
            await smtpClient.SendMailAsync(mail);

            _telemetry.TrackEvent("EmailAlertSent", new Dictionary<string, string>
            {
                { "To", request.ToEmail },
                { "Subject", request.Subject }
            });
        }
        catch (Exception ex)
        {
            _telemetry.TrackException(ex);
            throw new Exception($"Email sending failed: {ex.Message}");
        }
    }
}