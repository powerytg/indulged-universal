using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private MediaCapture captureManager = null;
        private bool isPreviewing;
        private bool isFocusing;        

        private async Task<bool> InitializeCamera()
        {
            bool retVal = true;
            var settings = new MediaCaptureInitializationSettings();
            settings.VideoDeviceId = currentCamera.Id;

            try
            {
                if (captureManager == null)
                {
                    isFocusing = false;

                    captureManager = new MediaCapture();
                    captureManager.Failed += new Windows.Media.Capture.MediaCaptureFailedEventHandler(Failed);
                    await captureManager.InitializeAsync(settings);

                    CameraView.Source = captureManager;
                    await captureManager.StartPreviewAsync();
                    isPreviewing = true;

                    if (currentCamera == frontCamera)
                    {
                        reversePreviewRotation = true;
                        CameraView.FlowDirection = FlowDirection.RightToLeft;
                    }
                    else
                    {
                        reversePreviewRotation = false;
                        CameraView.FlowDirection = FlowDirection.LeftToRight;
                    }

                    OnOrientationChanged();

                    // Initialize to max resolution
                    EnumerateResolutions();
                    if (supportedResolutions.Count > 0)
                    {
                        currentResolution = supportedResolutions[0];
                        await captureManager.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, currentResolution);
                    }
                    else
                    {
                        currentResolution = null;
                    }
                    
                    // Check focus support and configure focus mode
                    CheckFocusSupport();
                    if (focusSupported)
                    {
                        if (autoFocusSupported)
                        {
                            var focusSetting = new FocusSettings { Mode = FocusMode.Auto, AutoFocusRange = AutoFocusRange.Normal };
                            captureManager.VideoDeviceController.FocusControl.Configure(focusSetting);
                        }
                        else
                        {
                            var focusSetting = new FocusSettings { Mode = FocusMode.Single, AutoFocusRange = AutoFocusRange.FullRange };
                            captureManager.VideoDeviceController.FocusControl.Configure(focusSetting);
                        }
                    }

                    // Flash
                    CheckFlashSupport();
                    
                    // Focus assist
                    CheckFocusAssistSupport();

                    // Get supported capabilities
                    EnumerateEVValues();
                    EnumerateISOValues();
                    EnumerateSceneMode();
                    EnumerateWhiteBalances();
                    retVal = true;
                }
            }
            catch (Exception e)
            {
                isPreviewing = false;
                CameraView.Source = null;
                captureManager = null;

                // Show error message
                LoadingView.Visibility = Visibility.Visible;
                LoadingView.Text = e.Message;
                retVal = false;
            }

            return retVal;
        }

        private async void DestroyCamera()
        {
            try
            {
                if (isPreviewing && captureManager != null)
                {
                    await captureManager.StopPreviewAsync();
                    CameraView.Source = null;
                    captureManager.Dispose();
                    captureManager = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void Failed(Windows.Media.Capture.MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
        {
            /*
            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    // Show error to user
                    ModalPopup.Show(currentFailure.Message, "PearlCam", new List<string> {"Confirm"}, true);
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            */

            Debug.WriteLine(currentFailure.Message);
        }

        #region Shutter and focus

        private async void PerformFocus(Windows.Foundation.Point? point = null)
        {
            if (point != null)
            {
                // Unify point to be of 0..1
                var x = point.Value.X / CameraView.ActualWidth;
                var y = point.Value.Y / CameraView.ActualHeight;
                double epsilon = 0.01;

                // 'x + width' and 'y + height' should be less than 1.0.
                if (x >= 1.0 - epsilon)
                {
                    x = 1.0 - 2 * epsilon;
                }

                if (y >= 1.0 - 0.01)
                {
                    y = 1.0 - 2 * epsilon;
                }

                var region = new RegionOfInterest
                {
                    Type = RegionOfInterestType.Unknown,
                    Bounds = new Windows.Foundation.Rect(x, y, epsilon, epsilon),
                    BoundsNormalized = true,
                    AutoFocusEnabled = true,
                    Weight = 1
                };

                await captureManager.VideoDeviceController.RegionsOfInterestControl.SetRegionsAsync(new[] {region});
                await captureManager.VideoDeviceController.FocusControl.FocusAsync();
            }
            else
            {
                await captureManager.VideoDeviceController.RegionsOfInterestControl.ClearRegionsAsync();
            }

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OnFocusLocked();
            });
        }

        private void BeginAutoFocus(Windows.Foundation.Point? point = null)
        {
            isFocusing = true;

            if (point != null)
            {
               PerformFocusAnimation(point);
            }

            PerformFocus(point);
        }

        private void OnFocusLocked()
        {
            if (focusBinkAnimation != null)
            {
                focusBinkAnimation.Stop();
                focusBinkAnimation = null;
            }

            PerformFocusLockedAnimation();

            isFocusing = false;
        }

        #endregion

        private async void CapturePhoto()
        {
            var photoLibraryFolder = KnownFolders.CameraRoll;
            StorageFile file = await photoLibraryFolder.CreateFileAsync("IMG_INDULGED.jpg", CreationCollisionOption.GenerateUniqueName);
            
            InMemoryRandomAccessStream inputStream = null;
            IRandomAccessStream outputStream = null;
            try
            {
                inputStream = new InMemoryRandomAccessStream();
                var rotation = GetCurrentPhotoRotation();
                await captureManager.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), inputStream);
                inputStream.Seek(0);

                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(inputStream);
                outputStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                outputStream.Size = 0;

                var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateForTranscodingAsync(outputStream, decoder);

                var properties = new Windows.Graphics.Imaging.BitmapPropertySet();
                properties.Add("System.Photo.Orientation",
                    new Windows.Graphics.Imaging.BitmapTypedValue(rotation, Windows.Foundation.PropertyType.UInt16));

                await encoder.BitmapProperties.SetPropertiesAsync(properties);
                await encoder.FlushAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                if (inputStream != null)
                {
                    inputStream.Dispose();
                }

                if (outputStream != null)
                {
                    outputStream.Dispose();
                }
            }

            //await captureManager.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);
            
            // Post processing
            /*
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
            
            });
            */
        }

    }
}
