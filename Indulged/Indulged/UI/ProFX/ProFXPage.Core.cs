using Indulged.UI.Common.Controls;
using Indulged.UI.Common.Controls.Events;
using Indulged.UI.ProFX.Filters;
using Lumia.Imaging;
using System;
using System.Collections.Generic;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Indulged.UI.ProFX
{
    public partial class ProFXPage
    {
        private StorageFile originalFile;
        private uint originalImageWidth;
        private uint originalImageHeight;

        // Preview image showed in the view finder
        private WriteableBitmap originalPreviewBitmap;
        private int originalPreviewBitmapWidth;
        private int originalPreviewBitmapHeight;

        // Preview in-memory stream
        private InMemoryRandomAccessStream previewStream;
        private WriteableBitmap currentPreviewBitmap;

        // Filter manager
        private FXFilterManager filterManager;

        // Sample the original image
        private async void SampleOriginalImage()
        {
            ImageProperties properties = await originalFile.Properties.GetImagePropertiesAsync();
            originalImageWidth = properties.Width;
            originalImageHeight = properties.Height;
            uint maxOutputWidth = (uint)Window.Current.Bounds.Width;
            uint maxOutputHeight = (uint)Window.Current.Bounds.Height;
            float ratio = (float)originalImageWidth / (float)originalImageHeight;
            uint previewWidth;
            uint previewHeight;

            using (IRandomAccessStream stream = await originalFile.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Create output stream
                previewStream = new InMemoryRandomAccessStream();
                BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(previewStream, decoder);

                // Resize image
                if (maxOutputWidth > maxOutputHeight * ratio)
                {
                    previewWidth = (uint)Math.Floor(maxOutputHeight * ratio);
                    previewHeight = maxOutputHeight;
                }
                else
                {
                    previewWidth = maxOutputWidth;
                    previewHeight = (uint)Math.Floor(maxOutputWidth / ratio);
                }

                var transform = new BitmapTransform() { ScaledHeight = previewHeight, ScaledWidth = previewWidth };
                PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Ignore,
                    transform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.DoNotColorManage);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Ignore, previewWidth, previewHeight, 96.0, 96.0, pixelData.DetachPixelData());

                // Output a scaled version
                await encoder.FlushAsync();
            }

            // Save the original preview image and its stream
            originalPreviewBitmapWidth = (int)previewWidth;
            originalPreviewBitmapHeight = (int)previewHeight;
            originalPreviewBitmap = new WriteableBitmap(originalPreviewBitmapWidth, originalPreviewBitmapHeight);
            originalPreviewBitmap.SetSource(previewStream);

            // Create a preview image for editing
            currentPreviewBitmap = new WriteableBitmap(originalPreviewBitmapWidth, originalPreviewBitmapHeight);
            ViewFinder.Source = originalPreviewBitmap;
        }

        private async void UpdatePreviewAsync()
        {
            // Must rewind the stream in order for filters to work
            previewStream.Seek(0);
            using (var source = new RandomAccessStreamImageSource(previewStream))
            {
                using (var effects = new FilterEffect(source))
                {
                    List<IFilter> appliedFilters = new List<IFilter>();
                    foreach (var filterContainer in filterManager.AppliedFilters)
                    {
                        if (filterContainer.IsFilterEnabled)
                        {
                            appliedFilters.Add(filterContainer.Filter);
                        }
                    }

                    effects.Filters = appliedFilters.ToArray();

                    using (var renderer = new WriteableBitmapRenderer(effects, currentPreviewBitmap))
                    {
                        await renderer.RenderAsync();
                        currentPreviewBitmap.Invalidate();

                        if (ViewFinder.Source != currentPreviewBitmap)
                        {
                            ViewFinder.Source = currentPreviewBitmap;
                        }                        
                    }
                }
            }
        }
        
        private void PerformAutoEnhance()
        {
            if (filterManager.HasAppliedFilterOtherThan(FilterCategory.Transform))
            {
                var dialog = ModalPopup.Show("This will clear all your previous applied filters. Do you wish to continue?",
                    "Auto Enhance", new List<string> { "Confirm", "Cancel" });
                dialog.DismissWithButtonClick += (s, args) =>
                {
                    int buttonIndex = (args as ModalPopupEventArgs).ButtonIndex;
                    if (buttonIndex == 0)
                    {
                        AddAutoEnhanceFilters();
                    }
                };
            }
            else
            {
                AddAutoEnhanceFilters();
            }
        }

        private void AddAutoEnhanceFilters()
        {
            filterManager.ClearAllFiltersOtherThan(FilterCategory.Transform);

            foreach (var filter in filterManager.AutoEnhanceFilters)
            {
                filter.CurrentImage = currentPreviewBitmap;
                filter.OriginalImageWidth = originalImageWidth;
                filter.OriginalImageHeight = originalImageHeight;
                filter.OriginalPreviewImageWidth = originalPreviewBitmapWidth;
                filter.OriginalPreviewImageHeight = originalPreviewBitmapHeight;
            }

            filterManager.AutoEnhance();
        }
    
    }
}
