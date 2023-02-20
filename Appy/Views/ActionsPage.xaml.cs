using Appy.Classes;
using Appy.Data;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private double CommandOutputHeight;
        private StringBuilder CommandOutputText;
        public String Id;
        private bool navigate = true;

        private ActionPageViewModel dataContext;

        public ActionsPage(ObservableCollection<UserAction> Actions, String Id, ref Frame MyNavigator, ref StringBuilder sb)
        {
            this._MyNavigator = MyNavigator;
            InitializeComponent();
            UserActions = Actions;
            bool first = true;
            bool selected = true;
            runCommand = "";
            CommandOutputHeight = 250;
            CommandOutputText = sb;
            this.Id = Id;

            dataContext = new ActionPageViewModel();
            dataContext.EnableRun();

            this.DataContext = dataContext;

            TextBox search = new TextBox();
            search.Name = "Search";
            search.Height = 30;
            search.Text = "Search";
            // add placeholder text to Textbox
            search.GotFocus += (sender, e) =>
            {
                if (search.Text == "Search")
                {
                    search.Text = "";
                    search.Foreground = Brushes.Black;
                }
            };
            search.LostFocus += (sender, e) =>
            {
                if (search.Text == "")
                {
                    search.Text = "Search";
                    search.Foreground = Brushes.Gray;
                }
            };
            // add search functionality
            search.TextChanged += (sender, e) =>
            {
                if (search.Text != "Search")
                {
                    for (int i = 1; i < stackyTheStackPanel.Children.Count; i++)
                    {
                        if (stackyTheStackPanel.Children[i].GetType() == typeof(ActionControl))
                        {
                            ActionControl ac2 = (ActionControl)stackyTheStackPanel.Children[i];
                            if (ac2.Text.ToLower().Contains(search.Text.ToLower()))
                            {
                                ac2.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                ac2.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < stackyTheStackPanel.Children.Count; i++)
                    {
                        if (stackyTheStackPanel.Children[i].GetType() == typeof(ActionControl))
                        {
                            ActionControl ac2 = (ActionControl)stackyTheStackPanel.Children[i];
                            ac2.Visibility = Visibility.Visible;
                        }
                    }
                }
            };
            // Center text in the search box
            search.VerticalContentAlignment = VerticalAlignment.Center;
            // set the search box text size to 14px
            search.FontSize = 14;
            // set search box margin 3px
            search.Margin = new Thickness(5,0,5,8);
            search.Padding = new Thickness(8, 0, 0, 0);

            stackyTheStackPanel.Children.Add(search);

            foreach (UserAction item in UserActions)
            {
                if (first)
                {
                    selected = true;
                    first = false;
                    ActionTitle.Text = item.Title;
                    ActionDescription.Text = item.Description;
                    runCommand = item.CommandArguments;
                }
                else
                {
                    selected = false;
                }

                ActionControl ac = new ActionControl
                {
                    UpdateTheParent = UpdateActionControls,
                    Text = item.Title,
                    Color = Brushes.Azure.Color,
                    Selected = selected,
                    HoverColor = hoverColor,
                    SelectedColor = Brushes.White.Color,
                    Id = item.Id
                };
                stackyTheStackPanel.Children.Add(ac);
                            
            }
            InitializeCommandOutput();
            this.SizeChanged += ActionsPage_SizeChanged;
        }


        private void ActionsPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = CommandOutputGrid.ActualWidth;
            ActionTitle.Width = width * .7;
            ActionDescription.Width = width * .7;

        }

        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(navigate)
            {
                _MyNavigator.NavigationService.GoBack();
            }            
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
                if (stackyTheStackPanel.Children[i].GetType() == typeof(ActionControl))
                {
                    ActionControl ac2 = (ActionControl)stackyTheStackPanel.Children[i];
                    if (ac2.Id == ac.Id)
                    {
                        ac2.Selected = true;
                        ActionTitle.Text = ac2.Text;
                        ActionDescription.Text = UserActions.Where(x => x.Id == ac2.Id).FirstOrDefault().Description;
                        runCommand = UserActions.Where(x => x.Id == ac2.Id).FirstOrDefault().CommandArguments;
                    }
                    else
                    {
                        ac2.Selected = false;
                    }
                }
            }
        }

        private async void Run_ClickAsync(object sender, RoutedEventArgs e)
        {
            Classes.PowerShellCmd cmd = new PowerShellCmd();

            dataContext.DisableRun();
            navigate = false;
            
            
            await Task.Run(() => cmd.Run(runCommand, CommandOutputTextBlock, this.Dispatcher));
            if (ConsoleOutputRowDefinition.Height.Value == 0)
            {
                ShowOutput_Click(sender, e);
            }

            dataContext.EnableRun();
            navigate = true;
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
                ShowCommandOutput();
            }
            else
            {
                CommandOutputHeight = ConsoleOutputRowDefinition.Height.Value;
                HideCommandOutput();
            }           
        }

        private void InitializeCommandOutput()
        {
            CommandOutputTextBlock.Text = CommandOutputText.ToString();
            CommandOutputScrollViewer.ScrollToBottom();
        }

        public void ProcessCommandOutput(string output)
        {
            if (!String.IsNullOrEmpty(output))
            {
                foreach(string line in output.Split(
                    new string[] { Environment.NewLine },
                    StringSplitOptions.None))
                {
                    if(!String.IsNullOrEmpty(line))
                    {                        
                        CommandOutputText.AppendLine(line);     
                    }                    
                }                
            }
            CommandOutputTextBlock.Text = CommandOutputText.ToString();
            CommandOutputScrollViewer.ScrollToBottom();
            
            dataContext.EnableRun();
        }


        private void ShowCommandOutput()
        {
            GridLengthConverter converter = new GridLengthConverter();

            ConsoleOutputRowDefinition.Height = (GridLength)converter.ConvertFromString(CommandOutputHeight.ToString());
              
        }
        private void HideCommandOutput()
        {
            GridLengthConverter converter = new GridLengthConverter();

            ConsoleOutputRowDefinition.Height = (GridLength)converter.ConvertFromString("0");
        }

        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Background = new SolidColorBrush(hoverColor);
        }

        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Background = Brushes.Transparent;
        }

    }
}
