using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.UI.Common.Controls;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Dashboard
{
    public sealed partial class CatalogSection : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogSection()
        {
            this.InitializeComponent();

            // Events
            StorageService.Instance.AlbumListUpdated += OnAlbumListUpdated;

            // Get album list
            LoadingView.LoadingAction = () =>
            {
                LoadAlbumList();
            };

            LoadAlbumList();
        }

        private async void LoadAlbumList()
        {
            var retVal = await APIService.Instance.GetAlbumListAsync();
            if (!retVal.Success)
            {
                LoadingView.ErrorText = retVal.ErrorMessage;
                LoadingView.ShowRetryScreen();
            }
            else
            {
                // This should execute only once
                LoadingView.Destroy();
                ContainerView.Visibility = Visibility.Visible;
                AlbumGridView.AlbumList = StorageService.Instance.AlbumList;
            }
        }

        private void OnAlbumListUpdated(object sender, StorageEventArgs e)
        {
            // Update album count
            int albumCount = StorageService.Instance.AlbumList.Count;
            AlbumHeaderView.Title = albumCount.ToString();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StorageService.Instance.AlbumListUpdated -= OnAlbumListUpdated;
        }

    }
}
