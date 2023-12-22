using ConsoleAppScaffold;
using ConsoleAppScaffold.Helpers;
using ConsoleAppScaffold.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Serilog logging setup
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(context.Configuration)
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            SelfLog.Enable(Console.Error);
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, true);
        });

        // Smtp Settings -- Example of binding settings from appsettings.json
        var smtpSettings =
            SettingsBinder.BindAndValidate<SmtpSettings, SmtpSettingsValidator>(
                context.Configuration);
        services.AddSingleton(smtpSettings);
        
        // Add email client
        services.AddSingleton<EmailClient>();

        // Adding the core app entrypoint
        services.AddSingleton<App>();
    })
    .Build();

var app = host.Services.GetRequiredService<App>(); // Get the core app entrypoint from the DI container

await app.RunAsync(); // Run app

Log.CloseAndFlush(); // Serilog cleanup