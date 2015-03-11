﻿using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.Common;
using Indulged.UI.Profile.Sections;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Profile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserProfilePage : Page
    {
        public static string PAGE_STATE_USER_ID = "userId";
        private NavigationHelper navigationHelper;
        private FlickrUser user;

        private UserProfileSection profileSection;
        private UserStreamSection streamSection;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserProfilePage()
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
            var userId = "";
            if (e.PageState != null && e.PageState.ContainsKey(PAGE_STATE_USER_ID))
            {
                userId = e.PageState[PAGE_STATE_USER_ID] as string;
            }
            else
            {
                userId = e.NavigationParameter as string;
            }

            user = StorageService.Instance.UserCache[userId];

            // Set hub title
            TitleLabel.Text = user.Name;

            // Load user photo stream
            if (streamSection != null)
            {
                streamSection.User = user;
            }

            // Load user profile
            if (profileSection != null)
            {
                profileSection.User = user;
            }
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
            e.PageState[PAGE_STATE_USER_ID] = user.ResourceId;
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

        private void StreamSection_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            streamSection = sender as UserStreamSection;
            if (user != null)
            {
                streamSection.User = user;
            }
        }

        private void UserProfileSection_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            profileSection = sender as UserProfileSection;
            if (user != null)
            {
                profileSection.User = user;
            }
        }
    }
}
