using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
using Indulged.Common;
using Indulged.PolKit;
using Indulged.UI.Common.Controls;
using Indulged.UI.Dashboard;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Indulged
{
    public sealed partial class PivotPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        /// <summary>
        /// Constructor
        /// </summary>
        public PivotPage()
        {
            this.InitializeComponent();

            // Make app full screen
            ApplicationView.GetForCurrentView().
            SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            // Retrieve settings
            PolicyKit.Instance.RetrieveSettings();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {            
            // Events
            StorageService.Instance.PhotoStreamUpdated += OnPhotoStreamUpdated;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // Events
            StorageService.Instance.PhotoStreamUpdated -= OnPhotoStreamUpdated;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void OnPhotoStreamUpdated(object sender, StorageEventArgs e)
        {
            if (PreludeView.CurrentStream == e.UpdatedStream)
            {
                if (e.UpdatedStream.Photos.Count != 0)
                {
                    //BackgroundView.PhotoSource = e.UpdatedStream.Photos[0]; 
                }                
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 2)
            {
                DashboardThemeManager.Instance.SelectedTheme = DashboardThemes.Light;
            }
            else
            {
                DashboardThemeManager.Instance.SelectedTheme = DashboardThemes.Dark;
            }
        }

        private void PreludeStyleButton_Click(object sender, RoutedEventArgs e)
        {
            // Choose display style from magazine, banner, or journal
            var contentView = new PreludeStreamStyleDialog();
            var dialog = ModalPopup.Show(contentView, "Choose Display Style", new List<string> { "Confirm", "Cancel" });
            dialog.DismissWithButtonClick += (s, evt) =>
            {
                if (evt.ButtonIndex == 0)
                {
                    var selectedStyle = contentView.SelectedStyle;

                    if (selectedStyle != PolKit.PolicyKit.Instance.PreludeLayoutStyle)
                    {
                        PolKit.PolicyKit.Instance.PreludeLayoutStyle = selectedStyle;
                    }
                }
            };
        }

        /// <summary>
        /// Show "Choose Stream" dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            PreludeView.ShowStreamSelectionDialog();
        }

    }
}
