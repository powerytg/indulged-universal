using Indulged.API.Utils;
using Indulged.UI.Dashboard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Indulged.UI.Dashboard
{
    public class DashboardThemeManager
    {
        // Events
        public EventHandler<DashboardThemeChangedEventArgs> ThemeChanged;

        private static volatile DashboardThemeManager instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Singleton
        /// </summary>
        public static DashboardThemeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DashboardThemeManager();
                    }
                }

                return instance;
            }
        }

        private DashboardThemes _selectedTheme = DashboardThemes.Dark;
        public DashboardThemes SelectedTheme
        {
            get
            {
                return _selectedTheme;
            }

            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;

                    var evt = new DashboardThemeChangedEventArgs();
                    evt.SelectedTheme = _selectedTheme;
                    ThemeChanged.DispatchEvent(this, evt);

                    // Adjust status bar color
                    StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    if (_selectedTheme == DashboardThemes.Light)
                    {
                        statusBar.ForegroundColor = Color.FromArgb(0xff, 0x79, 0x79, 0x79);
                    }
                    else
                    {
                        statusBar.ForegroundColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardThemeManager()
        {

        }
    }
}
