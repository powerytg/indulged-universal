using Indulged.UI.ProFX.Filters;
using System;

namespace Indulged.UI.ProFX.Events
{
    public class AddFilterEventArgs : EventArgs
    {
        public FilterBase Filter { get; set; }
    }
}
