using Appy.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserHelp.UIElements;

namespace Appy.Views
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private List<Appy.Classes.Category> Categories;
        private Frame _MyNavigator;

        public CategoriesPage()
        {
            InitializeComponent();
        }

        public CategoriesPage(ref List<Appy.Classes.Category> categories, ref Frame MyNavigator)
        {
            InitializeComponent();
            Categories = categories;
            _MyNavigator = MyNavigator;
            //SetupMainLayout();
            SetupMainLayout2();

        }

        private void SetupMainLayout2()
        {
            WrapPanel p = new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            CategoryGrid.Children.Add(p);
            foreach (var group in Categories)
            {
                UserHelpButton b = new UserHelpButton(group.Id)
                {
                    Text = group.Name,
                    Description = group.Description
                };
                b.MouseUp += new MouseButtonEventHandler(this.IconBlock_MouseUp2);
                p.Children.Add(b);
            }
        }

        private void IconBlock_MouseUp2(object sender, MouseButtonEventArgs e)
        {
            UserHelpButton b = sender as UserHelpButton;
            Console.WriteLine("Click Up called {0} - {1}", sender.GetType(), b.Name);
            Appy.Classes.Category g = Categories.Where(x => x.Id == b.Id).FirstOrDefault<Appy.Classes.Category>();
            _MyNavigator.Navigate(new ActionsPage(g.UserActions, ref _MyNavigator));
        }
    }
}
