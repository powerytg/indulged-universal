using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Indulged.UI.ProCam.Utils;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.HUD
{
    public sealed partial class ISOHUD : UserControl
    {
        private int baseIndex;
        private FrameworkElement firstRenderer = null;
        private Canvas paddingHeader, paddingFooter;

        protected List<uint> _supportedValues;
        public List<uint> SupportedValues
        {
            get
            {
                return _supportedValues;
            }
            set
            {
                _supportedValues = value;

                baseIndex = _supportedValues.IndexOf(0);

                // Rebuild value panel
                ValuePanel.Children.Clear();

                // Add a padding header
                paddingHeader = new Canvas();
                paddingHeader.Height = this.Height;
                ValuePanel.Children.Add(paddingHeader);

                foreach (var iso in SupportedValues)
                {
                    TextBlock isoLabel = new TextBlock();
                    if (firstRenderer == null)
                    {
                        firstRenderer = isoLabel;
                    }

                    isoLabel.Foreground = new SolidColorBrush(Colors.White);
                    isoLabel.FontSize = 18;
                    isoLabel.FontWeight = FontWeights.Medium;
                    isoLabel.Text = iso.ToISOString();
                    isoLabel.Margin = new Thickness(6, 0, 0, 0);
                    ValuePanel.Children.Add(isoLabel);
                }

                // Add a padding footer
                paddingFooter = new Canvas();
                paddingFooter.Height = this.Height;
                ValuePanel.Children.Add(paddingFooter);

            }
        }

        protected uint _selectedValue;
        public uint SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                int index = SupportedValues.IndexOf(_selectedValue);
                SmoothShiftValueListToPosition(index);
            }
        }

        // Constructor
        public ISOHUD()
        {
            InitializeComponent();
        }

        private void SmoothShiftValueListToPosition(int index)
        {
            // Preparation
            double itemHeight = firstRenderer.ActualHeight;
            double targetY = itemHeight * index - Scroller.ActualHeight / 2 + itemHeight / 2;
            targetY += paddingHeader.Height;

            Scroller.ScrollToVerticalOffset(targetY);
        }
    }
}
