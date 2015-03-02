using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.AlbumStream
{
    public partial class AlbumRendererBase : UserControl
    {
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register(
        "Album",
        typeof(FlickrAlbum),
        typeof(AlbumRendererBase),
        new PropertyMetadata(null, OAlbumPropertyChanged));

        public FlickrAlbum Album
        {
            get { return (FlickrAlbum)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        protected static void OAlbumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (AlbumRendererBase)sender;
            target.OnAlbumChanged();
        }

        protected virtual void OnAlbumChanged()
        {
            // Subclass should override this method
        }
    }
}
