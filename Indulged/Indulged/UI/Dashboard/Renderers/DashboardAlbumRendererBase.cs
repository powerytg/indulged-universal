using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Dashboard.Renderers
{
    public partial class DashboardAlbumRendererBase : UserControl
    {
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register(
        "Album",
        typeof(FlickrAlbum),
        typeof(DashboardAlbumRendererBase),
        new PropertyMetadata(null, OAlbumPropertyChanged));

        public FlickrAlbum Album
        {
            get { return (FlickrAlbum)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        protected static void OAlbumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (DashboardAlbumRendererBase)sender;
            target.OnAlbumChanged();
        }

        protected virtual void OnAlbumChanged()
        {
            // Subclass should override this method
        }
    }
}
