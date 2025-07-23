using System.Diagnostics;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fullstack.SAXS.Application.Services
{
    public class PythonProcessHostedService : BackgroundService
    {
        private readonly IConnectionStrService _scriptPath;
        private readonly ILogger<PythonProcessHostedService> _logger;
        private Process _process;
        private bool _disposed;

        private static readonly Action<ILogger, string, Exception?> _logInfoMessage =
            LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1001, "InfoMessage"),
            "{Message}");

        private static readonly Action<ILogger, string, Exception?> _logWarningMessage =
            LoggerMessage.Define<string>(
                LogLevel.Warning,
                new EventId(1002, "WarningMessage"),
                "{Message}");

        private static readonly Action<ILogger, string, Exception?> _logErrorMessage =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1003, "ErrorMessage"),
                "{Message}");

        private static readonly Action<ILogger, Exception?> _logFlaskStarted =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1004, "FlaskStarted"),
                "Flask server started.");

        private static readonly Action<ILogger, Exception?> _logFlaskStopping =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1005, "FlaskStopping"),
                "Flask server stopping...");

        private static readonly Action<ILogger, Exception?> _logFlaskExited =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1006, "FlaskExited"),
                "Flask server exited.");

        private static readonly Action<ILogger, Exception?> _logFlaskDisposed =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1007, "FlaskDisposed"),
                "Flask server disposed.");

        public PythonProcessHostedService(
            IConnectionStrService scriptPath,
            ILogger<PythonProcessHostedService> logger)
        {
            _scriptPath = scriptPath;
            _logger = logger;

            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{_scriptPath.GetGraphServerFilePath()}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = start };
            _process.OutputDataReceived += (s, e) => { if (e.Data != null) _logInfoMessage(_logger, e.Data, null); };
            _process.ErrorDataReceived += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data)) return;

                if (e.Data.Contains("DeprecationWarning", StringComparison.Ordinal) || 
                    e.Data.Contains("UserWarning", StringComparison.Ordinal))
                    _logWarningMessage(_logger, e.Data, null);
                else
                    _logErrorMessage(_logger, e.Data, null);
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _logFlaskStarted(_logger, null);

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
                    _logFlaskStopping(_logger, null);
                }
            }

            _logFlaskExited(_logger, null);
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
            _logFlaskDisposed(_logger, null);
        }
    }

}
