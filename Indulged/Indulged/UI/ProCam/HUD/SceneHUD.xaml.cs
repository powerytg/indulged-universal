using Indulged.UI.ProCam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SceneHUD : UserControl
    {
        // Events
        public EventHandler SceneModeChanged;

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

                Label.Text = _supportedSceneModes[_currentIndex].Name;
                RadioButton button = RadioGroupPanel.Children[_currentIndex] as RadioButton;

                if (button.IsChecked == false)
                {
                    button.IsChecked = true;
                }
            }
        }

        private List<SceneMode> _supportedSceneModes;
        public List<SceneMode> SupportedSceneModes
        {
            get
            {
                return _supportedSceneModes;
            }

            set
            {
                _supportedSceneModes = value;
                RebuildSceneOptions();
            }
        }

        // Constructor
        public SceneHUD()
        {
            InitializeComponent();
        }

        private void RebuildSceneOptions()
        {
            if (_supportedSceneModes == null)
            {
                return;
            }

            RadioGroupPanel.Children.Clear();

            foreach (var scene in _supportedSceneModes)
            {
                RadioButton button = new RadioButton();
                button.GroupName = "sceneMode";
                button.Content = scene.Name;
                button.Style = (Style)App.Current.Resources["HUDRadioButtonStyle"];
                button.Margin = new Thickness(0, 4, 4, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.Click += (sender, e) =>
                {
                    int index = RadioGroupPanel.Children.IndexOf((RadioButton)sender);
                    CurrentIndex = index;

                    if (SceneModeChanged != null)
                    {
                        SceneModeChanged(this, null);
                    }
                };

                RadioGroupPanel.Children.Add(button);
            }
        }
    }
}
