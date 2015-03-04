using Indulged.API.Storage;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class JournalPhotoRenderer : PhotoRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JournalPhotoRenderer()
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

            if (StorageService.Instance.UserCache.ContainsKey(Photo.UserId))
            {
                AuthorLabel.Text = StorageService.Instance.UserCache[Photo.UserId].Name;
            }

            if (Photo.Title.Length == 0)
            {
                TitleLabel.Text = "Untitled";
            }
            else
            {
                TitleLabel.Text = Photo.Title;
            }

            if (Photo.Description.Length > 0 && !CommonPhotoOverlayView.IsTextInBlackList(Photo.Description))
            {
                DescPanel.Visibility = Visibility.Visible;
                DescLabel.Text = Photo.Description;
            }
            else
            {
                DescPanel.Visibility = Visibility.Collapsed;
            }
        }

    }
}
