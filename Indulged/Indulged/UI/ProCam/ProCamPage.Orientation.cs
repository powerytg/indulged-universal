using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private DisplayOrientations currentOrientation;

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
    }
}
