﻿using Microsoft.Extensions.Logging;

namespace MovieDbAssistant.Lib.Components.Logger;

/// <summary>
/// app logger configuration
/// </summary>
public sealed class AppLoggerConfiguration
{
    /// <summary>
    /// is enabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// if true dump stack traces
    /// </summary>
    public bool DumpStackTraces { get; set; } = true;

    static readonly Dictionary<LogLevel, string> _logLevelsTexts =
        new()
        {
            { LogLevel.Debug , "" },
            { LogLevel.Warning , "" },
            { LogLevel.Error , "" },
            { LogLevel.Trace , "" },
            { LogLevel.Information , "" }
        };

    /// <summary>
    /// Get message level preamble text.
    /// </summary>
    /// <param name="level">The level.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string GetMessageLevelPreambleText(LogLevel level)
    {
        if (_logLevelsTexts.TryGetValue(level, out var text))
            return text;
        return string.Empty;
    }
}
