using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Detail.Sections
{
    public sealed partial class BasicInfoSection : DetailSectionBase
    {
        private static SolidColorBrush createCCBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xf6, 0x8e, 0x56));
        private static SolidColorBrush noCCBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x03, 0xbe, 0x90));
        private static SolidColorBrush resrictedBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xf6, 0x8e, 0x56));
        private static SolidColorBrush unknownBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x6b, 0x8e, 0xac));

        /// <summary>
        /// Constructor
        /// </summary>
        public BasicInfoSection()
        {
            this.InitializeComponent();
        }

        public override void AddEventListeners()
        {
            base.AddEventListeners();
            StorageService.Instance.PhotoAddedAsFavourite += OnPhotoAddedToFavourite;
            StorageService.Instance.PhotoRemovedFromFavourite += OnPhotoRemovedFromFavourite;
        }

        public override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
            StorageService.Instance.PhotoAddedAsFavourite -= OnPhotoAddedToFavourite;
            StorageService.Instance.PhotoRemovedFromFavourite -= OnPhotoRemovedFromFavourite;
        }

        private void ImageView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        protected override void OnPhotoChanged()
        {
            base.OnPhotoChanged();

            var title = PolKit.PolicyKit.Instance.UseCleanText ? Photo.CleanTitle : Photo.Title;
            if (title.Length > 0)
            {
                TitleLabel.Text = title;
            }
            else
            {
                TitleLabel.Text = "Untitled";
            }

            // Date
            DateLabel.Text = Photo.DateTaken;            

            // Image view
            ImageView.Source = new BitmapImage(new Uri(Photo.GetImageUrl(), UriKind.Absolute));
            
            // Author
            var user = StorageService.Instance.UserCache[Photo.UserId];
            AuthorLabel.Text = user.Name;

            // Stats
            if (Photo.ViewCount == 0)
            {
                StatLabel.Text = "No view information";
            }
            else
            {
                StatLabel.Text = Photo.ViewCount.ToString() + " views";
            }

            // Like icon
            if (Photo.IsFavourite)
            {
                LikeIcon.Visibility = Visibility.Visible;
            }
            else
            {
                LikeIcon.Visibility = Visibility.Collapsed;
            }

            // Description
            var desc = PolKit.PolicyKit.Instance.UseCleanText ? Photo.CleanDescription : Photo.Description;
            if (desc.Length > 0)
            {
                DescLabel.Text = desc;
                DescLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }

            // License
            if (Photo.LicenseId == null){
                LicenseButton.Content = "Unknown License";
            }                
            else
            {
                var license = PolKit.PolicyKit.Instance.Licenses[Photo.LicenseId];
                LicenseLabel.Text = license.Name;
            }
        }

        private async void LicenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Photo.LicenseId == null)
            {
                return;
            }                

            var license = PolKit.PolicyKit.Instance.Licenses[Photo.LicenseId];
            if (license.Url == null)
            {
                return;
            }
                
            await Windows.System.Launcher.LaunchUriAsync(new Uri(license.Url, UriKind.RelativeOrAbsolute));
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnPhotoAddedToFavourite(object sender, StorageEventArgs e)
        {
            if (Photo == null || e.PhotoId != Photo.ResourceId)
            {
                return;
            }

            LikeIcon.Visibility = Photo.IsFavourite ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnPhotoRemovedFromFavourite(object sender, StorageEventArgs e)
        {
            if (Photo == null || e.PhotoId != Photo.ResourceId)
            {
                return;
            }

            LikeIcon.Visibility = Photo.IsFavourite ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
