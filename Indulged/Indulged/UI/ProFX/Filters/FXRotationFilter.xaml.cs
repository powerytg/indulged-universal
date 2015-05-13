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
    public sealed partial class FXRotationFilter : FilterBase
    {
        public double Degree { get; set; }

        public FXRotationFilter()
        {
            InitializeComponent();
            Degree = 0.0;
            Category = FilterCategory.Transform;

            DisplayName = "rotation";
            StatusBarName = "Rotate Image";
        }

        public override void CreateFilter()
        {
            //Filter = new RotationFilter(Degree);
            Filter = new ReframingFilter(new Rect(0, 0, OriginalPreviewImageWidth, OriginalPreviewImageHeight), Degree);
        }

        private void AmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (AmountSlider == null)
            {
                return;
            }

            Degree = AmountSlider.Value;
            UpdatePreviewAsync();
        }

    }
}
