using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Devices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.HUD
{
    public sealed partial class WhiteBalanceHUD : UserControl
    {
        // Events
        public EventHandler WhiteBalanceChanged;

        private int _currentWhiteBalanceIndex;
        public int CurrentWhiteBalanceIndex
        {
            get
            {
                return _currentWhiteBalanceIndex;
            }

            set
            {
                _currentWhiteBalanceIndex = value;

                Label.Text = whiteBalanceStrings[_currentWhiteBalanceIndex];
                RadioButton button = RadioGroupPanel.Children[_currentWhiteBalanceIndex] as RadioButton;

                if (button.IsChecked == false)
                {
                    button.IsChecked = true;
                }
            }
        }

        private List<ColorTemperaturePreset> _supportedWhiteBalances;
        public List<ColorTemperaturePreset> SupportedWhiteBalances
        {
            get
            {
                return _supportedWhiteBalances;
            }

            set
            {
                _supportedWhiteBalances = value;

                whiteBalanceStrings.Clear();
                for (int i = 0; i < _supportedWhiteBalances.Count; i++)
                {
                    if (i == 0)
                    {
                        whiteBalanceStrings.Add("Auto");
                    }
                    else
                    {
                        var wb = _supportedWhiteBalances[i];
                        switch (wb)
                        {
                            case ColorTemperaturePreset.Candlelight:
                                whiteBalanceStrings.Add("Candle light");
                                break;
                            case ColorTemperaturePreset.Cloudy:
                                whiteBalanceStrings.Add("Cloudy");
                                break;
                            case ColorTemperaturePreset.Daylight:
                                whiteBalanceStrings.Add("Day light");
                                break;
                            case ColorTemperaturePreset.Flash:
                                whiteBalanceStrings.Add("Flash");
                                break;
                            case ColorTemperaturePreset.Fluorescent:
                                whiteBalanceStrings.Add("Fluorescent");
                                break;
                            case ColorTemperaturePreset.Tungsten:
                                whiteBalanceStrings.Add("Tungsten");
                                break;
                        }
                    }
                }

                RebuildWhiteBalanceOptions();
            }
        }

        private List<string> whiteBalanceStrings = new List<string>();
        public List<string> WhiteBalanceStrings
        {
            get
            {
                return whiteBalanceStrings;
            }
        }

        // Constructor
        public WhiteBalanceHUD()
        {
            InitializeComponent();
        }

        private void RebuildWhiteBalanceOptions()
        {
            RadioGroupPanel.Children.Clear();

            foreach (var wb in whiteBalanceStrings)
            {
                RadioButton button = new RadioButton();
                button.GroupName = "whiteBalance";
                button.Content = wb;
                button.Style = (Style)App.Current.Resources["HUDRadioButtonStyle"];
                button.Margin = new Thickness(0, 4, 4, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.VerticalAlignment = VerticalAlignment.Center;
                
                button.Click += (sender, e) =>
                {
                    int index = RadioGroupPanel.Children.IndexOf((RadioButton)sender);
                    CurrentWhiteBalanceIndex = index;

                    if (WhiteBalanceChanged != null)
                    {
                        WhiteBalanceChanged(this, null);
                    }
                };
                RadioGroupPanel.Children.Add(button);
            }
        }
    }
}
