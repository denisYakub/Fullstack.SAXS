using System.Diagnostics;
using Fullstack.SAXS.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fullstack.SAXS.Application
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
                try
                {
                    if (!_process.HasExited)
                        _process.Kill();
                }
                catch { }
            }))
            {
                await _process.WaitForExitAsync(stoppingToken);
            }

            logger.LogInformation("Flask server exited.");
        }

        public override void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_process != null && !_process.HasExited)
                    _process.Kill();
            }
            catch { }

            _process?.Dispose();
            _disposed = true;

            base.Dispose();
            logger.LogInformation("Flask server stopped.");
        }
    }
    /*public class PythonProcessHostedService : IHostedService
    {
        private readonly string _scriptPath;
        private Process? _process;

        public PythonProcessHostedService(string scriptPath)
        {
            _scriptPath = scriptPath;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{_scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = start };
            _process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            _process.ErrorDataReceived += (s, e) => Console.Error.WriteLine(e.Data);

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            Console.WriteLine("Flask server started.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill(true);
                    Console.WriteLine("Flask server stopped.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error stopping Flask server: " + ex.Message);
            }

            return Task.CompletedTask;
        }
    }*/
}
