using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Lib.Components.Logger;

#pragma warning disable CA2254 // Le modèle doit être une expression statique.

/// <summary>
/// extensions methods for <see cref="AppLogger"/>
/// </summary>
public static class AppLoggerExtensions
{
    const string Sep_CallerMessage = "@@";

    /// <summary>
    /// Add app logger.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="ILoggingBuilder"/></returns>
    public static ILoggingBuilder AddAppLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        AppLogger.ClearLogFile();       
        builder.Services
            .TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, AppLoggerProvider>());

        return builder;
    }

    /// <summary>
    /// Add app logger.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="ILoggingBuilder"/></returns>
    public static ILoggingBuilder AddAppLogger(
        this ILoggingBuilder builder,
        Action<AppLoggerConfiguration> configure)
    {
        builder.AddAppLogger();
        builder.Services.Configure(configure);

        return builder;
    }

    /// <summary>
    /// Log error with id.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogErrorWithId(this ILogger logger, object caller, string message)
        => logger.LogError(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Log the trace.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogTrace(this ILogger logger, object caller, string message)
        => logger.LogTrace(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Log the debug.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogDebug(this ILogger logger, object caller, string message)
        => logger.LogDebug(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Log the warning.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogWarning(this ILogger logger, object caller, string message)
        => logger.LogWarning(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Log the error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogError(this ILogger logger, object caller, string message)
        => logger.LogError(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Log the error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="exception">exception</param>
    /// <param name="message">The message.</param>
    public static void LogError(
        this ILogger logger,
        object caller,
        string message,
        Exception exception) =>
        //logger.LogError(message.CollapseCallerMessage(caller));
        logger.Log(
            LogLevel.Error,
            exception,
            message.CollapseCallerMessage(caller),
            []
           );

    /// <summary>
    /// Log the info.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caller">The caller.</param>
    /// <param name="message">The message.</param>
    public static void LogInformation(this ILogger logger, object caller, string message)
        => logger.LogInformation(message.CollapseCallerMessage(caller));

    /// <summary>
    /// Collapses caller message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="caller">The caller.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string CollapseCallerMessage(this string message, object caller)
        => caller.GetId() + Sep_CallerMessage + message;

    /// <summary>
    /// Extracts caller message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>A <see cref="(string Caller,string Message) "/></returns>
    public static (string Caller, string Message) ExtractCallerMessage(this string message)
    {
        var t = message.Split(Sep_CallerMessage);
        if (t.Length == 1)
            return (string.Empty, t[0]);
        return (t[0], t[1]);
    }

}
