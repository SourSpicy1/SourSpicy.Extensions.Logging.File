using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SourSpicy.Extensions.Logging.File;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("File")]
public sealed class FileLoggerProvider : ILoggerProvider
{
    FileLoggerOptions _options;
    readonly IDisposable? _token;
    readonly ConcurrentDictionary<string, FileLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue;
        _token = optionsMonitor.OnChange(fileLoggerOptions => _options = fileLoggerOptions);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new(name, _options));

    public void Dispose()
    {
        _loggers.Clear();
        _token?.Dispose();
    }
}