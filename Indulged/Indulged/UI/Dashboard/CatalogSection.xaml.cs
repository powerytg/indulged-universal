using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
using Indulged.UI.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Navigation;

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
            LoadAlbumList();
        }

        private async void LoadAlbumList()
        {
            var retVal = await APIService.Instance.GetAlbumListAsync();
            if (!retVal.Success)
            {
                var dialog = ModalPopup.Show(retVal.ErrorMessage, "Cannot load album list", new List<String> { "Retry", "Cancel" });
                dialog.DismissWithButtonClick += (sender, e) =>
                {
                    if (e.ButtonIndex == 0)
                    {
                        LoadAlbumList();
                    }
                };
            }
        }

        private void OnAlbumListUpdated(object sender, StorageEventArgs e)
        {
            var ds = new ObservableCollection<FlickrModelBase>();
            ds.Add(new DashboardAlbumHeader());
            foreach (var album in StorageService.Instance.AlbumList)
            {
                ds.Add(album);
            }

            AlbumListView.ItemsSource = ds;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StorageService.Instance.AlbumListUpdated -= OnAlbumListUpdated;
        }

    }
}
