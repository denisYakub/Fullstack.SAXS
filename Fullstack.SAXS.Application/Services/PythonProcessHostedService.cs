using System.Diagnostics;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fullstack.SAXS.Application.Services
{
    public class PythonProcessHostedService : BackgroundService
    {
        private readonly IStringService _scriptPath;
        private readonly ILogger<PythonProcessHostedService> _logger;
        private Process _process;
        private bool _disposed;

        public PythonProcessHostedService(
            IStringService scriptPath,
            ILogger<PythonProcessHostedService> logger)
        {
            _scriptPath = scriptPath;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{_scriptPath.GetPythonServerFilePath()}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = start };
            _process.OutputDataReceived += (s, e) => { if (e.Data != null) _logger.LogInformation(e.Data); };
            _process.ErrorDataReceived += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data)) return;

                if (e.Data.Contains("DeprecationWarning") || e.Data.Contains("UserWarning"))
                    _logger.LogWarning(e.Data);
                else
                    _logger.LogError(e.Data);
            };

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _logger.LogInformation("Flask server started.");

            using (stoppingToken.Register(() =>
            {
                if (!_process.HasExited)
                    _process.Kill(entireProcessTree: true);
            }))
            {
                try
                {
                    await _process.WaitForExitAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Flask server stopping...");
                }
            }

            _logger.LogInformation("Flask server exited.");
        }

        public override void Dispose()
        {
            if (_disposed) return;

            if (_process != null && !_process.HasExited)
                _process.Kill(entireProcessTree: true);

            _process?.Dispose();
            _disposed = true;

            base.Dispose();
            _logger.LogInformation("Flask server disposed.");
        }
    }

}
