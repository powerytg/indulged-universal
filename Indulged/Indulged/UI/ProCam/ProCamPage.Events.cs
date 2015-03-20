using Indulged.UI.ProCam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            /*
            OSD.WhiteBalanceOSD.WhiteBalanceChanged += OnWhiteBalanceChanged;
            OSD.MainOSD.SceneButton.Click += OnSceneButtonClick;
            OSD.SceneOSD.SceneModeChanged += OnSceneModeChanged;

            OSD.MainOSD.FocusAssistButton.Click += OnFocusAssistButtonClick;
            OSD.FocusAssistOSD.FocusAssistModeChanged += OnFocusAssistModeChanged;

            OSD.MainOSD.ResolutionChanged += OnResolutionChanged;

            HUDSwitchButton.HUDStateChanged += OnOSDStateChanged;
            CameraSwitchButton.CameraChanged += OnCameraChanged;
             */
        }

        private void RemoveAllEventListeners()
        {
            EVDialer.DragBegin -= OnEVDialDragBegin;
            EVDialer.DragEnd -= OnEVDialDragEnd;
            EVDialer.ValueChanged -= OnEVDialValueChanged;
            
            ISODialer.DragBegin -= OnISODialDragBegin;
            ISODialer.DragEnd -= OnISODialDragEnd;
            ISODialer.ValueChanged -= OnISODialValueChanged;
            /*
            OSD.WhiteBalanceOSD.WhiteBalanceChanged -= OnWhiteBalanceChanged;
            OSD.MainOSD.SceneButton.Click -= OnSceneButtonClick;
            OSD.SceneOSD.SceneModeChanged -= OnSceneModeChanged;

            OSD.MainOSD.FocusAssistButton.Click -= OnFocusAssistButtonClick;
            OSD.FocusAssistOSD.FocusAssistModeChanged -= OnFocusAssistModeChanged;

            OSD.MainOSD.ResolutionChanged -= OnResolutionChanged;

            HUDSwitchButton.HUDStateChanged -= OnOSDStateChanged;
            CameraSwitchButton.CameraChanged -= OnCameraChanged;
             */
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

    }
}
