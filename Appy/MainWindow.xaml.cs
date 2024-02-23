using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
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

            if (!System.IO.File.Exists(userHelpFilePath))
            {          
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(userHelpFilePath));
                File.Copy(System.IO.Path.Combine(WorkingDirectory, "UserHelp.json"), userHelpFilePath, true);

                /// Will not start if the user does not have write permissions to UserHelp.json in the ProgramData Directory.
                /// ParseCategoriesJSON.GetData will throw an unauthorized access exception.
                FileSecurity fSecurity = File.GetAccessControl(userHelpFilePath);
                fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.Write, AccessControlType.Allow));
                File.SetAccessControl(userHelpFilePath, fSecurity);
            }
            else
            {
                Console.WriteLine("UserHelp.json already exists");
            }

            Categories = Appy.Data.ParseCategoriesJSON.GetData(userHelpFilePath);
            
            string title = this.Title;            
            EventLog.WriteEntry("Application", $"{title}:\nOpened {userHelpFilePath}", EventLogEntryType.Information, 1000);
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
#if DEBUG
                Console.WriteLine(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
#endif

            }

        }        
    }
}
