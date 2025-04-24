using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MovieDbAssistant.Lib.Components.Bootstrap;

/// <summary>
/// The I host builder extensions.
/// </summary>
public static class IHostBuilderExtensions
{
    public const string Filename_Pattern_App_Settings_Development = "appsettings.Development.json";
    public const string Filename_Pattern_App_Settings_Culture = "culture-settings.{0}.json";
    public const string DefaultCulture = "EN-US";

    /// <summary>
    /// Add localized settings.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="IHostBuilder"/></returns>
    public static IHostBuilder AddLocalizedSettings(
        this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(conf =>
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name.ToUpper();
            var t = currentCulture.Split('-');
            var file1 = string.Format(Filename_Pattern_App_Settings_Culture, currentCulture);
            var file2 = string.Format(Filename_Pattern_App_Settings_Culture, t[0]);
            var file3 = string.Format(Filename_Pattern_App_Settings_Culture, DefaultCulture);
            if (File.Exists(file3)) conf.AddJsonFile(file3);
            if (File.Exists(file2)) conf.AddJsonFile(file2);
            if (File.Exists(file1)) conf.AddJsonFile(file1);
        });
        return builder;
    }

    /// <summary>
    /// onfigure app env settings
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="IHostBuilder"/></returns>
    public static IHostBuilder AddEnvironmentSettings(
        this IHostBuilder builder)
    {        
        builder.ConfigureAppConfiguration(conf =>
        {
#if DEBUG
            if (File.Exists(Filename_Pattern_App_Settings_Development))
                conf.AddJsonFile(Filename_Pattern_App_Settings_Development);
#endif
        });
        return builder;
    }
}
