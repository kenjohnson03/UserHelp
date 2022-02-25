using System;
using System.Collections.Generic;
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
    /// Interaction logic for ActionControl.xaml
    /// </summary>
    public partial class ActionControl : UserControl, INotifyPropertyChanged
    {
        private string _Text { get; set; }
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                //DataContextChangedUpdate("Text");
            }
        }
        public string Id { get; set; }
        public Color Color { get; set; }
        private Boolean _Selected { get; set; }
        public Boolean Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                if (_Selected == true)
                {
                    State = AState.Selected;
                }
                else
                {
                    State = AState.NotSelected;
                }
                SelectedChanged();
            }
        }
        public double SelectedOpacity
        {
            get
            {
                if (Selected == true)
                {
                    return 1.0;
                }
                return 0;
            }
        }
        public Color HoverColor { get; set; }
        public Color SelectedColor { get; set; }
        private enum AState { NotSelected, Selected, Hover }
        private AState State { get; set; } = AState.NotSelected;
        public Brush ActionBackground
        {
            get
            {
                switch (State)
                {
                    case AState.NotSelected:
                        return Brushes.Transparent;
                    case AState.Selected:
                        return new SolidColorBrush(SelectedColor);
                    case AState.Hover:
                        return new SolidColorBrush(HoverColor);
                    default:
                        return Brushes.Transparent;
                }
            }
        }
        public delegate void UpdateParent(ActionControl actionControl);
        public UpdateParent UpdateTheParent;

        public event PropertyChangedEventHandler PropertyChanged;

        public ActionControl()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        public ActionControl(String Text, Color Color, Boolean Selected, Color HoverColor, Color SelectedColor)
        {
            InitializeComponent();
            this.Text = Text;
            this.Color = Color;
            this.HoverColor = HoverColor;
            this.SelectedColor = SelectedColor;
            this.DataContext = this;

            this.Selected = Selected;
        }

        private void DataContextChangedUpdate(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SelectedChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedOpacity"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActionBackground"));
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.Selected == true)
            {
                this.Selected = false;
            }
            else
            {
                this.Selected = true;
            }
            UpdateTheParent(this);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            State = AState.Hover;
            //Grid p = sender as Grid;
            //p.Background = new SolidColorBrush(HoverColor);
            DataContextChangedUpdate("ActionBackground");
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //Grid p = sender as Grid;
            //if(this.Selected == true)
            //{
            //    p.Background = new SolidColorBrush(SelectedColor);
            //}
            //else
            //{
            //    p.Background = Brushes.Transparent;                
            //}

            if (this.Selected == true)
            {
                State = AState.Selected;
            }
            else
            {
                State = AState.NotSelected;
            }
            DataContextChangedUpdate("ActionBackground");
        }
    }
}
