using Indulged.API.Networking;
using Indulged.UI.Models;
using System;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonStreamListView : StreamListViewBase
    {
        private string displayStyle;
        public string DisplayStyle
        {
            get
            {
                return displayStyle;
            }

            set
            {
                if (displayStyle != value)
                {
                    displayStyle = value;
                    ds.DisplayStyle = displayStyle;
                }
                
            }
        }

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
        
    }
}
