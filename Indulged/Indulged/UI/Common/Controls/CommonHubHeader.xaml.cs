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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.Controls
{
    public sealed partial class CommonHubHeader : CommonHeaderBase
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public CommonHubHeader()
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
                HeaderImageView.Source = new BitmapImage(new Uri("ms-appx:///Assets/Dashboard/LightBannerWide.png"));
            }
            else
            {

            }
        }
    }
}
