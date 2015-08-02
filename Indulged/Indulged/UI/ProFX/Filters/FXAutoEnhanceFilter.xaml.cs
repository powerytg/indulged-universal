using Lumia.Imaging.Adjustments;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Filters
{
    public sealed partial class FXAutoEnhanceFilter : FilterBase
    {
        private bool isAutoBrightnessContrastOn = true;
        private bool isAutoClarityOn = true;

        public FXAutoEnhanceFilter()
        {
            this.InitializeComponent();

            DisplayName = "auto enhance";
            StatusBarName = "Auto Enhance";
            Category = FilterCategory.Enhancement;

        }

        private void BrightnessToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (BrightnessToggle == null)
            {
                return;
            }

            isAutoBrightnessContrastOn = BrightnessToggle.IsOn;
            UpdatePreviewAsync();
        }

        private void ClarityToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (ClarityToggle == null)
            {
                return;
            }

            isAutoClarityOn = ClarityToggle.IsOn;
            UpdatePreviewAsync();
        }

        public override void CreateFilter()
        {
            Filter = new AutoEnhanceFilter(isAutoBrightnessContrastOn, isAutoClarityOn);
        }

    }
}
