using Indulged.UI.ProCam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private void InitializeEventListeners()
        {
            EVDialer.DragBegin += OnEVDialDragBegin;
            EVDialer.DragEnd += OnEVDialDragEnd;
            EVDialer.ValueChanged += OnEVDialValueChanged;
            
            ISODialer.DragBegin += OnISODialDragBegin;
            ISODialer.DragEnd += OnISODialDragEnd;
            ISODialer.ValueChanged += OnISODialValueChanged;
            
            OSD.GetWhiteBalanceOSD().WhiteBalanceChanged += OnWhiteBalanceChanged;
            OSD.GetMainOSD().GetSceneButton().Click += OnSceneButtonClick;
            OSD.GetMainOSD().GetFocusAssistButton().Click += OnFocusAssistButtonClick;
            OSD.GetSceneOSD().SceneModeChanged += OnSceneModeChanged;
            OSD.GetFocusAssistOSD().FocusAssistModeChanged += OnFocusAssistChanged;

            HUDSwitchButton.HUDStateChanged += OnOSDStateChanged;
            CameraSwitchButton.CameraChanged += OnCameraChanged;
             
        }

        private void RemoveAllEventListeners()
        {
            EVDialer.DragBegin -= OnEVDialDragBegin;
            EVDialer.DragEnd -= OnEVDialDragEnd;
            EVDialer.ValueChanged -= OnEVDialValueChanged;
            
            ISODialer.DragBegin -= OnISODialDragBegin;
            ISODialer.DragEnd -= OnISODialDragEnd;
            ISODialer.ValueChanged -= OnISODialValueChanged;
            
            OSD.GetWhiteBalanceOSD().WhiteBalanceChanged -= OnWhiteBalanceChanged;
            OSD.GetMainOSD().GetSceneButton().Click -= OnSceneButtonClick;
            OSD.GetSceneOSD().SceneModeChanged -= OnSceneModeChanged;
            OSD.GetFocusAssistOSD().FocusAssistModeChanged -= OnFocusAssistChanged;

            HUDSwitchButton.HUDStateChanged -= OnOSDStateChanged;
            CameraSwitchButton.CameraChanged -= OnCameraChanged;             
        }

        private void OnEVDialDragBegin(object sender, EventArgs e)
        {
            ShowEVHUD();
        }

        private async void OnEVDialDragEnd(object sender, EventArgs e)
        {
            DismissEVHUD();

            if (currentCamera != null && captureManager != null)
            {
                var ec = captureManager.VideoDeviceController.ExposureCompensationControl;
                try
                {
                    await ec.SetValueAsync(EVDialer.CurrentValue);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }

        }

        private void OnEVDialValueChanged(object sender, EventArgs e)
        {
            if (evHUDView == null)
            {
                return;
            }

            evHUDView.SelectedValue = EVDialer.CurrentValue;
        }

        private void OnISODialDragBegin(object sender, EventArgs e)
        {
            ShowISOHUD();
        }

        private async void OnISODialDragEnd(object sender, EventArgs e)
        {
            DismissISOHUD();

            if (currentCamera != null && captureManager != null)
            {
                var isoControl = captureManager.VideoDeviceController.IsoSpeedControl;
                if (ISODialer.CurrentValue == ProCamConstraints.PROCAM_AUTO_ISO)
                {
                    await isoControl.SetAutoAsync();
                }
                else
                {
                    await isoControl.SetValueAsync(ISODialer.CurrentValue);
                }
            }
        }

        private void OnISODialValueChanged(object sender, EventArgs e)
        {
            if (isoHUDView == null)
            {
                return;
            }

            isoHUDView.SelectedValue = ISODialer.CurrentValue;
        }

        private void OnOSDStateChanged(object sender, EventArgs e)
        {
            if (HUDSwitchButton.IsOn)
            {
                ShowOSD();
            }
            else
            {
                DismissOSD();
            }
        }

        private async void OnCameraChanged(object sender, EventArgs e)
        {
            DismissOSD();
            ShowLoadingView();
            
            DestroyCamera();

            if (CameraSwitchButton.CurrentCamera == Windows.Devices.Enumeration.Panel.Back)
            {
                currentCamera = backCamera;
            }
            else if (CameraSwitchButton.CurrentCamera == Windows.Devices.Enumeration.Panel.Front)
            {
                currentCamera = frontCamera;
            }
            else
            {
                return;
            }

            await InitializeCamera();
            InitializeChrome();
        }

        private void OnWhiteBalanceButtonClick(object sender, RoutedEventArgs e)
        {
            if (OSD.Visibility == Visibility.Collapsed)
            {
                ShowOSD(OSD.GetWhiteBalanceOSD());
            }
            else
            {
                if (OSD.CurrentOSD == OSD.GetWhiteBalanceOSD())
                {
                    DismissOSD();
                }
                else
                {
                    ShowOSD(OSD.GetWhiteBalanceOSD());
                }
            }
        }

        private void OnFlashButtonClick(object sender, RoutedEventArgs e)
        {
            if (!flashSupported)
            {
                return;
            }

            int currentIndex = flashModes.IndexOf(currentFlashMode);
            if (currentIndex == flashModes.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            currentFlashMode = flashModes[currentIndex];
            if (currentFlashMode == FlashMode.AUTO)
            {
                FlashButton.Style = (Style)App.Current.Resources["HUDButtonStyle"];
                FlashIcon.Source = flashIconAuto;
                FlashLabel.Text = "AUTO";

                captureManager.VideoDeviceController.FlashControl.Auto = true;
            }
            else if (currentFlashMode == FlashMode.ON)
            {
                FlashButton.Style = (Style)App.Current.Resources["HUDActiveButtonStyle"];
                FlashIcon.Source = flashIconOn;
                FlashLabel.Text = "ON";

                captureManager.VideoDeviceController.FlashControl.Enabled = true;
            }
            else
            {
                FlashButton.Style = (Style)App.Current.Resources["HUDButtonStyle"];
                FlashIcon.Source = flashIconOff;
                FlashLabel.Text = "OFF";

                captureManager.VideoDeviceController.FlashControl.Enabled = false;
            }

        }

        private void OnSceneButtonClick(object sender, RoutedEventArgs e)
        {
            ShowOSD(OSD.GetSceneOSD());
        }

        private void OnFocusAssistButtonClick(object sender, RoutedEventArgs e)
        {
            ShowOSD(OSD.GetFocusAssistOSD());
        }

        private async void OnWhiteBalanceChanged(object sender, EventArgs e)
        {
            DismissOSD();

            WBLabel.Text = OSD.GetWhiteBalanceOSD().WhiteBalanceStrings[OSD.GetWhiteBalanceOSD().CurrentWhiteBalanceIndex];
            var wb = supportWhiteBalances[OSD.GetWhiteBalanceOSD().CurrentWhiteBalanceIndex];
            await captureManager.VideoDeviceController.WhiteBalanceControl.SetPresetAsync(wb);
        }

        private async void OnSceneModeChanged(object sender, EventArgs e)
        {
            var sceneMode = availableSceneModes[OSD.GetSceneOSD().CurrentIndex];
            OSD.GetMainOSD().GetSceneButton().Content = sceneMode.Name;
            ShowOSD(OSD.GetMainOSD());
            
            await captureManager.VideoDeviceController.SceneModeControl.SetValueAsync(sceneMode.Mode);
        }

        private void OnFocusAssistChanged(object sender, EventArgs e)
        {
            OSD.GetMainOSD().GetFocusAssistButton().Content = OSD.GetFocusAssistOSD().ModeStrings[OSD.GetFocusAssistOSD().CurrentIndex];
            ShowOSD(OSD.GetMainOSD());

            if (focusAssistSupported)
            {
                var mode = supportedFocusAssistModes[OSD.GetFocusAssistOSD().CurrentIndex];
                if (mode == FocusAssistMode.ON)
                {
                    captureManager.VideoDeviceController.FlashControl.AssistantLightEnabled = true;
                }
                else
                {
                    captureManager.VideoDeviceController.FlashControl.AssistantLightEnabled = false;
                }                
            }
        }

        private void OnViewFinderTap(object sender, TappedRoutedEventArgs e)
        {
            if (HUDSwitchButton.IsOn)
            {
                DismissOSD();
            }
            else
            {
                if (focusSupported && focusRegionSupported && !isFocusing)
                {
                    var pt = e.GetPosition(CameraView);
                    BeginAutoFocus(new Windows.Foundation.Point(pt.X, pt.Y));
                }
            }
        }

        private void OnShutterButtonClick(object sender, RoutedEventArgs e)
        {
            CapturePhoto();
        }

        private void OnShutterHalfPress(object sender, EventArgs e)
        {
            BeginAutoFocus();
        }

        private void OnShutterFullPress(object sender, EventArgs e)
        {
            CapturePhoto();
        }

        private void OnShutterReleased(object sender, EventArgs e)
        {
            // Ignore
        }

    }
}
