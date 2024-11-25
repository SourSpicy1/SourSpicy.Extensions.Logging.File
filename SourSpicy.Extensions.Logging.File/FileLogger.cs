using Microsoft.Extensions.Logging;

namespace SourSpicy.Extensions.Logging.File;

sealed class FileLogger(string name, FileLoggerOptions options) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
        default;

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel >= options.LogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var path = options.Path;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd}.txt");
        var message =
            $"{DateTime.Now:HH:mm:ss}\n" +
            $"{logLevel.ToString().Substring(0, 4)}: {name}[{eventId.Id}]\n" +
            $"\t  {formatter(state, exception)}\n" +
            $"\t  {exception}\n";

        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);
            using var file = new StreamWriter(path);
            file.WriteLine(message);
        }
        else
        {
            using var file = new StreamWriter(path, true);
            file.WriteLine(message);
        }
    }
}