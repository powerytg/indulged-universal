using Indulged.API.Storage.Models;
using Indulged.UI.Common.PhotoStream;
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
    public sealed partial class DashboardAlbumRenderer : DashboardAlbumRendererBase
    {        
        protected override void OnAlbumChanged()
        {
            if (Album == null)
            {
                return;
            }

            ImageView.Source = new BitmapImage(new Uri(Album.PrimaryPhoto.GetImageUrl()));
            TitleLabel.Text = Album.Title.Length > 0 ? Album.Title : "Untitled";
            DateLabel.Text = "Last updated on " + Album.UpdatedDate.ToString("MMM d, yyyy");
            StatLabel.Text = Album.PhotoStream.PhotoCount.ToString();

            if (Album.Description.Length > 0 && !CommonPhotoOverlayView.IsTextInBlackList(Album.Description))
            {
                DescLabel.Visibility = Visibility.Visible;
                DescLabel.Text = Album.Description;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }
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
