using Lumia.Imaging.Artistic;
using Windows.UI.Xaml.Controls.Primitives;

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
