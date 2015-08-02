using Indulged.API.Media;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Controls
{
    public sealed partial class ColorEditor : UserControl
    {
        private static int MaxBrightness = 255;

        // Events
        public EventHandler ValueChanged;

        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
       "HSBColorSource",
       typeof(HSBColor),
       typeof(ColorEditor),
       new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ColorEditor)sender;
            target.OnHSBColorSourceChanged();
        }

        private void OnHSBColorSourceChanged()
        {
            if (HSBColorSource == null)
            {
                return;
            }

            UpdateColorPreview();
            Editor.HSBColorSource = HSBColorSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorEditor()
        {
            this.InitializeComponent();

            Editor.ValueChanged += OnColorChanged;
        }

        private void UpdateColorPreview()
        {
            Color fillColor = HSBColor.FromHSB(HSBColorSource);
            ColorNameLabel.Text = HSBColorSource.ToRGBString();

            double maxSize = PreviewBorder.Width - 15;
            double previewSize = maxSize * (HSBColorSource.B / MaxBrightness);
            ThumbnailView.Width = previewSize;
            ThumbnailView.Height = previewSize;
            ThumbnailFill.Color = fillColor;
        }

        private void OnColorChanged(object sender, EventArgs e)
        {
            UpdateColorPreview();

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

    }
}
