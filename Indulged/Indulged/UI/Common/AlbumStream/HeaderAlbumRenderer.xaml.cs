using Indulged.UI.Common.PhotoStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.AlbumStream
{
    public sealed partial class HeaderAlbumRenderer : AlbumRendererBase
    {
        private double overlayThreshold = 60;

        /// <summary>
        /// Constructor
        /// </summary>
        public HeaderAlbumRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnAlbumChanged()
        {
            base.OnAlbumChanged();

            if (Album == null)
            {
                return;
            }

            ImageView.Source = new BitmapImage(new Uri(Album.PrimaryPhoto.GetImageUrl(), UriKind.Absolute));

            if (Album.Title.Length == 0)
            {
                TitleLabel.Text = "Last Updated";
            }
            else
            {
                TitleLabel.Text = Album.Title;
            }

            if (Album.Description.Length > 0 && !CommonPhotoOverlayView.IsTextInBlackList(Album.Description))
            {
                DescPanel.Visibility = Visibility.Visible;
                DescLabel.Text = Album.Description;
            }
            else
            {
                DescPanel.Visibility = Visibility.Collapsed;
            }

            UpdateGridLayout();
        }

        private void UpdateGridLayout()
        {
            if (DescPanel.Visibility == Visibility.Collapsed)
            {
                LayoutTextOnOverImage();
                return;
            }

            TitleLabel.Measure(new Size(0, 0));
            DescLabel.Measure(new Size(0, 0));
            
            // Measure text blocks to determine whether the text should be rendered below the image
            double textHeight = TitleLabel.ActualHeight + DescLabel.ActualHeight;
            if (textHeight < overlayThreshold)
            {
                LayoutTextOnOverImage();
            }
            else
            {
                LayoutTextBelowImage();
            }
        }

        private void LayoutTextOnOverImage()
        {
            LayoutRoot.RowDefinitions.Clear();

            TextPanel.Background = new SolidColorBrush(Color.FromArgb(0x99, 0, 0, 0));
            TextPanel.VerticalAlignment = VerticalAlignment.Bottom;

            // Left align title if description is missing
            if (DescPanel.Visibility == Visibility.Collapsed)
            {
                TitleLabel.TextAlignment = TextAlignment.Left;
            }
            else
            {
                TitleLabel.TextAlignment = TextAlignment.Center;
            }
            
        }

        private void LayoutTextBelowImage()
        {
            LayoutRoot.RowDefinitions.Clear();
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            ImageView.SetValue(Grid.RowProperty, 0);
            TextPanel.SetValue(Grid.RowProperty, 1);
            TextPanel.ClearValue(FrameworkElement.VerticalAlignmentProperty);
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
