using Indulged.UI.Dashboard.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Dashboard
{
    public sealed partial class DashboardBackgroundView : UserControl
    {
        private static SolidColorBrush darkBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x1c, 0x1c, 0x1c));
        private static SolidColorBrush lightBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));

        public void ShowLightBackground()
        {
            LayoutRoot.Background = lightBrush;
        }

        public void ShowDarkBackground()
        {
            LayoutRoot.Background = darkBrush;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardBackgroundView()
        {
            this.InitializeComponent();

            // Events
            DashboardThemeManager.Instance.ThemeChanged += OnThemeChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DashboardThemeManager.Instance.ThemeChanged -= OnThemeChanged;
        }

        private void OnThemeChanged(object sender, DashboardThemeChangedEventArgs e)
        {
            if (e.SelectedTheme == DashboardThemes.Dark)
            {
                ShowDarkBackground();
            }
            else if (e.SelectedTheme == DashboardThemes.Light)
            {
                ShowLightBackground();
            }
        }
    }
}
