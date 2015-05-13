using Nokia.Graphics.Imaging;
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

namespace Indulged.UI.ProFX.Filters
{
    public sealed partial class FXVignetteFilter : FilterBase
    {
        private Windows.UI.Color vignetteColor = Windows.UI.Color.FromArgb(0xff, 0, 0, 0);
        private double radius = 0.4;

        /// <summary>
        /// Constructor
        /// </summary>
        public FXVignetteFilter()
        {
            this.InitializeComponent();

            DisplayName = "vignette";
            StatusBarName = "Vigenette";
            Category = FilterCategory.Enhancement;
        }

        public override void CreateFilter()
        {
            Filter = new VignettingFilter(radius, vignetteColor);
        }

        private void AmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (AmountSlider == null)
            {
                return;
            }

            radius = AmountSlider.Value;
            UpdatePreviewAsync();
        }
    }
}
