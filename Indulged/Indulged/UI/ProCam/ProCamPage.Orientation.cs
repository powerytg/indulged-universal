using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private DisplayOrientations currentOrientation;
        private Guid rotGUID = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");
        private double rotHeight;
        private double rotWidth;

        // Orientation is reversed for front camera
        private bool reversePreviewRotation;

        private uint VideoPreviewRotationLookup(
            Windows.Graphics.Display.DisplayOrientations displayOrientation, bool counterclockwise)
        {
            switch (displayOrientation)
            {
                case Windows.Graphics.Display.DisplayOrientations.Landscape:
                    return 0;

                case Windows.Graphics.Display.DisplayOrientations.Portrait:
                    {
                        if (counterclockwise)
                        {
                            return 270;
                        }
                        else
                        {
                            return 90;
                        }
                    }

                case Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped:
                    return 180;

                case Windows.Graphics.Display.DisplayOrientations.PortraitFlipped:
                    {
                        if (counterclockwise)
                        {
                            return 90;
                        }
                        else
                        {
                            return 270;
                        }
                    }

                default:
                    return 0;
            }
        }

        private async void OnOrientationChanged()
        {
            try
            {
                if (captureManager == null)
                {
                    return;
                }

                var videoEncodingProperties = captureManager.VideoDeviceController.GetMediaStreamProperties(Windows.Media.Capture.MediaStreamType.VideoPreview);

                bool previewMirroring = captureManager.GetPreviewMirroring();
                bool counterclockwiseRotation = (previewMirroring && !reversePreviewRotation) ||
                    (!previewMirroring && reversePreviewRotation);

                if (isPreviewing)
                {
                    var rotDegree = VideoPreviewRotationLookup(currentOrientation, counterclockwiseRotation);
                    videoEncodingProperties.Properties.Add(rotGUID, rotDegree);
                    await captureManager.SetEncodingPropertiesAsync(Windows.Media.Capture.MediaStreamType.VideoPreview, videoEncodingProperties, null);
                    if (rotDegree == 90 || rotDegree == 270)
                    {
                        CameraView.Height = rotHeight;
                        CameraView.Width = rotWidth;
                    }
                    else
                    {
                        CameraView.Height = rotWidth;
                        CameraView.Width = rotHeight;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private Windows.Storage.FileProperties.PhotoOrientation PhotoRotationLookup(
            Windows.Graphics.Display.DisplayOrientations displayOrientation,
            bool counterclockwise)
        {
            switch (displayOrientation)
            {
                case Windows.Graphics.Display.DisplayOrientations.Landscape:
                    return Windows.Storage.FileProperties.PhotoOrientation.Normal;

                case Windows.Graphics.Display.DisplayOrientations.Portrait:
                    return (counterclockwise) ? Windows.Storage.FileProperties.PhotoOrientation.Rotate90 :
                        Windows.Storage.FileProperties.PhotoOrientation.Rotate270;

                case Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped:
                    return Windows.Storage.FileProperties.PhotoOrientation.Rotate180;

                case Windows.Graphics.Display.DisplayOrientations.PortraitFlipped:
                    return (counterclockwise) ? Windows.Storage.FileProperties.PhotoOrientation.Rotate270 :
                        Windows.Storage.FileProperties.PhotoOrientation.Rotate90;

                default:
                    return Windows.Storage.FileProperties.PhotoOrientation.Unspecified;
            }
        }

    }
}
