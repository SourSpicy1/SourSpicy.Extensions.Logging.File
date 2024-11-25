using Microsoft.Extensions.Logging;

namespace SourSpicy.Extensions.Logging.File;

public sealed class FileLoggerOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public string Path { get; set; } = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
}