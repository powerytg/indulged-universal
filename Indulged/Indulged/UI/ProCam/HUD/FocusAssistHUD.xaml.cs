using Indulged.UI.ProCam.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.HUD
{
    public sealed partial class FocusAssistHUD : UserControl
    {
        // Events
        public EventHandler FocusAssistModeChanged;

        private int _currentIndex;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }

            set
            {
                _currentIndex = value;

                Label.Text = ModeStrings[_currentIndex];
                RadioButton button = RadioGroupPanel.Children[_currentIndex] as RadioButton;

                if (button.IsChecked == false)
                {
                    button.IsChecked = true;
                }
            }
        }

        private List<FocusAssistMode> _supportedModes;
        public List<FocusAssistMode> SupportedModes
        {
            get
            {
                return _supportedModes;
            }

            set
            {
                _supportedModes = value;

                ModeStrings.Clear();
                foreach (FocusAssistMode mode in _supportedModes)
                {
                    switch (mode)
                    {
                        case FocusAssistMode.ON:
                            ModeStrings.Add("On");
                            break;
                        case FocusAssistMode.OFF:
                            ModeStrings.Add("Off");
                            break;
                    }
                }


                RebuildModeOptions();
            }
        }

        public List<string> ModeStrings { get; set; }

        // Constructor
        public FocusAssistHUD()
        {
            InitializeComponent();
            ModeStrings = new List<string>();
        }

        private void RebuildModeOptions()
        {
            RadioGroupPanel.Children.Clear();

            foreach (var mode in ModeStrings)
            {
                RadioButton button = new RadioButton();
                button.GroupName = "focusAssistMode";
                button.Content = mode;
                button.Style = (Style)App.Current.Resources["HUDRadioButtonStyle"];
                button.Margin = new Thickness(0, 4, 4, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.Click += (sender, e) =>
                {
                    int index = RadioGroupPanel.Children.IndexOf((RadioButton)sender);
                    CurrentIndex = index;

                    if (FocusAssistModeChanged != null)
                    {
                        FocusAssistModeChanged(this, null);
                    }
                };

                RadioGroupPanel.Children.Add(button);
            }
        }
    }
}
