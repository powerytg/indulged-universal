using Indulged.API.Storage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Detail.Sections
{
    public class DetailSectionBase : UserControl
    {
        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register(
        "Photo",
        typeof(FlickrPhoto),
        typeof(DetailSectionBase),
        new PropertyMetadata(null, OnPhotoPropertyChanged));

        public FlickrPhoto Photo
        {
            get { return (FlickrPhoto)GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }

        private static void OnPhotoPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (DetailSectionBase)sender;
            target.OnPhotoChanged();
        }

        protected virtual void OnPhotoChanged()
        {
        }
    }
}
