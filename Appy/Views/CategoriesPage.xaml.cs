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
        private static int iconWidth = 175;
        private static int iconWidthShrink = 165;

        private static int iconHeight = 120;
        private static int iconHeightShrink = 110;

        Thickness iconMargin = new Thickness(15);
        Thickness iconMarginShrink = new Thickness(20);

        private static int iconTitleFontSize = 18;
        private static int iconTitleFontSizeShrink = 17;

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

        private void SetupMainLayout()
        {
            var rand = new Random();

            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255))));


            brush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255))));


            //StackPanel p = new StackPanel();
            WrapPanel p = new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            CategoryGrid.Children.Add(p);

            foreach (var group in Categories)
            {
                brush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255))));

                Grid grid = new Grid();
                

                TextBlock tb = new TextBlock()
                {
                    Text = group.Name,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = iconTitleFontSize,
                    FontWeight = FontWeights.Bold,
                    Name = "a" + group.Id.Replace(" ", "").Replace("-","") + "_textblock"
                };
                TextBlock desc = new TextBlock()
                {
                    Text = group.Description,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 15,
                    FontWeight = FontWeights.Normal
                };

                StackPanel s = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                s.Children.Add(tb);
                s.Children.Add(desc);

                GroupBorder b = new GroupBorder()
                {
                    Width = iconWidth,
                    Height = iconHeight,
                    Background = Brushes.Transparent,
                    Margin = iconMargin,
                    Child = s,
                    CornerRadius = new CornerRadius(12),
                    Name = "a" + group.Id.Replace(" ", "").Replace("-", "") + "_border",
                    Id = group.Id,
                    BorderThickness = new Thickness(2)

                };

                try
                {
                    this.RegisterName(tb.Name, tb);
                    this.RegisterName(b.Name, b);
                } 
                catch (System.ArgumentException ex)
                {
                    _MyNavigator.Navigate(new ErrorPage("Look at the names in your JSON\n"));
                }
                catch (Exception ex)
                {
                    _MyNavigator.Navigate(new ErrorPage("Look at the names in your JSON\n"));
                }
                    
                
                    
                
                
                b.MouseDown += new MouseButtonEventHandler(this.IconBlock_MouseDown);
                b.MouseUp += new MouseButtonEventHandler(this.IconBlock_MouseUp);
                b.MouseLeave += new MouseEventHandler(this.IconBlock_DragLeave);
                b.MouseEnter += new MouseEventHandler(this.IconBlock_MouseEnter);
                

                grid.Children.Add(b);
                
                p.Children.Add(grid);
            }
        }

        private void IconBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            GroupBorder b = sender as GroupBorder;
            b.BorderBrush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(100), Convert.ToByte(100), Convert.ToByte(100)));
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IconPressed(b);
            }
        }

        private void IconBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GroupBorder b = sender as GroupBorder;

            //DoubleAnimation myDoubleAnimation = new DoubleAnimation
            //{
            //    From = 1.0,
            //    To = 0.5,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationWidth = new DoubleAnimation
            //{
            //    From = 100,
            //    To = 90,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            //{
            //    From = 100,
            //    To = 90,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            //{
            //    From = iconTitleFontSize,
            //    To = iconTitleFontSizeShrink,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            //{
            //    From = iconMargin,
            //    To = iconMarginShrink,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //Storyboard aStoryboard = new Storyboard();
            //aStoryboard.Children.Add(myDoubleAnimation);
            //Storyboard.SetTargetName(myDoubleAnimation, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            //aStoryboard.Children.Add(myDoubleAnimationWidth);
            //Storyboard.SetTargetName(myDoubleAnimationWidth, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            //aStoryboard.Children.Add(myDoubleAnimationHeight);
            //Storyboard.SetTargetName(myDoubleAnimationHeight, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            //aStoryboard.Children.Add(myDoubleAnimationText);
            //Storyboard.SetTargetName(myDoubleAnimationText, (b.Name.Split('_').First() + "_textblock"));
            //Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            //aStoryboard.Children.Add(myThicknessAnimation);
            //Storyboard.SetTargetName(myThicknessAnimation, b.Name);
            //Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            //aStoryboard.Begin(this);

            IconPressed(b);
            Console.WriteLine("Click called Down {0} - {1}", sender.GetType(), b.Name);


        }

        private void IconBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GroupBorder b = sender as GroupBorder;

            //DoubleAnimation myDoubleAnimation = new DoubleAnimation
            //{
            //    From = 0.5,
            //    To = 1.0,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationWidth = new DoubleAnimation
            //{
            //    From = 90,
            //    To = iconWidth,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            //{
            //    From = 90,
            //    To = iconHeight,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            //{
            //    From = iconTitleFontSizeShrink,
            //    To = iconTitleFontSize,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //Thickness newMargin = new Thickness(15);
            //Thickness oldMargin = new Thickness(20);

            //ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            //{
            //    From = oldMargin,
            //    To = newMargin,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
            //    AutoReverse = false
            //};

            //Storyboard aStoryboard = new Storyboard();
            //aStoryboard.Children.Add(myDoubleAnimation);
            //Storyboard.SetTargetName(myDoubleAnimation, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            //aStoryboard.Children.Add(myDoubleAnimationWidth);
            //Storyboard.SetTargetName(myDoubleAnimationWidth, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            //aStoryboard.Children.Add(myDoubleAnimationHeight);
            //Storyboard.SetTargetName(myDoubleAnimationHeight, b.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            //aStoryboard.Children.Add(myDoubleAnimationText);
            //Storyboard.SetTargetName(myDoubleAnimationText, (b.Name.Split('_').First() + "_textblock"));
            //Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            //aStoryboard.Children.Add(myThicknessAnimation);
            //Storyboard.SetTargetName(myThicknessAnimation, b.Name);
            //Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            //aStoryboard.Begin(this);

            IconReleased(b);
            Console.WriteLine("Click Up called {0} - {1}", sender.GetType(), b.Name);
            Appy.Classes.Category g = Categories.Where(x => x.Id == b.Id).FirstOrDefault<Appy.Classes.Category>();
            _MyNavigator.Navigate(new ActionsPage(g.UserActions, ref _MyNavigator));
        }

        private void IconBlock_MouseLeave(object sender, MouseButtonEventArgs e)
        {
            GroupBorder b = sender as GroupBorder;
            IconBlock_DragLeave(sender, e);
        }

        private void IconBlock_DragLeave(object sender, MouseEventArgs e)
        {
            GroupBorder b = sender as GroupBorder;
            IconReleased(b);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                
                //DoubleAnimation myDoubleAnimation = new DoubleAnimation
                //{
                //    From = 1.0,
                //    To = 1.0,
                //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                //    AutoReverse = false
                //};

                //DoubleAnimation myDoubleAnimationWidth = new DoubleAnimation
                //{
                //    From = iconWidth,
                //    To = iconWidth,
                //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                //    AutoReverse = false
                //};

                //DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
                //{
                //    From = iconHeight,
                //    To = iconHeight,
                //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                //    AutoReverse = false
                //};

                //DoubleAnimation myDoubleAnimationText = new DoubleAnimation
                //{
                //    From = 15,
                //    To = 15,
                //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                //    AutoReverse = false
                //};

                //Thickness newMargin = new Thickness(15);
                //Thickness oldMargin = new Thickness(15);

                //ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
                //{
                //    From = oldMargin,
                //    To = newMargin,
                //    Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                //    AutoReverse = false
                //};

                //Storyboard aStoryboard = new Storyboard();
                //aStoryboard.Children.Add(myDoubleAnimation);
                //Storyboard.SetTargetName(myDoubleAnimation, b.Name);
                //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

                //aStoryboard.Children.Add(myDoubleAnimationWidth);
                //Storyboard.SetTargetName(myDoubleAnimationWidth, b.Name);
                //Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

                //aStoryboard.Children.Add(myDoubleAnimationHeight);
                //Storyboard.SetTargetName(myDoubleAnimationHeight, b.Name);
                //Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

                //aStoryboard.Children.Add(myDoubleAnimationText);
                //Storyboard.SetTargetName(myDoubleAnimationText, (b.Name.Split('_').First() + "_textblock"));
                //Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

                //aStoryboard.Children.Add(myThicknessAnimation);
                //Storyboard.SetTargetName(myThicknessAnimation, b.Name);
                //Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

                //aStoryboard.Begin(this);
            }
            b.BorderBrush = Brushes.Transparent;
            Console.WriteLine("Mouse leave called {0} - {1} - {2}", sender.GetType(), b.Name, e.LeftButton);

        }

        private void IconPressed(GroupBorder b)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.5,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationWidth = new DoubleAnimation
            {
                From = iconWidth,
                To = iconWidthShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            {
                From = iconHeight,
                To = iconHeightShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            {
                From = iconTitleFontSize,
                To = iconTitleFontSizeShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            {
                From = iconMargin,
                To = iconMarginShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            Storyboard aStoryboard = new Storyboard();
            aStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            aStoryboard.Children.Add(myDoubleAnimationWidth);
            Storyboard.SetTargetName(myDoubleAnimationWidth, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            aStoryboard.Children.Add(myDoubleAnimationHeight);
            Storyboard.SetTargetName(myDoubleAnimationHeight, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            aStoryboard.Children.Add(myDoubleAnimationText);
            Storyboard.SetTargetName(myDoubleAnimationText, (b.Name.Split('_').First() + "_textblock"));
            Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            aStoryboard.Children.Add(myThicknessAnimation);
            Storyboard.SetTargetName(myThicknessAnimation, b.Name);
            Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            aStoryboard.Begin(this);
        }

        private void IconReleased(GroupBorder b)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationWidth = new DoubleAnimation
            {
                From = iconWidthShrink,
                To = iconWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            {
                From = iconHeightShrink,
                To = iconHeight,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            {
                From = iconTitleFontSizeShrink,
                To = iconTitleFontSize,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            {
                From = iconMarginShrink,
                To = iconMargin,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            Storyboard aStoryboard = new Storyboard();
            aStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            aStoryboard.Children.Add(myDoubleAnimationWidth);
            Storyboard.SetTargetName(myDoubleAnimationWidth, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            aStoryboard.Children.Add(myDoubleAnimationHeight);
            Storyboard.SetTargetName(myDoubleAnimationHeight, b.Name);
            Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            aStoryboard.Children.Add(myDoubleAnimationText);
            Storyboard.SetTargetName(myDoubleAnimationText, (b.Name.Split('_').First() + "_textblock"));
            Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            aStoryboard.Children.Add(myThicknessAnimation);
            Storyboard.SetTargetName(myThicknessAnimation, b.Name);
            Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            aStoryboard.Begin(this);
            b.BorderBrush = Brushes.Transparent;
        }
    }
}
