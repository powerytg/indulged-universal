using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Indulged.UI.ProCam.Utils;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.Controls
{
    public sealed partial class ISODialControl : UserControl
    {
        private double cumulativeDist;
        private double rotationStep = 80;
        private double anglePerStep;

        private int baseIndex;

        public bool IsInfiniteScrollingEnabled { get; set; }

        public uint CurrentValue { get; set; }

        private List<uint> _supportedValues;
        public List<uint> SupportedValues
        {
            get
            {
                return _supportedValues;
            }
            set
            {
                _supportedValues = value;
                anglePerStep = 360 / SupportedValues.Count;
                baseIndex = 0;
                CurrentIndex = baseIndex;

                // Calculate an optimized rotation step
                var size = Math.Min(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
                rotationStep = size / _supportedValues.Count;

                Label.Text = SupportedValues[CurrentIndex].ToISOString();
            }
        }

        public int CurrentIndex { get; set; }

        // Events
        public EventHandler DragBegin;
        public EventHandler DragEnd;
        public EventHandler ValueChanged;

        // Constructor
        public ISODialControl()
        {
            InitializeComponent();

            // Initialize properties
            IsInfiniteScrollingEnabled = true;

            GestureCaptureCanvas.ManipulationStarted += OnDialerDragStart;
            GestureCaptureCanvas.ManipulationCompleted += OnDialerDragEnd;
            GestureCaptureCanvas.ManipulationDelta += OnDialerDragDelta;
        }

        private void OnDialerDragStart(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DialerTransform.CenterX = Dialer.ActualWidth / 2;
            DialerTransform.CenterY = Dialer.ActualHeight / 2;

            cumulativeDist = 0;

            if (DragBegin != null)
            {
                DragBegin(this, null);
            }
        }

        private void OnDialerDragEnd(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (DragEnd != null)
            {
                DragEnd(this, null);
            }
        }

        private void OnDialerDragDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double currentDist;
            if (Math.Abs(e.Cumulative.Translation.X) > Math.Abs(e.Cumulative.Translation.Y))
            {
                currentDist = e.Cumulative.Translation.X;
            }
            else
            {
                currentDist = e.Cumulative.Translation.Y;
            }

            if (Math.Abs(currentDist - cumulativeDist) >= rotationStep)
            {
                var dist = currentDist - cumulativeDist;
                if (dist < 0)
                {
                    // Moving up
                    CurrentIndex++;

                    if (CurrentIndex >= SupportedValues.Count)
                    {
                        CurrentIndex = IsInfiniteScrollingEnabled ? 0 : SupportedValues.Count - 1;
                    }
                }
                else
                {
                    // Moving down
                    CurrentIndex--;

                    if (CurrentIndex < 0)
                    {
                        CurrentIndex = IsInfiniteScrollingEnabled ? SupportedValues.Count - 1 : 0;
                    }
                }

                DialerTransform.Angle = (CurrentIndex - baseIndex) * anglePerStep;

                // Reset cumulative dist
                cumulativeDist = currentDist;
                CurrentValue = SupportedValues[CurrentIndex];
                Label.Text = CurrentValue.ToISOString();

                // Show ticks
                var percent = CurrentIndex / ((float)SupportedValues.Count - 1);
                //ISOTickView.Opacity = 0.3 + 0.7 * percent;
                if (ValueChanged != null)
                {
                    ValueChanged(this, null);
                }
            }
        }
    }
}
