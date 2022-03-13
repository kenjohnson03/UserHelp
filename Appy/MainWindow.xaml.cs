using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Appy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static StringBuilder sortOutput = null;
        private static int numOutputLines = 0;
        private StringBuilder ConsoleOutputLines;
        List<Classes.Category> Categories = new List<Classes.Category>();

        public MainWindow()
        {

            InitializeComponent();
            
            ConsoleOutputLines = new StringBuilder();
            string userHelpFilePath = Environment.GetEnvironmentVariable("ProgramData") + "\\UserHelp\\UserHelp.json";
            string WorkingDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine(userHelpFilePath);

            if (System.IO.File.Exists(userHelpFilePath))
            {
#if DEBUG
                Console.WriteLine("Path exists");
#endif
            }
            else
            {
#if DEBUG
                Console.WriteLine("Path does not exist");
#endif
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(userHelpFilePath));
                File.Copy(System.IO.Path.Combine(WorkingDirectory, "UserHelp.json"), userHelpFilePath, true);
            }
#if DEBUG
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(userHelpFilePath));
            File.Copy(System.IO.Path.Combine(WorkingDirectory, "UserHelp.json"), userHelpFilePath, true);
#endif
            Categories = Appy.Data.ParseCategoriesJSON.GetData(userHelpFilePath);

            Appy.Views.CategoriesPage categoryPage = new Views.CategoriesPage(ref Categories, ref NavigationFrame, ref ConsoleOutputLines);
            NavigationFrame.Navigate(categoryPage);
        }

        

        private void PowerShellOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;

                // Add the text to the collected output.
                sortOutput.Append(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");

                Console.WriteLine(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");


            }

        }

        private void Run_MouseUp(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Mouse up called run");

            try
            {
                using (Process myProcess = new Process())
                {
                    string command = "Get-Process | Ogv -passthru";
                    myProcess.StartInfo.UseShellExecute = false;
                    // You can start any process, HelloWorld is a do-nothing example.
                    myProcess.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                    myProcess.StartInfo.Arguments = "-Command \"& { " + command + " }\"";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.RedirectStandardOutput = true;
                    sortOutput = new StringBuilder();

                    // Set our event handler to asynchronously read the sort output.
                    myProcess.OutputDataReceived += PowerShellOutputHandler;

                    // Redirect standard input as well.  This stream
                    // is used synchronously.
                    myProcess.StartInfo.RedirectStandardInput = true;


                    myProcess.Start();
                    // This code assumes the process you are starting will terminate itself.
                    // Given that it is started without a window so you cannot terminate it
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                    myProcess.BeginOutputReadLine();
                    Console.WriteLine("Started Everything");
                    myProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXE: " + ex.Message);
            }
        }

        
    }
}
