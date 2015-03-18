using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;

namespace Indulged.UI.ProCam.Models
{
    public class SceneMode
    {
        public string Name { get; set; }
        public CaptureSceneMode Mode { get; set; }

        public SceneMode(string name, CaptureSceneMode mode)
        {
            Name = name;
            Mode = mode;
        }
    }
}
