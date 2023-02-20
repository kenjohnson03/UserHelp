using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Appy.Classes
{
    public class PowerShellCmd
    {
        private StringBuilder outputString;
        public delegate void CommandComplete(string output);
        public CommandComplete UpdateCommandComplete;
        private System.Windows.Controls.TextBox _cmdOutput;
        private Dispatcher _dispatcher;

        public string Output
        {
            get
            {
                return outputString.ToString();
            }
        }

        public PowerShellCmd()
        {
            outputString = new StringBuilder();
        }

        public string Run(string Arguments, System.Windows.Controls.TextBox cmdOutput, Dispatcher dispatcher)
        {
            _cmdOutput = cmdOutput;
            _dispatcher = dispatcher;
            string title = System.Diagnostics.Process.GetCurrentProcess().MainWindowTitle;

            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;

                    myProcess.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                    myProcess.StartInfo.Arguments = Arguments;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.RedirectStandardOutput = true;

                    myProcess.OutputDataReceived += PowerShellOutputHandler;

                    myProcess.StartInfo.RedirectStandardInput = true;


                    myProcess.Start();
                    myProcess.BeginOutputReadLine();
                    myProcess.WaitForExit();
                }

                // write to the application log
                EventLog.WriteEntry("Application", $"{title}:\nPowerShell command run.\n{Arguments}", EventLogEntryType.Information, 1001);
            }
            catch (Exception ex)
            {
                DispatcherOperation op = _dispatcher.BeginInvoke((Action)(() => {
                    _cmdOutput.Text += $"{ex.Message}" + Environment.NewLine;
                }));
#if DEBUG
                Console.WriteLine("EXE: " + ex.Message);
#endif
                outputString.AppendLine(ex.Message);
                // Get the main window title
                
                EventLog.WriteEntry("Application", $"{title}:\nPowerShell command run.\n\npowershell.exe {Arguments}\n\nError:\n{ex.Message}\n{ex.InnerException}", EventLogEntryType.Error,1002);
            }
            return outputString.ToString();
        }

        private void PowerShellOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                outputString.AppendLine($"{outLine.Data}");
                DispatcherOperation op = _dispatcher.BeginInvoke((Action)(() => {
                    _cmdOutput.Text += $"{outLine.Data}" + Environment.NewLine;
                }));
#if DEBUG
                Console.WriteLine($"{outLine.Data}");
#endif
            }            
        }
    }
}
