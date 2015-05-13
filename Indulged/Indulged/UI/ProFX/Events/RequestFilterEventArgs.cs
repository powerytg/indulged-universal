using Indulged.UI.ProFX.Filters;
using System;

namespace Indulged.UI.ProFX.Events
{
    public class RequestFilterEventArgs : EventArgs
    {
        public FilterBase Filter { get; set; }
    }
}
