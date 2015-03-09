using Indulged.API.Networking;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Detail.Sections
{
    public sealed partial class ReviewsSection : DetailSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsSection()
        {
            this.InitializeComponent();
        }

        public override void AddEventListeners()
        {
            base.AddEventListeners();
            StorageService.Instance.PhotoCommentsUpdated += OnCommentsUpdated;
            StorageService.Instance.CommentAdded += OnCommentsUpdated;
        }

        public override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
            StorageService.Instance.PhotoCommentsUpdated -= OnCommentsUpdated;
            StorageService.Instance.CommentAdded -= OnCommentsUpdated;
        }

        private void OnCommentsUpdated(object sender, API.Storage.Events.StorageEventArgs e)
        {
            if (Photo == null || e.PhotoId != Photo.ResourceId)
            {
                return;
            }

            UpdateItemRenderers();
        }

        protected override async void OnPhotoChanged()
        {
            base.OnPhotoChanged();

            if (Photo == null)
            {
                return;
            }

            // Always refresh comments
            var status = await APIService.Instance.GetPhotoCommentsAsync(Photo.ResourceId);
            if (!status.Success)
            {
                LoadingView.Text = "Cannot load comments";
            }
        }

        private void UpdateItemRenderers()
        {
            if (Photo.Comments.Count == 0)
            {
                LoadingView.Text = "This photo has not received comments yet";
            }
            else
            {
                LoadingView.Visibility = Visibility.Collapsed;

                if (Photo.Comments.Count >= 1)
                {
                    renderer1.Visibility = Visibility.Visible;
                    renderer1.Comment = Photo.Comments[0];
                }

                if (Photo.Comments.Count >= 2)
                {
                    renderer2.Visibility = Visibility.Visible;
                    renderer2.Comment = Photo.Comments[1];
                }

                if (Photo.Comments.Count >= 3)
                {
                    renderer3.Visibility = Visibility.Visible;
                    renderer3.Comment = Photo.Comments[0];
                }

                ViewAllButton.Visibility = Visibility.Visible;
            }

        }

        private void ViewAllButon_Click(object sender, RoutedEventArgs e)
        {

        }
     

    }
}
