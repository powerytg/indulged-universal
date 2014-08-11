using Indulged.API.Utils;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonPhotoStatView : UserControl
    {
        public static readonly DependencyProperty PhotoSourceProperty = DependencyProperty.Register(
        "PhotoSource",
        typeof(FlickrPhoto),
        typeof(CommonPhotoStatView),
        new PropertyMetadata(null, OPhotoSourcePropertyChanged));

        public FlickrPhoto PhotoSource
        {
            get { return (FlickrPhoto)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        private static void OPhotoSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonPhotoStatView)sender;
            target.OnPhotoSourceChanged();
        }

        private void OnPhotoSourceChanged()
        {
            if (PhotoSource.ViewCount > 0)
            {
                ViewIcon.Visibility = Visibility.Visible;
                ViewLabel.Text = PhotoSource.ViewCount.ToShortString();
            }
            else
            {
                ViewIcon.Visibility = Visibility.Collapsed;
                ViewLabel.Text = null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPhotoStatView()
        {
            this.InitializeComponent();
        }
    }
}
