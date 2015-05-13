using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.OSD
{
    public sealed partial class RotationHUD : UserControl
    {
        public EventHandler OnDismiss;
        public EventHandler OnDelete;
        public EventHandler ValueChanged;

        public double Degree { get; set; }

        // Constructor
        public RotationHUD()
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

        private void AmountSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (AmountSlider == null)
            {
                return;
            }

            Degree = AmountSlider.Value;
            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        public Slider GetAmountSlider()
        {
            return AmountSlider;
        }

    }
}
