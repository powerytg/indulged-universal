using Indulged.API.Storage.Models;
using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.Controls
{
    public sealed partial class BlurredBackgroundView : UserControl
    {
        public static readonly DependencyProperty PhotoSourceProperty = DependencyProperty.Register(
        "PhotoSource",
        typeof(FlickrPhoto),
        typeof(BlurredBackgroundView),
        new PropertyMetadata(null, OnPhotoSourcePropertyChanged));

        public FlickrPhoto PhotoSource
        {
            get { return (FlickrPhoto)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        private static void OnPhotoSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (BlurredBackgroundView)sender;
            target.OnPhotoSourceChanged();
        }

        private void OnPhotoSourceChanged()
        {
            // Fade out the old image
            Storyboard animation = new Storyboard();
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            animation.Children.Add(fadeOutAnimation);
            fadeOutAnimation.Duration = animation.Duration;
            fadeOutAnimation.To = 0;
            Storyboard.SetTarget(fadeOutAnimation, ImageView);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            animation.Completed += FadeOutAnimationCompleted;
            animation.Begin();
        }

        public int SampleWidth { get; set; }
        public int SampleHeight { get; set; }

        private WriteableBitmap outputBitmap;

        /// <summary>
        /// Constructor
        /// </summary>
        public BlurredBackgroundView()
        {
            this.InitializeComponent();

            SampleWidth = (int) Window.Current.Bounds.Width;
            SampleHeight = (int) Window.Current.Bounds.Height;
        }

        private async void FadeOutAnimationCompleted(Object storyboard, Object e)
        {
            await DownloadAndApplyFilterAsync();
            FadeInNewImage();
        }

        private async Task DownloadAndApplyFilterAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(PhotoSource.GetImageUrl());
            InMemoryRandomAccessStream randomAccess = new InMemoryRandomAccessStream();
            DataWriter writer = new DataWriter(randomAccess.GetOutputStreamAt(0));
            writer.WriteBytes(await response.Content.ReadAsByteArrayAsync());
            await writer.StoreAsync();

            // Create approtiate size of output image
            var decoder = await BitmapDecoder.CreateAsync(randomAccess);
            var frame = await decoder.GetFrameAsync(0);
            outputBitmap = new WriteableBitmap((int)frame.PixelWidth, (int)frame.PixelHeight);
            

            using (var source = new RandomAccessStreamImageSource(randomAccess))
            {
                using (var filter = new LensBlurEffect(source, new LensBlurPredefinedKernel(LensBlurPredefinedKernelShape.Circle, 64)))
                {
                    using (var renderer = new WriteableBitmapRenderer(filter, outputBitmap))
                    {
                        outputBitmap = await renderer.RenderAsync();
                        outputBitmap.Invalidate();
                        ImageView.Source = outputBitmap;
                    }
                }
            }
        }

        private void FadeInNewImage()
        {
            Storyboard animation = new Storyboard();
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            animation.Children.Add(fadeInAnimation);
            fadeInAnimation.Duration = animation.Duration;
            fadeInAnimation.To = 0.3;
            Storyboard.SetTarget(fadeInAnimation, ImageView);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");
            animation.Begin();
        }
    }
}
