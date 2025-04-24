using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.Bootstrap;
using MovieDbAssistant.Lib.Components.DependencyInjection;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;

#pragma warning disable IDE0130 // Le namespace ne correspond pas à la structure de dossiers

namespace MovieDbAssistant;

/// <summary>
/// The program.
/// </summary>
public class Program
{
    const string AppId = "5781ECE5-AA4A-4653-A02E-D118FF1C1A2F";
    const string AppDataFolder = "MovieDbAssistant";
    const string PackageFolder = "package";

    /// <summary>
    /// ok exit code
    /// </summary>
    public const int EXIT_OK = 0;

    /// <summary>
    /// instance already running exit code
    /// </summary>
    public const int EXIT_INSTANCE_ALLREADY_RUNNING = 1;

    /// <summary>
    /// main
    /// </summary>
    /// <param name="args">The args.</param>
    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        using var mutex = new Mutex(false, AppId);

        if (!mutex.WaitOne(0))
        {
            Console.WriteLine("already running");
            return EXIT_INSTANCE_ALLREADY_RUNNING;
        }

        // setup env
        Env.Init(AppDataFolder);

        // setup base directory
        // msix: C:\Program Files\WindowsApps\FranckGaspoz.Software.MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w
        // inno setup: installation folder
        // dev: /bin/...

        // fix base path
        var basePath = System.AppContext.BaseDirectory;
        Env.InitialBaseDirectory = basePath;
        if (basePath.Contains(PackageFolder))
            basePath = basePath.Replace(PackageFolder, "");
        Directory.SetCurrentDirectory(basePath);

        Type[] fromTypes =
            [typeof(AppLogger),
            typeof(DmnSettings),
            typeof(AppSettings)];

        var host = Host.CreateDefaultBuilder(args)
            .AddEnvironmentSettings()
            .AddLocalizedSettings()
            .ConfigureServices((context, services) => services
                .AutoRegister(fromTypes)
                .AddSignalR(fromTypes)
                .Configure<DmnSettings>(context.Configuration)
                .Configure<AppSettings>(context.Configuration))
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddAppLogger();
            })
            .Build();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);

        Application.Run(
            host.Services
                .GetRequiredService<TrayApplication>());

        await host.RunAsync();
        return EXIT_OK;
    }

}
