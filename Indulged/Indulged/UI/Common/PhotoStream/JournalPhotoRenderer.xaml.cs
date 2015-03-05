using Indulged.API.Storage;
using System;
using Windows.UI.Xaml;
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

    }
}
