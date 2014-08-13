using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.Dashboard.Events
{
    public class DashboardThemeChangedEventArgs : EventArgs
    {
        public DashboardThemes SelectedTheme { get; set; }
    }
}
