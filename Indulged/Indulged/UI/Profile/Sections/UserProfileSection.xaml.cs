using Indulged.API.Networking;
using Indulged.API.Storage;
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

namespace Indulged.UI.Profile.Sections
{
    public sealed partial class UserProfileSection : UserSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserProfileSection()
        {
            this.InitializeComponent();

            // Initial state
            ContentView.Visibility = Visibility.Collapsed;
            LoadingView.ShowLoadingScreen();
        }

        protected override void OnUserChanged()
        {
            base.OnUserChanged();

            // Always refresh user info
            LoadingView.LoadingAction = () =>
            {
                LoadUserInfo();
            };

            LoadUserInfo();
        }

        private async void LoadUserInfo()
        {
            var status = await APIService.Instance.GetUserInfoAsync(User.ResourceId);
            if (!status.Success)
            {
                LoadingView.ErrorText = "Cannot get user info: " + status.ErrorMessage;
                LoadingView.ShowRetryScreen();
            }
            else
            {
                LoadingView.Destroy();
                UpdateInfoView();
            }
        }

        private void UpdateInfoView()
        {
            ContentView.Visibility = Visibility.Visible;

            AvatarView.Source = new BitmapImage(new Uri(User.AvatarUrl));

            if (User == StorageService.Instance.CurrentUser){
                NameLabel.Text = "You";
            }
            else
            {
                NameLabel.Text = User.Name;
            }               

            if (User.IsProUser){
                ProLabel.Visibility = Visibility.Visible;
            }
            else
            {
                ProLabel.Visibility = Visibility.Collapsed;
            }
                
            if (User.RealName != null && User.RealName.Length > 0)
            {
                RealNameSection.Visibility = Visibility.Visible;
                RealNameLabel.Text = User.RealName;
            }
            else
            {
                RealNameSection.Visibility = Visibility.Collapsed;
            }

            if (User.Location != null && User.Location.Length > 0)
            {
                LocationSection.Visibility = Visibility.Visible;
                LocationLabel.Text = User.Location;
            }
            else
            {
                LocationSection.Visibility = Visibility.Collapsed;
            }

            if (User.Description != null && User.Description.Length > 0)
            {
                DescSection.Visibility = Visibility.Visible;

                if (PolKit.PolicyKit.Instance.UseCleanText)
                {
                    DescLabel.Text = User.CleanDescription;
                }
                else
                {
                    DescLabel.Text = User.Description;
                }                
            }
            else
            {
                DescSection.Visibility = Visibility.Collapsed;
            }

            if (User.ProfileUrl != null && User.ProfileUrl.Length > 0)
            {
                ProfileUrlSection.Visibility = Visibility.Visible;
                ProfileUrlRun.Text = User.ProfileUrl;
            }
            else
            {
                ProfileUrlSection.Visibility = Visibility.Collapsed;
            }

            if (User.PhotoStream.PhotoCount == 0){
                PhotoCountLabel.Text = "No photo yet";
            }
            else
            {
                PhotoCountLabel.Text = User.PhotoStream.PhotoCount.ToString();
            }                
        }

        private async void ProfileUrlLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(User.ProfileUrl, UriKind.RelativeOrAbsolute));
        }

    }
}
