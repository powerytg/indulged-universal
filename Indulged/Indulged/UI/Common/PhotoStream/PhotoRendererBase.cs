using Indulged.API.Storage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoRendererBase : UserControl
    {
        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register(
        "Photo",
        typeof(FlickrPhoto),
        typeof(PhotoRendererBase),
        new PropertyMetadata(null, OnPhotoPropertyChanged));

        public FlickrPhoto Photo
        {
            get { return (FlickrPhoto)GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }

        private static void OnPhotoPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PhotoRendererBase)sender;
            target.OnPhotoChanged();
        }

        protected virtual void OnPhotoChanged()
        {
        }
    }
}
