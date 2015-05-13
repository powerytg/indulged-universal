using Indulged.UI.ProFX.Events;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Controls
{
    public sealed partial class ViewFinderControl : UserControl
    {
        // Events
        public EventHandler<CropAreaChangedEventArgs> CropAreaChanged;

        // Crop area
        private Rect cropRect = new Rect();

        // Constructor
        public ViewFinderControl()
        {
            InitializeComponent();

            // Events
            Handle.ManipulationDelta += OnHandleDrag;
            HighlightBox.ManipulationDelta += OnViewfinderDrag;
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ViewFinderControl), new PropertyMetadata(null, OnSourcePropertyChanged));

        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ViewFinderControl)sender).OnSourceChanged();
        }

        private void OnSourceChanged()
        {
            ImageView.Source = Source;
        }

        public void ShowCropFinder()
        {
            Curtain.Visibility = Visibility.Visible;

            WriteableBitmap bmp = (WriteableBitmap)Source;

            if (cropRect.Width == 0 || cropRect.Height == 0)
            {
                double w = bmp.PixelWidth / 2;
                double h = bmp.PixelHeight / 2;

                HighlightBox.Width = w;
                HighlightBox.Height = h;
            }
            else
            {
                HighlightBox.Width = cropRect.Width;
                HighlightBox.Height = cropRect.Height;
            }

            HighlightBox.SetValue(Canvas.LeftProperty, CropCanvas.ActualWidth / 2 - HighlightBox.Width / 2);
            HighlightBox.SetValue(Canvas.TopProperty, CropCanvas.ActualHeight / 2 - HighlightBox.Height / 2);
            HighlightBox.Visibility = Visibility.Visible;

            // Handles
            Handle.Visibility = Visibility.Visible;
            PositionHandlesAroundViewfinder();

            // Events
            BroadcastCropAreaChangeEvent();
        }

        public void ResetCropArea()
        {
            cropRect = new Rect();
        }

        private void BroadcastCropAreaChangeEvent()
        {
            double viewfinderLeft = (double)HighlightBox.GetValue(Canvas.LeftProperty);
            double viewfinderTop = (double)HighlightBox.GetValue(Canvas.TopProperty);

            if (CropAreaChanged != null)
            {
                var evt = new CropAreaChangedEventArgs();
                evt.X = viewfinderLeft;
                evt.Y = viewfinderTop;
                evt.Width = HighlightBox.Width;
                evt.Height = HighlightBox.Height;

                CropAreaChanged(this, evt);
            }
        }

        public void DismissCropFinder()
        {
            Curtain.Visibility = Visibility.Collapsed;
            HighlightBox.Visibility = Visibility.Collapsed;

            // Handles
            Handle.Visibility = Visibility.Collapsed;
        }

        private void PositionHandlesAroundViewfinder()
        {
            double viewfinderLeft = (double)HighlightBox.GetValue(Canvas.LeftProperty);
            double viewfinderTop = (double)HighlightBox.GetValue(Canvas.TopProperty);

            Handle.SetValue(Canvas.LeftProperty, viewfinderLeft + HighlightBox.Width - Handle.Width / 2);
            Handle.SetValue(Canvas.TopProperty, viewfinderTop + HighlightBox.Height - Handle.Height / 2);
        }

        private void OnHandleDrag(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double left = (double)Handle.GetValue(Canvas.LeftProperty);
            double top = (double)Handle.GetValue(Canvas.TopProperty);

            double viewfinderLeft = (double)HighlightBox.GetValue(Canvas.LeftProperty);
            double viewfinderTop = (double)HighlightBox.GetValue(Canvas.TopProperty);

            double minWidth = 20;
            double minHeight = 20;
            double newWidth = HighlightBox.Width + e.Cumulative.Translation.X;
            double newHeight = HighlightBox.Height + e.Cumulative.Translation.Y;

            left += e.Cumulative.Translation.X;
            if (Math.Abs(left - viewfinderLeft) <= minWidth)
            {
                left = viewfinderLeft + minWidth;
                newWidth = minWidth;
            }

            if (left >= Curtain.ActualWidth - Handle.Width / 2)
            {
                left = Curtain.ActualWidth - Handle.Width / 2;
                newWidth = Math.Min(HighlightBox.Width, newWidth);
            }

            top += e.Cumulative.Translation.Y;
            if (Math.Abs(top - viewfinderTop) <= minHeight)
            {
                top = viewfinderTop + minHeight;
                newHeight = minHeight;
            }

            if (top >= Curtain.ActualHeight - Handle.Height / 2)
            {
                top = Curtain.ActualHeight - Handle.Height / 2;
                newHeight = Math.Min(HighlightBox.Height, newHeight);
            }

            Handle.SetValue(Canvas.LeftProperty, left);
            Handle.SetValue(Canvas.TopProperty, top);

            HighlightBox.Width = newWidth;
            HighlightBox.Height = newHeight;

            // Events
            BroadcastCropAreaChangeEvent();
        }

        private void OnViewfinderDrag(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double viewfinderLeft = (double)HighlightBox.GetValue(Canvas.LeftProperty);
            double viewfinderTop = (double)HighlightBox.GetValue(Canvas.TopProperty);

            double newViewfinderLeft = viewfinderLeft + e.Cumulative.Translation.X;
            double newViewfinderTop = viewfinderTop + e.Cumulative.Translation.Y;
            if (newViewfinderLeft < 0)
                newViewfinderLeft = 0;

            if (newViewfinderLeft > Curtain.ActualWidth - HighlightBox.Width)
                newViewfinderLeft = Curtain.ActualWidth - HighlightBox.Width;

            if (newViewfinderTop < 0)
                newViewfinderTop = 0;

            if (newViewfinderTop > Curtain.ActualHeight - HighlightBox.Height)
                newViewfinderTop = Curtain.ActualHeight - HighlightBox.Height;

            HighlightBox.SetValue(Canvas.LeftProperty, newViewfinderLeft);
            HighlightBox.SetValue(Canvas.TopProperty, newViewfinderTop);

            // Snap handle
            PositionHandlesAroundViewfinder();

            // Events
            BroadcastCropAreaChangeEvent();
        }
    }
}
