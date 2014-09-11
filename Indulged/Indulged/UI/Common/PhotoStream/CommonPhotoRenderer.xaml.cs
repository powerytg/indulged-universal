using Indulged.PolKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class CommonPhotoRenderer : PhotoRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPhotoRenderer()
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
            OverlayView.PhotoSource = PhotoSource;
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

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PhotoSource == null)
            {
                return;
            }

            if (!PolicyKit.Instance.ShowOverlayOnPreludeTiles)
            {
                OverlayView.Visibility = Visibility.Collapsed;
            }
            else
            {
                PerformLayout();
                OverlayView.Visibility = Visibility.Visible;
            }
        }

        private void PerformLayout()
        {
            double screenSize = Window.Current.Bounds.Width;

            LayoutRoot.RowDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Clear();
            OverlayView.ClearValue(FrameworkElement.MaxWidthProperty);
            OverlayView.ClearValue(FrameworkElement.MaxHeightProperty);
            OverlayView.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            OverlayView.ClearValue(FrameworkElement.VerticalAlignmentProperty);

            int textLength = PhotoSource.Title.Length + PhotoSource.Description.Length;

            if (textLength <= 100 || ActualWidth < screenSize * 0.4)
            {
                LayoutOverlayInMiniMode();
            }
            else if (textLength > 100 && textLength < 250)
            {
                if (CommonPhotoOverlayView.IsTextInBlackList(PhotoSource.Description))
                {
                    LayoutOverlayInMiniMode();
                }
                else
                {
                    if (LayoutRoot.ActualWidth > screenSize - 20)
                    {
                        LayoutOverlayInOverlayModeHorizontally();
                    }
                    else
                    {
                        LayoutOverlayInOverlayModeVertically();
                    }
                }    
            }
            else
            {
                if (CommonPhotoOverlayView.IsTextInBlackList(PhotoSource.Description))
                {
                    LayoutOverlayInOverlayModeVertically();
                }
                else if (LayoutRoot.ActualWidth > screenSize - 20)
                {
                    LayoutOverlayInFullMode();
                }
                else
                {
                    LayoutOverlayInOverlayModeVertically();
                }
            }
        }

        private void LayoutOverlayInMiniMode()
        {
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Mini;
            OverlayView.HorizontalAlignment = HorizontalAlignment.Left;
            OverlayView.VerticalAlignment = VerticalAlignment.Bottom;
        }

        private void LayoutOverlayInOverlayModeHorizontally()
        {
            double screenSize = Window.Current.Bounds.Width;
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Overlay;
            OverlayView.HorizontalAlignment = HorizontalAlignment.Right;
            OverlayView.MaxWidth = screenSize * 0.4;
            OverlayView.MaxHeight = ImageView.ActualHeight;
        }

        private void LayoutOverlayInOverlayModeVertically()
        {
            double screenSize = Window.Current.Bounds.Width;
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Overlay;
            OverlayView.HorizontalAlignment = HorizontalAlignment.Stretch;
            OverlayView.VerticalAlignment = VerticalAlignment.Bottom;
            OverlayView.MaxHeight = ImageView.ActualHeight * 0.4;
        }

        private void LayoutOverlayInFullMode()
        {
            OverlayView.LayoutMode = CommonPhotoOverlayView.PhotoOverlayLayoutMode.Full;
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition() { MaxHeight = 300 });
            
            ImageView.SetValue(Grid.RowProperty, 0);
            OverlayView.SetValue(Grid.RowProperty, 1);
        }

    }
}
