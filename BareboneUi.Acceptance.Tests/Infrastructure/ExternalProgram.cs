using System;
using System.Diagnostics;
using System.IO;

namespace BareboneUi.Acceptance.Tests.Infrastructure
{
    public class ExternalProgram : IDisposable
    {
        private Process _process;

        public ExternalProgram(string exePath, string args)
        {
            _process = new Process
            {
                StartInfo =
                {
                    FileName = exePath,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _process.OutputDataReceived += (sender, e) => WriteToTestConsole(e);
            _process.ErrorDataReceived += (sender, e) => WriteToTestConsole(e);
        }

        private static void WriteToTestConsole(DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data)) return;
            Console.WriteLine($"[WEB-SERVER]==> {e.Data}");
        }

        public int ExitCode => _process.ExitCode;

        public void Start()
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Kill();
                _process?.Dispose();
                _process = null;
            }
        }

        public void Kill()
        {
            if (_process == null || _process.HasExited)
            {
                var processName = Path.GetFileName(_process?.StartInfo.FileName ?? "Unknown");
                Console.WriteLine($"*** This test failed because the process [{processName}] exited unexpectedly early ***");
            }
            else
            {
                KillProcessTree();
            }
        }

        private void KillProcessTree()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo("taskkill", $"/F /T /PID {_process.Id}")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                Process.Start(processStartInfo);
            }
            catch { }
        }
    }
}
