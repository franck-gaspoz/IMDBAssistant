using System.Runtime.InteropServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray application.
/// </summary>
[Singleton]
public sealed partial class TrayApplication : ApplicationContext
{
    /// <inheritdoc/>
    public TrayApplication(
        IServiceProvider serviceProvider,
        IOptions<DmnSettings> dmnSettings,
        ILogger<TrayApplication> logger)
    {
        LogProps(dmnSettings, logger);

        serviceProvider
            .GetRequiredService<TrayMenuBuilder>()
            .Build();

        serviceProvider
            .GetRequiredService<TrayMenuService>()
            .ShowBalloonTip_Start();
    }

    void LogProps(IOptions<DmnSettings> dmnSettings, ILogger<TrayApplication> logger)
    {
        void O(string s) => logger.LogInformation(this, s);
        void Sep() => O("".PadLeft(80, '-'));

        Sep();
        O(dmnSettings.Value.App.Title);
        O("logging to file: " + AppLogger.GetLogFilePath());
        Sep();

        O("- environment");
        Sep();

        var vars = Environment.GetEnvironmentVariables().Keys;
        foreach (var key in vars)
            O(key + ": " + Environment.GetEnvironmentVariable(
                key.ToString()!));

        Sep();
        O("- paths");
        Sep();

        foreach (var p in Enum.GetValues<Environment.SpecialFolder>())
            O(p + ": " + Environment.GetFolderPath(p));

        Sep();
        O("- properties");
        Sep();

        O("IsWindowsAppRunningMode: " + Env.IsWindowsAppRunningMode);
        O("InitialBaseDirectory: " + Env.InitialBaseDirectory);
        O("BaseDirectory: " + AppContext.BaseDirectory);
        O("TargetFrameworkName: " + AppContext.TargetFrameworkName);
        O("FrameworkDescription: " + RuntimeInformation.FrameworkDescription);
        O("OSArchitecture: " + RuntimeInformation.OSArchitecture);
        O("OSDescription: " + RuntimeInformation.OSDescription);
        O("ProcessArchitecture: " + RuntimeInformation.ProcessArchitecture);
        O("RuntimeIdentifier: " + RuntimeInformation.RuntimeIdentifier);
        Sep();
    }
}
