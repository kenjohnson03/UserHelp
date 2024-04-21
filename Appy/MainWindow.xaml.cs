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
            if (System.IO.File.Exists(userHelpFilePath))
            {
#if DEBUG
                Console.WriteLine("Path exists");
#endif
                Categories = Appy.Data.ParseCategoriesJSON.GetData(userHelpFilePath);

                string title = this.Title;
                EventLog.WriteEntry("Application", $"{title}:\nOpened {userHelpFilePath}", EventLogEntryType.Information, 1000);
                Appy.Views.CategoriesPage categoryPage = new Views.CategoriesPage(ref Categories, ref NavigationFrame, ref ConsoleOutputLines);
                NavigationFrame.Navigate(categoryPage);
            }
            else
            {
#if DEBUG
                Console.WriteLine("Path does not exist");
#endif
                string message = $"UserHelp.json was not found at {userHelpFilePath}";
                Appy.Views.ErrorPage errorPage = new Views.ErrorPage(message);
                NavigationFrame.Navigate(errorPage);
                EventLog.WriteEntry("Application", message, EventLogEntryType.Error, 5000);
            }
            
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
#if DEBUG
                Console.WriteLine(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
#endif

            }

        }        
    }
}
