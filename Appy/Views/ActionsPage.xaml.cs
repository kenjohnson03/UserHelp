using Appy.Classes;
using Appy.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Appy.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ActionsPage : Page
    {
        public Frame _MyNavigator { get; set; }
        ObservableCollection<UserAction> UserActions { get; set; }
        Color selectedColor = Color.FromArgb(Convert.ToByte(.2), Convert.ToByte(210), Convert.ToByte(210), Convert.ToByte(210));
        Color hoverColor = Color.FromArgb(Convert.ToByte(100), Convert.ToByte(190), Convert.ToByte(190), Convert.ToByte(190));
        string runCommand;
        private string PowerShellTextBlockText;
        private double CommandOutputHeight;
        private StringBuilder CommandOutputText;


        private ActionPageViewModel dataContext;

        public ActionsPage(ObservableCollection<UserAction> Actions, ref Frame MyNavigator)
        {
            this._MyNavigator = MyNavigator;
            InitializeComponent();
            UserActions = Actions;
            bool first = true;
            bool selected = true;
            runCommand = "";
            CommandOutputHeight = 250.0;
            CommandOutputText = new StringBuilder();


            dataContext = new ActionPageViewModel();
            dataContext.EnableRun();

            this.DataContext = dataContext;


            foreach (UserAction item in UserActions)
            {
                if (first)
                {
                    selected = true;
                    first = false;
                    Title.Text = item.Title;
                    Description.Text = item.Description;
                    runCommand = item.CommandArguments;
                }
                else
                {
                    selected = false;
                }
                ActionControl ac = new ActionControl
                {

                    //ActionControl ac = new ActionControl(item.Title, Brushes.Azure.Color, false, hoverColor, Brushes.White.Color);
                    UpdateTheParent = UpdateActionControls,
                    Text = item.Title,
                    Color = Brushes.Azure.Color,
                    Selected = selected,
                    HoverColor = hoverColor,
                    SelectedColor = Brushes.White.Color,
                    Id = item.Id
                };
                stackyTheStackPanel.Children.Add(ac);
                //stackyTheStackPanel.Children.Add(new Rectangle() { Height = 2, HorizontalAlignment = HorizontalAlignment.Stretch, Fill = Brushes.DarkGray });                
            }
        }


        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _MyNavigator.NavigationService.GoBack();
        }

        private void StackyTheStackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Stacky clicked");
#endif
        }

        public void UpdateActionControls(ActionControl ac)
        {
#if DEBUG
            Console.WriteLine("Delegate called by " + ac.Id);
#endif
            for (int i = 1; i < stackyTheStackPanel.Children.Count; i++)
            {
                if ((stackyTheStackPanel.Children[i] as ActionControl).Id != ac.Id)
                {
                    (stackyTheStackPanel.Children[i] as ActionControl).Selected = false;
                }
                else
                {
                    UserAction ua = UserActions.Where(u => u.Id == ac.Id).FirstOrDefault();
                    Title.Text = ua.Title;
                    Description.Text = ua.Description;
                    runCommand = ua.CommandArguments;
                    (stackyTheStackPanel.Children[i] as ActionControl).Selected = true;
                }
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Classes.PowerShellCmd cmd = new PowerShellCmd();

            dataContext.DisableRun();
            
            cmd.UpdateCommandComplete += ProcessCommandOutput;
            cmd.Run(runCommand);
            
            this.Run.IsEnabled = true;
            
        }

        private async void Run_ClickAsync(object sender, RoutedEventArgs e)
        {
            Classes.PowerShellCmd cmd = new PowerShellCmd();

            dataContext.DisableRun();

            ProcessCommandOutput(await Task.Run(() => cmd.Run(runCommand)));
            if (ConsoleOutputRowDefinition.Height.Value == 0)
            {
                ShowOutput_Click(sender, e);
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CommandOutputText.ToString());
        }

        private void ShowOutput_Click(object sender, RoutedEventArgs e)
        { 
            GridLengthConverter converter = new GridLengthConverter();
            if (ConsoleOutputRowDefinition.Height.Value == 0)
            {                
                ConsoleOutputRowDefinition.Height = (GridLength)converter.ConvertFromString(CommandOutputHeight.ToString());
            }
            else
            {
                CommandOutputHeight = ConsoleOutputRowDefinition.Height.Value;
                ConsoleOutputRowDefinition.Height = (GridLength)converter.ConvertFromString("0");
            }           
        }

        public void ProcessCommandOutput(string output)
        {
            if (output != null)
            {
                foreach(string line in output.Split(
                    new string[] { Environment.NewLine },
                    StringSplitOptions.None))
                {
                    if(!String.IsNullOrEmpty(line))
                    {
                        CommandOutputText.AppendLine(line);
                        dataContext.AddLine(line);
                    }                    
                }                
            }
            dataContext.EnableRun();
        }
    }
}
