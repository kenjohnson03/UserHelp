using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appy.Classes
{
    public class UserAction
    {
        public string Id { get; 
            private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CommandArguments { get; set; }

        public UserAction()
        {
            Id = System.Guid.NewGuid().ToString();
        }
    }
}
