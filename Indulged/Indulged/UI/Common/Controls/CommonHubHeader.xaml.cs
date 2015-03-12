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
    public sealed partial class CommonHubHeader : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(string),
        typeof(CommonHubHeader),
        new PropertyMetadata("", OnTitlePropertyChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonHubHeader)sender;
            target.OnTitleChanged();
        }

        private void OnTitleChanged()
        {
            TitleLabel.Text = Title;
        }

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
        "Theme",
        typeof(string),
        typeof(CommonHubHeader),
        new PropertyMetadata("Light", OnThemePropertyChanged));

        public string Theme
        {
            get { return (string)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        private static void OnThemePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonHubHeader)sender;
            target.OnThemeChanged();
        }

        private void OnThemeChanged()
        {
            if (Theme == "Light")
            {
                HeaderImageView.Source = new BitmapImage(new Uri("/Assets/LightBannerWide.png", UriKind.Relative));
            }
            else
            {

            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonHubHeader()
        {
            this.InitializeComponent();
        }
    }
}
