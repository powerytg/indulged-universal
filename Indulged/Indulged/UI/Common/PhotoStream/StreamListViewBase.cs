using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public partial class StreamListViewBase : ListView
    {
        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        public static readonly DependencyProperty StreamProperty = DependencyProperty.Register(
        "Stream",
        typeof(FlickrPhotoStream),
        typeof(StreamListViewBase),
        new PropertyMetadata(null, OStreamPropertyChanged));

        public FlickrPhotoStream Stream
        {
            get { return (FlickrPhotoStream)GetValue(StreamProperty); }
            set { SetValue(StreamProperty, value); }
        }

        private static void OStreamPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (StreamListViewBase)sender;
            target.OnStreamChanged();
        }

        protected virtual void OnStreamChanged()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamListViewBase()
            : base()
        {

        }
    }
}
