using System;
using System.ComponentModel;

namespace Appy.Data
{
    public class ActionPageViewModel : INotifyPropertyChanged
    {
        private Boolean _runEnabled { get; set; }
        public Boolean RunEnabled
        {
            get
            {
                return _runEnabled;
            }
        }

        public ActionPageViewModel()
        {
        }

        public void EnableRun()
        {
            _runEnabled = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RunEnabled"));
        }

        public void DisableRun()
        {
            _runEnabled = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RunEnabled"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
