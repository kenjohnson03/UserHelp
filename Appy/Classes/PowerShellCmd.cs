using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appy.Classes
{
    public class PowerShellCmd
    {
        private StringBuilder outputString;
        private int numOutputLines;
        public delegate void CommandComplete(string output);
        public CommandComplete UpdateCommandComplete;

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
            numOutputLines = 0;
        }

        public string Run(string Arguments)
        {
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
                    // This code assumes the process you are starting will terminate itself.
                    // Given that it is started without a window so you cannot terminate it
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                    myProcess.BeginOutputReadLine();
                    myProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("EXE: " + ex.Message);
#endif
                outputString.AppendLine(ex.Message);
            }
            if(UpdateCommandComplete != null)
            {
                UpdateCommandComplete(outputString.ToString());

            }
            return outputString.ToString();
        }

        private void PowerShellOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;

                // Add the text to the collected output.
                outputString.Append(Environment.NewLine +
                    $"{outLine.Data}");
#if DEBUG
                Console.WriteLine(Environment.NewLine +
                    $"{outLine.Data}");
#endif
            }            
        }
    }
}
