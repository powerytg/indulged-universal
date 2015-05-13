using System;

namespace Indulged.UI.ProFX.Events
{
    public class CropAreaChangedEventArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
