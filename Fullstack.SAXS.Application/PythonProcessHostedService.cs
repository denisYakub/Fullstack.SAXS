using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Fullstack.SAXS.Application
{
    public class PythonProcessHostedService : IHostedService
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
    }
}
