using Appy.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Appy.Views
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private List<Appy.Classes.Category> Categories;
        private Frame _MyNavigator;
        private StringBuilder _ConsoleOutput;
        private List<ActionsPage> ActionsPages;

        public CategoriesPage()
        {
            InitializeComponent();
        }

        public CategoriesPage(ref List<Appy.Classes.Category> categories, ref Frame MyNavigator, ref StringBuilder sb)
        {
            InitializeComponent();
            Categories = categories;
            _MyNavigator = MyNavigator;            
            _ConsoleOutput = sb;
            ActionsPages = new List<ActionsPage>();
            SetupMainLayout2();
        }

        private void SetupMainLayout2()
        {
            foreach (var group in Categories)
            {
                UserHelpButton b = new UserHelpButton(group.Id)
                {
                    Text = group.Name,
                    Description = group.Description
                };
                b.MouseUp += new MouseButtonEventHandler(this.IconBlock_MouseUp2);
                p.Children.Add(b);

                // Create each of the ActionsPage
                ActionsPages.Add(new ActionsPage(group.UserActions, group.Id, ref _MyNavigator, ref _ConsoleOutput));
            }
        }

        private void IconBlock_MouseUp2(object sender, MouseButtonEventArgs e)
        {
            UserHelpButton b = sender as UserHelpButton;
            //Appy.Classes.Category g = Categories.Where(x => x.Id == b.Id).FirstOrDefault<Appy.Classes.Category>();
            _MyNavigator.Navigate(ActionsPages.Where(p => p.Id == b.Id).FirstOrDefault());
        }
    }
}
