using Indulged.API.Networking;
using Indulged.UI.Common.PhotoStream.Factories;
using Indulged.UI.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonStreamListView : StreamListViewBase
    {
        /// <summary>
        /// Data source
        /// </summary>
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

            this.ItemsSource = ds;
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

        private void InnerGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate wrap grid unit size
            double screenWidth = Window.Current.Bounds.Width;
            double cellSize = Math.Floor(screenWidth / PhotoTileLayoutGeneratorBase.MAX_COL_COUNT);
            var grid = sender as VariableSizedWrapGrid;
            grid.ItemWidth = cellSize - 4;
            grid.ItemHeight = cellSize;       
        }
    }
}
