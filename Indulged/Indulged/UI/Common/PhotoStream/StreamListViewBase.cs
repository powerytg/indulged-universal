using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
using System;
using Windows.UI.Xaml;

namespace Indulged.UI.Common.PhotoStream
{
    public partial class StreamListViewBase : MagazineView
    {
        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        public static readonly DependencyProperty StreamProperty = DependencyProperty.Register(
        "Stream",
        typeof(FlickrPhotoStream),
        typeof(StreamListViewBase),
        new PropertyMetadata(null, OnStreamPropertyChanged));

        public FlickrPhotoStream Stream
        {
            get { return (FlickrPhotoStream)GetValue(StreamProperty); }
            set { SetValue(StreamProperty, value); }
        }

        private static void OnStreamPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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
