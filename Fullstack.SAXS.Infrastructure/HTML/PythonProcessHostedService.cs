using System.Diagnostics;
using Fullstack.SAXS.Infrastructure.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Infrastructure.HTML
{
    public class PythonProcessHostedService : BackgroundService
    {
        private readonly ILogger<PythonProcessHostedService> _logger;
        private readonly Process _process;
        private bool _disposed;

        public PythonProcessHostedService(
            IOptions<GraphOptions> options,
            ILogger<PythonProcessHostedService> logger)
        {
            _logger = logger;

            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{options.Value.RunFilePath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = start };
            _process.OutputDataReceived += (s, e) => 
            { 
                if (e.Data != null)
                    if (e.Data.ToLower().Contains("warning"))
                        LogMessage(_logger, LogLevel.Warning, e.Data);
                    else
                        LogMessage(_logger, LogLevel.Information, e.Data); 
            };
            _process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                    LogMessage(_logger, LogLevel.Error, e.Data);
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            LogMessage(_logger, LogLevel.Information, "Python server started.");

            using (stoppingToken.Register(() =>
            {
                if (!_process.HasExited)
                    _process.Kill(entireProcessTree: true);
            }))
            {
                try
                {
                    await _process
                        .WaitForExitAsync(stoppingToken)
                        .ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    LogMessage(_logger, LogLevel.Information, "Python server stopping...");
                }
            }

            LogMessage(_logger, LogLevel.Information, "Python server exited.");
        }

        public override void Dispose()
        {
            if (_disposed) return;

            if (_process != null && !_process.HasExited)
                _process.Kill(entireProcessTree: true);

            _process?.Dispose();
            _disposed = true;

            base.Dispose();
            GC.SuppressFinalize(this);
            LogMessage(_logger, LogLevel.Information, "Python server disposed.");
        }

        private static void LogMessage(ILogger logger, LogLevel level, string message)
        {
            var eventId = level switch
            {
                LogLevel.Information => new EventId(1001, "InfoMessage"),
                LogLevel.Warning => new EventId(1002, "WarningMessage"),
                LogLevel.Error => new EventId(1003, "ErrorMessage"),
                _ => new EventId(1000, "GenericMessage")
            };

            logger.Log(level, eventId, message);
        }
    }
}
