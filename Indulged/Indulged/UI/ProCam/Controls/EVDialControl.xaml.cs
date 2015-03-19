using Indulged.UI.ProCam.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.Controls
{
    public sealed partial class EVDialControl : UserControl
    {
        private double cumulativeDist;
        private double rotationStep = 80;
        private double anglePerStep;

        private int baseIndex;

        public bool IsInfiniteScrollingEnabled { get; set; }

        public float CurrentValue { get; set; }

        private List<float> positiveValues = new List<float>();
        private List<float> negativeValues = new List<float>();

        private BitmapImage baseTickImage;
        private List<BitmapImage> positiveTickImages = new List<BitmapImage>();
        private List<BitmapImage> negativeTickImages = new List<BitmapImage>();

        private BitmapImage arrowLeftImage = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/ArrowLeft.png"));
        private BitmapImage arrowRightImage = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/ArrowRight.png"));

        private List<float> _supportedValues;
        public List<float> SupportedValues
        {
            get
            {
                return _supportedValues;
            }
            set
            {
                _supportedValues = value;
                positiveValues.Clear();
                negativeValues.Clear();

                foreach (var ev in SupportedValues)
                {
                    if (ev > 0)
                    {
                        positiveValues.Add(ev);
                    }
                    else if (ev < 0)
                    {
                        negativeValues.Add(ev);
                    }
                }

                // Calculate an optimized rotation step
                var size = Math.Min(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
                rotationStep = size / _supportedValues.Count;

                anglePerStep = 360 / SupportedValues.Count;
                baseIndex = SupportedValues.IndexOf(0);
                CurrentIndex = baseIndex;

                Label.Text = SupportedValues[CurrentIndex].ToEVString();
            }
        }

        public int CurrentIndex { get; set; }

        // Events
        public EventHandler DragBegin;
        public EventHandler DragEnd;
        public EventHandler ValueChanged;

        // Constructor
        public EVDialControl()
        {
            InitializeComponent();

            // Initialize properties
            IsInfiniteScrollingEnabled = true;

            // Initialize tick images
            baseTickImage = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV0.png"));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV1.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV2.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV3.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV4.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV5.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV6.png")));
            positiveTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV7.png")));

            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-1.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-2.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-3.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-4.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-5.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-6.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-7.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-8.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-9.png")));
            negativeTickImages.Add(new BitmapImage(new Uri("ms-appx:///Assets/ProCam/EV/EV-10.png")));

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
                Label.Text = CurrentValue.ToEVString();

                // Show ticks
                if (CurrentIndex == baseIndex)
                {
                    EVTickView.Source = baseTickImage;
                }
                else if (CurrentIndex > baseIndex)
                {
                    var percent = (CurrentIndex - baseIndex) / (float)positiveValues.Count;
                    int tick = (int)Math.Floor(percent * positiveTickImages.Count);
                    tick = Math.Min(tick, positiveTickImages.Count - 1);
                    tick = Math.Max(tick, 0);
                    EVTickView.Source = positiveTickImages[tick];
                }
                else
                {
                    var percent = (baseIndex - CurrentIndex) / (float)negativeValues.Count;
                    int tick = (int)Math.Floor(percent * negativeTickImages.Count);
                    tick = Math.Min(tick, negativeTickImages.Count - 1);
                    tick = Math.Max(tick, 0);
                    EVTickView.Source = negativeTickImages[tick];
                }

                if (ValueChanged != null)
                {
                    ValueChanged(this, null);
                }
            }
        }

        public void LayoutInLandscapeMode()
        {
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Pixel) });

            Dialer.SetValue(Grid.ColumnProperty, 0);

            Label.SetValue(Grid.ColumnProperty, 1);
            Label.HorizontalAlignment = HorizontalAlignment.Left;
            Label.Margin = new Thickness(10, 0, 0, 0);

            Arrow.SetValue(Grid.ColumnProperty, 0);
            Arrow.Source = arrowRightImage;
            GestureCaptureCanvas.SetValue(Grid.ColumnProperty, 0);
        }

        public void LayoutInPortraitMode()
        {
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Pixel) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Label.SetValue(Grid.ColumnProperty, 0);
            Label.HorizontalAlignment = HorizontalAlignment.Right;
            Label.Margin = new Thickness(0, 0, 10, 0);

            Dialer.SetValue(Grid.ColumnProperty, 1);


            Arrow.SetValue(Grid.ColumnProperty, 1);
            Arrow.Source = arrowLeftImage;
            GestureCaptureCanvas.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
