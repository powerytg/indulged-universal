using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.OSD
{
    public sealed partial class CropHUD : UserControl
    {
        public EventHandler OnDismiss;
        public EventHandler OnDelete;

        // Constructor
        public CropHUD()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDismiss != null)
            {
                OnDismiss(this, null);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this, null);
            }
        }
    }
}
