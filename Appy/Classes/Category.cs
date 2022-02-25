using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appy.Classes
{
    public class Category
    {

        public string Name { get; set; }
        public string Id { get; 
            private set;
            }
        public string Description { get; set; }

        public Category()
        {
            Id = System.Guid.NewGuid().ToString();
            Children = new ObservableCollection<UserAction>();
        }

        public ObservableCollection<UserAction> UserActions { get; set; }

        public ObservableCollection<UserAction> Children
        {
            get;
            set;
        }
    }
}
