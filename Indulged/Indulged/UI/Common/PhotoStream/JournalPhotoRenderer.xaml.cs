using Indulged.API.Storage;
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

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class JournalPhotoRenderer : PhotoRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JournalPhotoRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnPhotoSourceChanged()
        {
            base.OnPhotoSourceChanged();

            if (PhotoSource == null)
            {
                return;
            }

            ImageView.Source = new BitmapImage(new Uri(PhotoSource.GetImageUrl(), UriKind.Absolute));

            if (StorageService.Instance.UserCache.ContainsKey(PhotoSource.UserId))
            {
                AuthorLabel.Text = StorageService.Instance.UserCache[PhotoSource.UserId].Name.ToUpper();
            }

            if (PhotoSource.Title.Length == 0)
            {
                TitleLabel.Text = "Untitled";
            }
            else
            {
                TitleLabel.Text = PhotoSource.Title;
            }            

            if (PhotoSource.Description.Length > 0 && !CommonPhotoOverlayView.IsTextInBlackList(PhotoSource.Description))
            {
                DescPanel.Visibility = Visibility.Visible;
                DescLabel.Text = PhotoSource.Description;
            }
            else
            {
                DescPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void PhotoRendererBase_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            bool result = VisualStateManager.GoToState(this, "Pressed", false);
        }

        private void PhotoRendererBase_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void PhotoRendererBase_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void PhotoRendererBase_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        
    }
}
