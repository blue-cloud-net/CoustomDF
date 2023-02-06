using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.Exceptions;

/// <summary>
/// Interface to define a <see cref="LogLevel"/> property (see <see cref="LogLevel"/>).
/// </summary>
public interface IHasLogLevel
{
    /// <summary>
    /// Log severity.
    /// </summary>
    LogLevel? LogLevel { get; set; }
}
