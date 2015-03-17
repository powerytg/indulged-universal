using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.ProCam
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProCamPage : Page
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProCamPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Listen for orientation events
            DisplayInformation.GetForCurrentView().OrientationChanged += DisplayInfo_OrientationChanged;
            currentOrientation = DisplayInformation.GetForCurrentView().CurrentOrientation;

            // Initialize camera
            InitializeCamera();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Remove event handlers
            DisplayInformation.GetForCurrentView().OrientationChanged -= DisplayInfo_OrientationChanged;

            // Destroy camera
            DestroyCamera();
        }

        private void DisplayInfo_OrientationChanged(DisplayInformation sender, object args)
        {
            
        }
    }
}
