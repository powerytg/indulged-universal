using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Media.Capture;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private DisplayOrientations currentOrientation;
        private Guid rotGUID = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");

        // Orientation is reversed for front camera
        private bool reversePreviewRotation;
        
        private void OnOrientationChanged()
        {
            if (captureManager == null)
            {
                return;
            }

            bool previewMirroring = captureManager.GetPreviewMirroring();
            bool counterclockwiseRotation = (previewMirroring && !reversePreviewRotation) ||
                (!previewMirroring && reversePreviewRotation);

            if (isPreviewing)
            {
                captureManager.SetPreviewRotation(PreviewRotationLookup(currentOrientation, counterclockwiseRotation));
            }

            // Update UI
            DismissOSD();

            if (currentOrientation == DisplayOrientations.Landscape)
            {
                LayoutInLandscapeMode();
            }
            else
            {
                LayoutInPortraitMode();
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

        private VideoRotation PreviewRotationLookup(DisplayOrientations displayOrientation, bool counterclockwise)
        {
            switch (displayOrientation)
            {
                case DisplayOrientations.Landscape:
                    return VideoRotation.None;

                case DisplayOrientations.Portrait:
                    return (counterclockwise) ? VideoRotation.Clockwise270Degrees : VideoRotation.Clockwise90Degrees;

                case DisplayOrientations.LandscapeFlipped:
                    return VideoRotation.Clockwise180Degrees;

                case DisplayOrientations.PortraitFlipped:
                    return (counterclockwise) ? VideoRotation.Clockwise90Degrees :
                    VideoRotation.Clockwise270Degrees;

                default:
                    return VideoRotation.None;
            }
        }

        private Windows.Storage.FileProperties.PhotoOrientation GetCurrentPhotoRotation()
        {
            bool counterclockwiseRotation = reversePreviewRotation;
            return PhotoRotationLookup(currentOrientation, counterclockwiseRotation);
        }

    }
}
