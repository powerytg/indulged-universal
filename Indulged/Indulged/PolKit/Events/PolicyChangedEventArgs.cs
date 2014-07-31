using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.PolKit.Events
{
    public class PolicyChangedEventArgs : EventArgs
    {
        public string PolicyName { get; set; }
    }
}
