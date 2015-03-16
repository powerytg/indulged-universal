using System;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.Controls
{
    public sealed partial class CommonPageHeader : CommonHeaderBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPageHeader()
        {
            this.InitializeComponent();
        }

        protected override void OnTitleChanged()
        {
            TitleLabel.Text = Title;
        }

        protected override void OnThemeChanged()
        {
            if (Theme == "Light")
            {
                HeaderImageView.Source = new BitmapImage(new Uri("ms-appx:///Assets/Dashboard/LightBanner.png"));
            }
            else if(Theme == "Dark")
            {
                HeaderImageView.Source = new BitmapImage(new Uri("ms-appx:///Assets/Dashboard/DarkBanner.png"));
            }
        }
    }
}
