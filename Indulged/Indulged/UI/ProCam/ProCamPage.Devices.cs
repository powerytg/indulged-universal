using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private DeviceInformationCollection availableCameras;
        private DeviceInformation frontCamera;
        private DeviceInformation backCamera;
        private DeviceInformation currentCamera;

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
    }
}
