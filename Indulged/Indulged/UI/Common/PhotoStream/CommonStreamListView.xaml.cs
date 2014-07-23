using Indulged.API.Networking;
using Indulged.UI.Models;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonStreamListView : StreamListViewBase
    {
        private PhotoTileCollection ds = new PhotoTileCollection();

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonStreamListView()
        {
            this.InitializeComponent();

            // Events
            ds.LoadingStarted = OnLoadingStarted;
            ds.LoadingComplete = OnLoadingComplete;
        }

        protected async override void OnStreamChanged()
        {
            if (Stream == null)
            {
                return;
            }

            PhotoListView.ItemsSource = ds;
            ds.Stream = Stream;
            await ds.LoadMoreItemsAsync((uint)APIService.PerPage);
        }

        private void OnLoadingStarted(object sender, EventArgs e)
        {
            // Delegate event
            if (LoadingStarted != null)
            {
                LoadingStarted(this, null);
            }
        }

        private void OnLoadingComplete(object sender, EventArgs e)
        {
            // Delegate event
            if (LoadingComplete != null)
            {
                LoadingComplete(this, null);
            }
        }
    }
}
