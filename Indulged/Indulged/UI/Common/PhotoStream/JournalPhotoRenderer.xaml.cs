using Indulged.API.Storage;
using Indulged.UI.Detail;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class JournalPhotoRenderer : PhotoTileRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JournalPhotoRenderer() : base()
        {
            this.InitializeComponent();
        }

        protected override void LayoutCells(double containerWidth)
        {
            if (PhotoTileSource == null)
            {
                return;
            }

            base.LayoutCells(containerWidth);

            var photo = PhotoTileSource.Photos[0];
            if (photo.Height != 0)
            {
                ImageView.Height = Math.Min(photo.Height, 400);
            }
            else
            {
                ImageView.Height = 340;
            }

            ImageView.Source = new BitmapImage(new Uri(photo.GetImageUrl(), UriKind.Absolute));


            if (photo.Title.Length == 0)
            {
                TitleLabel.Text = "Untitled";
            }
            else
            {
                TitleLabel.Text = photo.Title;
            }         

            if (photo.Description.Length > 0 && !CommonPhotoOverlayView.IsTextInBlackList(photo.Description))
            {
                DescPanel.Visibility = Visibility.Visible;
                DescLabel.Text = photo.Description;
            }
            else
            {
                DescPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void PhotoTileRendererBase_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(DetailPage), PhotoTileSource.Photos[0].ResourceId);
        }

        private void PhotoRendererBase_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            bool result = VisualStateManager.GoToState(this, "Pressed", false);
        }

        private void PhotoRendererBase_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void PhotoRendererBase_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void PhotoRendererBase_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

    }
}
