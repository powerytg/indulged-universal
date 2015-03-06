using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PreludeStreamStyleDialog : UserControl
    {
        public string SelectedStyle { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PreludeStreamStyleDialog()
        {
            this.InitializeComponent();

            // Select the current style
            SelectedStyle = PolKit.PolicyKit.Instance.PreludeLayoutStyle;
            if (SelectedStyle == StreamLayoutStyle.Journal)
            {
                JournalButton.IsChecked = true;
            }
            else if (SelectedStyle == StreamLayoutStyle.Magazine)
            {
                MagazineButton.IsChecked = true;
            }
            else if (SelectedStyle == StreamLayoutStyle.Linear)
            {
                LinearButton.IsChecked = true;
            }
        }

        private void JournalButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedStyle = StreamLayoutStyle.Journal;
        }

        private void MagazineButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedStyle = StreamLayoutStyle.Magazine;
        }

        private void LinearButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedStyle = StreamLayoutStyle.Linear;
        }
    }
}
