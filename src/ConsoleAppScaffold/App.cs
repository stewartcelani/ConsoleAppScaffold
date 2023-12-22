using Microsoft.Extensions.Logging;

namespace ConsoleAppScaffold;

public class App
{
    private readonly EmailClient _emailClient;
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger, EmailClient emailClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
    }

    // Console app entrypoint with dependency injection and logging configured
    public async Task RunAsync()
    {
        _logger.LogInformation("========== Running ==========");

        await Task.Delay(5000); // Simulate a 5 second delay

        _logger.LogInformation("Sending email...");

        await _emailClient.SendEmailAsync(["stewart@laiswitchboards.com.au", "me@stewartcelani.com"], "Test Email",
            "This is a test email from the ConsoleAppScaffold.");

        _logger.LogInformation("========== Complete ==========");
    }
}