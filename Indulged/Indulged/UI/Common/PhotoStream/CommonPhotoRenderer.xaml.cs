using Indulged.PolKit;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonPhotoRenderer : PhotoRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPhotoRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnPhotoChanged()
        {
            base.OnPhotoChanged();

            if (Photo == null)
            {
                return;
            }

            ImageView.Source = new BitmapImage(new Uri(Photo.GetImageUrl(), UriKind.Absolute));
            OverlayView.PhotoSource = Photo;
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Photo == null)
            {
                return;
            }

            if (!PolicyKit.Instance.ShowOverlayOnPreludeTiles)
            {
                OverlayView.Visibility = Visibility.Collapsed;
            }
            else
            {
                PerformLayout();
                OverlayView.Visibility = Visibility.Visible;
            }
        }

        private void PerformLayout()
        {
            double screenSize = Window.Current.Bounds.Width;

            LayoutRoot.RowDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Clear();
            OverlayView.ClearValue(FrameworkElement.MaxWidthProperty);
            OverlayView.ClearValue(FrameworkElement.MaxHeightProperty);
            OverlayView.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            OverlayView.ClearValue(FrameworkElement.VerticalAlignmentProperty);

            int textLength = Photo.Title.Length + Photo.Description.Length;

            if (textLength <= 100 || ActualWidth < screenSize * 0.4)
            {
                LayoutOverlayInMiniMode();
            }
            else if (textLength > 100 && textLength < 250)
            {
                if (CommonPhotoOverlayView.IsTextInBlackList(Photo.Description))
                {
                    LayoutOverlayInMiniMode();
                }
                else
                {
                    LayoutOverlayInOverlayModeVertically();
                }
            }
        }

        private void LayoutOverlayInMiniMode()
        {
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Mini;
            OverlayView.HorizontalAlignment = HorizontalAlignment.Left;
            OverlayView.VerticalAlignment = VerticalAlignment.Bottom;
        }

        private void LayoutOverlayInOverlayModeVertically()
        {
            double screenSize = Window.Current.Bounds.Width;
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Overlay;
            OverlayView.HorizontalAlignment = HorizontalAlignment.Stretch;
            OverlayView.VerticalAlignment = VerticalAlignment.Bottom;
            OverlayView.MaxHeight = ImageView.ActualHeight * 0.4;
        }

    }
}
