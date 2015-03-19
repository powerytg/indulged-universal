using Indulged.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.UI.Xaml;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private MediaCapture captureManager = null;
        private bool isPreviewing;

        private async void InitializeCamera()
        {
            // Detect available cameras
            var hasCameras = await EnumerateCamerasAsync();
            if (!hasCameras)
            {
                LoadingView.Text = "No cameras found";
                return;
            }

            var settings = new MediaCaptureInitializationSettings();
            settings.VideoDeviceId = currentCamera.Id;

            try
            {
                if (captureManager == null)
                {
                    captureManager = new MediaCapture();
                    captureManager.Failed += new Windows.Media.Capture.MediaCaptureFailedEventHandler(Failed);
                    await captureManager.InitializeAsync(settings);

                    CameraView.Source = captureManager;
                    await captureManager.StartPreviewAsync();
                    isPreviewing = true;

                    // Initial orientation
                    rotWidth = CameraView.Height;
                    rotHeight = CameraView.Width;

                    if (currentCamera == frontCamera)
                    {
                        reversePreviewRotation = true;
                    }
                    else
                    {
                        reversePreviewRotation = false;
                    }

                    OnOrientationChanged();

                    // Get supported ev values
                    EnumerateEVValues();
                    if (!evSupported)
                    {
                        EVDialer.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        EVDialer.SupportedValues = supportedEVValues;
                    }

                    // Hide loading view
                    LoadingView.Visibility = Visibility.Collapsed;
                    CameraView.Visibility = Visibility.Visible;
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
            }

            
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
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void Failed(Windows.Media.Capture.MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
        {
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
        }

    }
}
