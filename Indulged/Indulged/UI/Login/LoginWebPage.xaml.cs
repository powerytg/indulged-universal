using Indulged.API.Networking;
using Indulged.Common;
using Indulged.UI.Common.Controls;
using Indulged.UI.Common.Controls.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWebPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public LoginWebPage()
        {
            this.InitializeComponent();

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
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Show progress bar
            LoadingView.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Collapsed;

            // Starting auth process
            GetRequestToken();
        }

        private void GetRequestToken()
        {
            APIService.Instance.GetRequestTokenAsync(() =>
            {
                // Proceed to show web view
                Uri loginUri = new Uri(APIService.Instance.AuthorizeUrl, UriKind.Absolute);
                Browser.Navigate(loginUri);
            },
            (errorMessage) =>
            {
                var errorDialog = ModalPopup.Show(errorMessage, "Sign In Error", new List<string> { "Retry", "Cancel" });
                errorDialog.DismissWithButtonClick += (s, evt) => {
                    if(evt.ButtonIndex == 0){
                        GetRequestToken();
                    }
                    else{
                        if(Frame.CanGoBack){
                            Frame.GoBack();
                        }
                    }
                };
            });
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if(LoadingView.Visibility != Visibility.Collapsed){
                LoadingView.Visibility = Visibility.Collapsed;
            }

            if (Browser.Visibility != Visibility.Visible)
            {
                Browser.Visibility = Visibility.Visible;
            }
        }

        private void Browser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string url = args.Uri.AbsoluteUri;
            Debug.WriteLine(url);

            if (url.StartsWith(APIService.Instance.CallbackUrl))
            {
                // Auth is successful
                args.Cancel = true;

                string paramString = url.Split('?')[1];
                string[] parts = paramString.Split('&');

                APIService.Instance.RequestTokenVerifier = parts[1].Split('=')[1];

                // Exchange for access token
                Browser.Visibility = Visibility.Collapsed;
                LoadingView.Visibility = Visibility.Visible;

                GetAccessToken();
            }
        }

        private void GetAccessToken()
        {
            APIService.Instance.GetAccessTokenAsync(() =>
            {
                APIService.Instance.SaveAccessCredentials();
                ModalPopup.Show(APIService.Instance.AccessToken, "OK", new List<string> { "Confirm" });
            }, 
            (errorMessage) =>
            {
                var errorDialog = ModalPopup.Show(errorMessage, "Authorization Error", new List<string> { "Confirm" });
                errorDialog.DismissWithButtonClick += (s, evt) =>
                {
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                };
            });
        }

    }
}
