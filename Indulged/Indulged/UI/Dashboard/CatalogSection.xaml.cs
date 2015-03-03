using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
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
        private bool albumListRetrieved = false;
        private bool groupListRetrieved = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogSection()
        {
            this.InitializeComponent();

            // Events
            StorageService.Instance.AlbumListUpdated += OnAlbumListUpdated;
            StorageService.Instance.GroupListUpdated += OnGroupListUpdated;

            // Retry action for loading album list and group list
            LoadingView.LoadingAction = () =>
            {
                LoadAlbumsAndGroups();
            };

            LoadAlbumsAndGroups();
        }

        private async void LoadAlbumsAndGroups()
        {
            // Load album list if not done already
            if (!albumListRetrieved)
            {
                var albumStatus = await APIService.Instance.GetAlbumListAsync();
                if (!albumStatus.Success)
                {
                    // Failure
                    LoadingView.ErrorText = albumStatus.ErrorMessage;
                    LoadingView.ShowRetryScreen();
                    return;
                }
                else
                {
                    albumListRetrieved = true;
                }
            }

            // Load group list if now done already
            if (!groupListRetrieved)
            {
                var groupStatus = await APIService.Instance.GetGroupListAsync(StorageService.Instance.CurrentUser.ResourceId);
                if (!groupStatus.Success)
                {
                    // Failure
                    LoadingView.ErrorText = groupStatus.ErrorMessage;
                    LoadingView.ShowRetryScreen();
                    return;
                }
                else
                {
                    groupListRetrieved = true;
                }
            }
            
            // Check whether both albums and groups have been retrieved
            if (albumListRetrieved && groupListRetrieved)
            {
                // Remove loading screen and fill data context
                LoadingView.Destroy();
                ContainerView.Visibility = Visibility.Visible;
                AlbumGridView.AlbumList = StorageService.Instance.AlbumList;

                List<FlickrGroup> groups = new List<FlickrGroup>();
                foreach(var groupId in StorageService.Instance.CurrentUser.GroupIds)
                {
                    var group = StorageService.Instance.GroupCache[groupId];
                    groups.Add(group);
                }

                GroupGridView.GroupList = groups;
            }
        }

        private void OnAlbumListUpdated(object sender, StorageEventArgs e)
        {
            // Update album count
            int albumCount = StorageService.Instance.AlbumList.Count;
            AlbumHeaderView.Title = albumCount.ToString();
        }

        private void OnGroupListUpdated(object sender, StorageEventArgs e)
        {
            var currentUser = StorageService.Instance.CurrentUser;
            // Ignore all other users
            if (e.UserId != currentUser.ResourceId)
            {
                return;
            }

            // Update group count
            int groupCount = currentUser.GroupIds.Count;
            GroupHeaderView.Title = groupCount.ToString();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StorageService.Instance.AlbumListUpdated -= OnAlbumListUpdated;
        }

    }
}
