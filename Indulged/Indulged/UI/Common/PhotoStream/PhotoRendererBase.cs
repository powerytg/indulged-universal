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
    public class PhotoRendererBase : UserControl
    {
        public static readonly DependencyProperty PhotoSourceProperty = DependencyProperty.Register(
        "PhotoSource",
        typeof(FlickrPhoto),
        typeof(PhotoRendererBase),
        new PropertyMetadata(null, OPhotoSourcePropertyChanged));

        public FlickrPhoto PhotoSource
        {
            get { return (FlickrPhoto)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        private static void OPhotoSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PhotoRendererBase)sender;
            target.OnPhotoSourceChanged();
        }

        protected virtual void OnPhotoSourceChanged()
        {
        }
    }
}
