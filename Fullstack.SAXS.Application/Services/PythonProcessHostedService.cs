using System.Diagnostics;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fullstack.SAXS.Application.Services
{
    public class PythonProcessHostedService(
        IStringService scriptPath, 
        ILogger<PythonProcessHostedService> logger
    ) : BackgroundService
    {
        private Process _process;
        private bool _disposed;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath.GetPythonServerFilePath()}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = start };

            _process.OutputDataReceived += (s, e) => { if (e.Data != null) logger.LogInformation(e.Data); };
            _process.ErrorDataReceived += (s, e) => { if (e.Data != null) logger.LogError(e.Data); };

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            logger.LogInformation("Flask server started.");

            using (stoppingToken.Register(() =>
            {
                if (!_process.HasExited)
                    _process.Kill();
            }))
            {
                await Task.WhenAny(
                    Task.Run(() => _process.WaitForExit()),
                    Task.Delay(Timeout.Infinite, stoppingToken)
                );
            }

            logger.LogInformation("Flask server exited.");
        }

        public override void Dispose()
        {
            if (_disposed) return;

            if (_process != null && !_process.HasExited)
                _process.Kill();

            _process?.Dispose();
            _disposed = true;

            base.Dispose();
            logger.LogInformation("Flask server stopped.");
        }
    }
}
