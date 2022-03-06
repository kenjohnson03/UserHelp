using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UserHelp.UIElements;

namespace Appy.UIElements
{
    /// <summary>
    /// Interaction logic for UserHelpButton.xaml
    /// </summary>
    public partial class UserHelpButton : UserControl
    {
        private string _text = "";
        private string _description = "";

        public string Text
        {
            get
            {
                if(tb == null)
                {
                    return _text;
                }
                return tb.Text;
            }
            set
            {
                if (tb != null)
                {
                    tb.Text = value;
                }
                _text = value;
            }
        }

        public string Description
        {
            get
            {
                if (desc == null)
                {
                    return _description;
                }
                return desc.Text;
            }
            set
            {
                if (desc != null)
                {
                    desc.Text = value;
                }
                _description = value;
            }
        }

        public int IconWidth { get; set; }
        public int IconWidthShrink { get; set; }

        public int IconHeight { get; set; }
        public int IconHeightShrink { get; set; }

        public Thickness IconMargin { get; set; }
        public Thickness IconMarginShrink { get; set; }

        public int IconTitleFontSize { get; set; }
        public int IconTitleFontSizeShrink { get; set; }

        public int IconDescriptionFontSize { get; set; }

        public string Id
        {
            get;
            set;
        }

        private TextBlock tb;
        private TextBlock desc;
        private GroupBorder border;


        public UserHelpButton(string Id,
            int IconWidth = 175,
            int IconWidthShrink = 165,
            int IconHeight = 120,
            int IconHeightShrink = 110,
            int IconTitleFontSize = 18,
            int IconTitleFontSizeShrink = 17,
            int IconDescriptionFontSize = 15)
        {
            InitializeComponent();
            this.Id = Id;
            this.IconMargin = new Thickness(15);
            this.IconMarginShrink = new Thickness(20);
            this.IconWidth = IconWidth;
            this.IconWidthShrink = IconWidthShrink;
            this.IconHeight = IconHeight;
            this.IconHeightShrink = IconHeightShrink;
            this.IconTitleFontSize = IconTitleFontSize;
            this.IconTitleFontSizeShrink = IconTitleFontSizeShrink;
            this.IconDescriptionFontSize = IconDescriptionFontSize;
            BuildUI();
        }

        private void BuildUI()
        {

            tb = new TextBlock()
            {
                Text = this.Text,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap,
                FontSize = this.IconTitleFontSize,
                FontWeight = FontWeights.Bold,
            };
            desc = new TextBlock()
            {
                Text = this.Description,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap,
                FontSize = IconDescriptionFontSize,
                FontWeight = FontWeights.Normal
            };

            StackPanel s = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            s.Children.Add(tb);
            s.Children.Add(desc);

            border = new GroupBorder()
            {
                Width = this.IconWidth,
                Height = this.IconHeight,
                Background = Brushes.Transparent,
                Margin = this.IconMargin,
                Child = s,
                CornerRadius = new CornerRadius(12),
                Id = this.Id,
                BorderThickness = new Thickness(2)

            };


            border.MouseDown += new MouseButtonEventHandler(this.IconBlock_MouseDown);
            border.MouseUp += new MouseButtonEventHandler(this.IconBlock_MouseUp);
            border.MouseLeave += new MouseEventHandler(this.IconBlock_DragLeave);
            border.MouseEnter += new MouseEventHandler(this.IconBlock_MouseEnter);


            grid.Children.Add(border);
        }

        private void IconBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(100), Convert.ToByte(100), Convert.ToByte(100)));            
        }

        private void IconBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            IconPressed();
        }

        private void IconBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IconReleased();
        }

        private void IconBlock_MouseLeave(object sender, MouseButtonEventArgs e)
        {
            IconBlock_DragLeave(sender, e);
        }

        private void IconBlock_DragLeave(object sender, MouseEventArgs e)
        {
            IconReleased();
            
            border.BorderBrush = Brushes.Transparent;
        }


        private void IconPressed()
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
                From = IconWidth,
                To = IconWidthShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            {
                From = IconHeight,
                To = IconHeightShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            {
                From = IconTitleFontSize,
                To = IconTitleFontSizeShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            {
                From = IconMargin,
                To = IconMarginShrink,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            Storyboard aStoryboard = new Storyboard();
            aStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTarget(myDoubleAnimation, border);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            aStoryboard.Children.Add(myDoubleAnimationWidth);
            Storyboard.SetTarget(myDoubleAnimationWidth, border);
            Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            aStoryboard.Children.Add(myDoubleAnimationHeight);
            Storyboard.SetTarget(myDoubleAnimationHeight, border);
            Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            aStoryboard.Children.Add(myDoubleAnimationText);
            Storyboard.SetTarget(myDoubleAnimationText, tb);
            Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            aStoryboard.Children.Add(myThicknessAnimation);
            Storyboard.SetTarget(myThicknessAnimation, border);
            Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            aStoryboard.Begin(this);
        }

        private void IconReleased()
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
                From = IconWidthShrink,
                To = IconWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationHeight = new DoubleAnimation
            {
                From = IconHeightShrink,
                To = IconHeight,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            DoubleAnimation myDoubleAnimationText = new DoubleAnimation
            {
                From = IconTitleFontSizeShrink,
                To = IconTitleFontSize,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            ThicknessAnimation myThicknessAnimation = new ThicknessAnimation
            {
                From = IconMarginShrink,
                To = IconMargin,
                Duration = new Duration(TimeSpan.FromMilliseconds(0)),
                AutoReverse = false
            };

            Storyboard aStoryboard = new Storyboard();
            aStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTarget(myDoubleAnimation, this.border);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(GroupBorder.OpacityProperty));

            aStoryboard.Children.Add(myDoubleAnimationWidth);
            Storyboard.SetTarget(myDoubleAnimationWidth, this.border);
            Storyboard.SetTargetProperty(myDoubleAnimationWidth, new PropertyPath(GroupBorder.WidthProperty));

            aStoryboard.Children.Add(myDoubleAnimationHeight);
            Storyboard.SetTarget(myDoubleAnimationHeight, this.border);
            Storyboard.SetTargetProperty(myDoubleAnimationHeight, new PropertyPath(GroupBorder.HeightProperty));

            aStoryboard.Children.Add(myDoubleAnimationText);
            Storyboard.SetTarget(myDoubleAnimationText, this.tb);
            Storyboard.SetTargetProperty(myDoubleAnimationText, new PropertyPath(TextBlock.FontSizeProperty));

            aStoryboard.Children.Add(myThicknessAnimation);
            Storyboard.SetTarget(myThicknessAnimation, this.border);
            Storyboard.SetTargetProperty(myThicknessAnimation, new PropertyPath(GroupBorder.MarginProperty));

            aStoryboard.Begin(this);
            border.BorderBrush = Brushes.Transparent;
        }

    }
}
