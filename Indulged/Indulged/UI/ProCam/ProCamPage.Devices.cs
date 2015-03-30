using Indulged.UI.ProCam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private DeviceInformationCollection availableCameras;
        private DeviceInformation frontCamera;
        private DeviceInformation backCamera;
        private DeviceInformation currentCamera;
        private VideoEncodingProperties currentResolution;

        private List<SceneMode> availableSceneModes;
        private List<float> supportedEVValues;
        private bool evSupported;

        private List<uint> _supportedISOFixtures = new List<uint> { 100, 125, 160, 200, 250, 320, 400, 500, 640, 800, 1000, 1250, 1600, 2000, 2500, 3200, 4000, 5000, 6400, 12800, 25600 };
        private List<uint> supportedISOValues;
        private bool isoSupported;

        private List<VideoEncodingProperties> supportedResolutions = new List<VideoEncodingProperties>();

        private bool wbSupported;
        private List<ColorTemperaturePreset> supportWhiteBalances = new List<ColorTemperaturePreset>();

        private bool flashSupported;
        private List<FlashMode> flashModes = new List<FlashMode> { FlashMode.AUTO, FlashMode.ON, FlashMode.OFF };
        private FlashMode currentFlashMode = FlashMode.AUTO;

        private List<FocusAssistMode> supportedFocusAssistModes = new List<FocusAssistMode>();
        private bool focusAssistSupported;
        private bool focusSupported;
        private bool autoFocusSupported;
        private bool focusRegionSupported;

        private async Task<bool> EnumerateCamerasAsync()
        {
            availableCameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            if (availableCameras.Count == 0)
            {
                Debug.WriteLine("No camera found");
                return false;
            }

            for (int i = 0; i < availableCameras.Count; i++)
            {
                var devInfo = availableCameras[i];
                var location = devInfo.EnclosureLocation;

                if (location != null)
                {
                    if (location.Panel == Windows.Devices.Enumeration.Panel.Front)
                    {
                        frontCamera = devInfo;
                    }
                    else if (location.Panel == Windows.Devices.Enumeration.Panel.Back)
                    {
                        backCamera = devInfo;
                    }
                }
            }

            // Choose default camera
            if (backCamera != null)
            {
                currentCamera = backCamera;
            }
            else if (frontCamera != null)
            {
                currentCamera = frontCamera;
            }
            else
            {
                return false;
            }

            return true;
        }

        private void EnumerateSceneMode()
        {
            availableSceneModes = new List<SceneMode>();

            try
            {                
                var sceneModes = captureManager.VideoDeviceController.SceneModeControl.SupportedModes;
                
                // We only support a limit number of modes. Too many modes can be annoying and confusing
                foreach (var mode in sceneModes)
                {
                    switch (mode)
                    {
                        case Windows.Media.Devices.CaptureSceneMode.Auto:
                            availableSceneModes.Add(new SceneMode("Auto", mode));
                            break;
                        case Windows.Media.Devices.CaptureSceneMode.Portrait:
                            availableSceneModes.Add(new SceneMode("Portrait", mode));
                            break;
                        case Windows.Media.Devices.CaptureSceneMode.Landscape:
                            availableSceneModes.Add(new SceneMode("Landscape", mode));
                            break;
                        case Windows.Media.Devices.CaptureSceneMode.Night:
                            availableSceneModes.Add(new SceneMode("Night", mode));
                            break;
                        case Windows.Media.Devices.CaptureSceneMode.Macro:
                            availableSceneModes.Add(new SceneMode("Macro", mode));
                            break;
                        case Windows.Media.Devices.CaptureSceneMode.Sport:
                            availableSceneModes.Add(new SceneMode("Sport", mode));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void EnumerateEVValues()
        {
            // EV
            supportedEVValues = new List<float>();
            var ec = captureManager.VideoDeviceController.ExposureCompensationControl;
            evSupported = ec.Supported;
            
            if (evSupported)
            {
                var minEV = ec.Min;
                var maxEV = ec.Max;
                float step = (float)Math.Max(0.3, ec.Step);

                for (float i = minEV; i <= maxEV; i += step)
                {
                    if (i < maxEV)
                    {
                        supportedEVValues.Add(i);
                    }                    
                }

                // Make sure 0 is inside the range!
                if (!supportedEVValues.Contains(0))
                {
                    supportedEVValues.Add(0);
                    supportedEVValues.Sort();
                }

            }
        }

        private void EnumerateISOValues()
        {
            // ISO
            supportedISOValues = new List<uint>();
            var isoControl = captureManager.VideoDeviceController.IsoSpeedControl;
            isoSupported = isoControl.Supported;
            
            if (isoSupported)
            {
                // Add "auto" as base
                supportedISOValues.Add(ProCamConstraints.PROCAM_AUTO_ISO);

                for (uint iso = isoControl.Min; iso <= isoControl.Max; iso++)
                {
                    if (_supportedISOFixtures.Contains(iso))
                    {
                        supportedISOValues.Add(iso);
                    }                    
                }
            }
        }

        private void EnumerateResolutions()
        {
            supportedResolutions = new List<VideoEncodingProperties>();

            var resList = captureManager.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);
            var list = resList.OrderByDescending(res => (res as VideoEncodingProperties).Width).ToList();
            foreach (VideoEncodingProperties res in list)
            {
                supportedResolutions.Add(res);
            }
        }

        private void EnumerateWhiteBalances()
        {
            supportWhiteBalances = new List<ColorTemperaturePreset>();

            wbSupported = captureManager.VideoDeviceController.WhiteBalanceControl.Supported;
            if (wbSupported)
            {
                supportWhiteBalances.Add(ColorTemperaturePreset.Auto);
                supportWhiteBalances.Add(ColorTemperaturePreset.Daylight);
                supportWhiteBalances.Add(ColorTemperaturePreset.Cloudy);
                supportWhiteBalances.Add(ColorTemperaturePreset.Fluorescent);
                supportWhiteBalances.Add(ColorTemperaturePreset.Tungsten);
                supportWhiteBalances.Add(ColorTemperaturePreset.Flash);
                supportWhiteBalances.Add(ColorTemperaturePreset.Candlelight);
            }
        }

        private void CheckFocusSupport()
        {
            focusSupported = captureManager.VideoDeviceController.FocusControl.Supported;
            autoFocusSupported = captureManager.VideoDeviceController.FocusControl.SupportedFocusModes.Contains(FocusMode.Auto);
            focusRegionSupported = captureManager.VideoDeviceController.RegionsOfInterestControl.AutoFocusSupported && captureManager.VideoDeviceController.RegionsOfInterestControl.MaxRegions > 0;
        }

        /// <summary>
        /// There's a known issue where on certain devices, calling FlashControl.Supported causes exception
        /// </summary>
        private void CheckFlashSupport()
        {
            try
            {
                flashSupported = captureManager.VideoDeviceController.FlashControl.Supported;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                flashSupported = false;
            }
        }

        /// <summary>
        /// There's a know issue where on certain devices, calling FlashControl.AssistantLightSupport causes exception
        /// </summary>
        private void CheckFocusAssistSupport()
        {
            try
            {
                focusAssistSupported = captureManager.VideoDeviceController.FlashControl.AssistantLightSupported;
            } 
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                focusAssistSupported = false;
            }
            
            if (focusAssistSupported)
            {
                supportedFocusAssistModes = new List<FocusAssistMode> { FocusAssistMode.ON, FocusAssistMode.OFF };
            }
            else
            {
                supportedFocusAssistModes = new List<FocusAssistMode> { FocusAssistMode.OFF };
            }

        }

    }
}
