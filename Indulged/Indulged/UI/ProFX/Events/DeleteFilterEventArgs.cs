using Indulged.UI.ProFX.Filters;
using System;

namespace Indulged.UI.ProFX.Events
{
    public class DeleteFilterEventArgs : EventArgs
    {
        public FilterBase Filter { get; set; }
    }
}
