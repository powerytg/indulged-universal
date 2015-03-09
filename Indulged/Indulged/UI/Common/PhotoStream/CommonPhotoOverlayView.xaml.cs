using Indulged.API.Storage.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonPhotoOverlayView : UserControl
    {
        public enum PhotoOverlayLayoutMode
        {
            Mini, Overlay
        }

        private static SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private static SolidColorBrush solidBrush = new SolidColorBrush(Color.FromArgb(255, 25, 29, 36));
        private static SolidColorBrush semiTransparentBrush = new SolidColorBrush(Color.FromArgb(215, 25, 29, 36));

        public static readonly DependencyProperty PhotoSourceProperty = DependencyProperty.Register(
        "PhotoSource",
        typeof(FlickrPhoto),
        typeof(CommonPhotoOverlayView),
        new PropertyMetadata(null, OPhotoSourcePropertyChanged));

        public FlickrPhoto PhotoSource
        {
            get { return (FlickrPhoto)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        private static void OPhotoSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonPhotoOverlayView)sender;
            target.OnPhotoSourceChanged();
        }

        private void OnPhotoSourceChanged()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPhotoOverlayView()
        {
            this.InitializeComponent();
        }

        private PhotoOverlayLayoutMode _layoutMode;
        public PhotoOverlayLayoutMode LayoutMode
        {
            get
            {
                return _layoutMode;
            }

            set
            {
                _layoutMode = value;
                PerformLayout();
            }
        }

        private void PerformLayout()
        {
            switch (_layoutMode)
            {
                case PhotoOverlayLayoutMode.Mini:
                    LayoutInMiniMode();
                    break;
                case PhotoOverlayLayoutMode.Overlay:
                    LayoutInOverlayMode();
                    break;
            }
        }

        private void LayoutInMiniMode()
        {
            LayoutRoot.Background = transparentBrush;

            if (ShouldShowTitle())
            {
                var title = PolKit.PolicyKit.Instance.UseCleanText ? PhotoSource.CleanTitle : PhotoSource.Title;
                TitleLabel.Text = title;
                TitleLabel.Visibility = Visibility.Visible;
            }
            else
            {
                TitleLabel.Visibility = Visibility.Collapsed;
            }

            DescLabel.Visibility = Visibility.Collapsed;
        }
        
        private void LayoutInOverlayMode()
        {
            LayoutRoot.Background = semiTransparentBrush;

            if (ShouldShowTitle())
            {
                var title = PolKit.PolicyKit.Instance.UseCleanText ? PhotoSource.CleanTitle : PhotoSource.Title;
                TitleLabel.Text = title;
                TitleLabel.Visibility = Visibility.Visible;
            }
            else
            {
                TitleLabel.Visibility = Visibility.Collapsed;
            }

            if (ShouldShowDescription())
            {
                var desc = PolKit.PolicyKit.Instance.UseCleanText ? PhotoSource.CleanDescription : PhotoSource.Description;
                DescLabel.Text = desc;
                DescLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }

        }

        private bool ShouldShowTitle()
        {
            if (PhotoSource.Title.Length == 0)
            {
                return false;
            }

            // Filter out blacklist
            if (IsTextInBlackList(PhotoSource.Title))
            {
                return false;
            }

            return true;
        }

        private bool ShouldShowDescription()
        {
            if (PhotoSource.Description.Length == 0)
            {
                return false;
            }

            // Filter out blacklist
            if (IsTextInBlackList(PhotoSource.Description))
            {
                return false;
            }

            return true;
        }

        public static bool IsTextInBlackList(string text)
        {
            string lowcaseText = text.ToLower().Trim();
            if (lowcaseText.StartsWith("untitled")
                || lowcaseText.StartsWith("img")
                || lowcaseText.StartsWith("http")
                || lowcaseText.StartsWith("<"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
