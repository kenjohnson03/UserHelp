using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appy.Data
{
    public class ActionPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<String> _cmdOutputLines = new ObservableCollection<string>();
        
        public ObservableCollection<String> CommandOutputLines
        {
            get { return _cmdOutputLines; }
        }

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

        public void AddLine(string Line)
        {
            _cmdOutputLines.Add(Line);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommandOutputLines"));
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
