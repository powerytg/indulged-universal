using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoTileRendererBase : UserControl
    {
        public static readonly DependencyProperty PhotoTileSourceProperty = DependencyProperty.Register(
        "PhotoTileSource",
        typeof(PhotoTile),
        typeof(PhotoTileRendererBase),
        new PropertyMetadata(null, OPhotoTilePropertyChanged));

        public PhotoTile PhotoTileSource
        {
            get { return (PhotoTile)GetValue(PhotoTileSourceProperty); }
            set { SetValue(PhotoTileSourceProperty, value); }
        }

        private static void OPhotoTilePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PhotoTileRendererBase)sender;
            target.OnPhotoTileChanged();
        }

        protected virtual void OnPhotoTileChanged()
        {
        }
    }
}
