using Indulged.API.Storage.Models;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Dashboard.Renderers
{
    public sealed partial class DashboardAlbumRenderer : UserControl
    {
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register(
        "Album",
        typeof(FlickrAlbum),
        typeof(DashboardAlbumRenderer),
        new PropertyMetadata(null, OAlbumPropertyChanged));

        public FlickrAlbum Album
        {
            get { return (FlickrAlbum)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        private static void OAlbumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (DashboardAlbumRenderer)sender;
            target.OnAlbumChanged();
        }

        private void OnAlbumChanged()
        {
            ImageView.Source = new BitmapImage(new Uri(Album.PrimaryPhoto.GetImageUrl()));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardAlbumRenderer()
        {
            this.InitializeComponent();
        }
    }
}
