using Indulged.API.Networking;
using Indulged.Common;
using Indulged.UI.Search.Sections;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Search
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResultPage : Page
    {
        public static string QUERY_KEY = "query";
        public static string QUERY_TYPE_KEY = "queryType";

        private NavigationHelper navigationHelper;
        private PhotoResultSection photoSection;
        private GroupResultSection groupSection;
        private string keyword;
        private string queryType;

        public SearchResultPage()
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
            var dict = e.NavigationParameter as Dictionary<string, string>;
            keyword = dict[QUERY_KEY];
            queryType = dict[QUERY_TYPE_KEY];

            if (queryType == APIService.QUERY_TYPE_TAGS)
            {
                HeaderView.Title = "#" + keyword;
            }
            else
            {
                HeaderView.Title = keyword;
            }            

            if (photoSection != null && photoSection.Keyword != keyword)
            {
                photoSection.QueryType = queryType;
                photoSection.Keyword = keyword;
            }

            if (groupSection != null && groupSection.Keyword != keyword)
            {
                groupSection.QueryType = queryType;
                groupSection.Keyword = keyword;
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

        private void PhotoSection_Loaded(object sender, RoutedEventArgs e)
        {
            photoSection = sender as PhotoResultSection;
            if (keyword != null && queryType != null)
            {
                photoSection.QueryType = queryType;
                photoSection.Keyword = keyword;
            }
        }

        private void GroupSection_Loaded(object sender, RoutedEventArgs e)
        {
            groupSection = sender as GroupResultSection;
            if (keyword != null && queryType != null)
            {
                groupSection.QueryType = queryType;
                groupSection.Keyword = keyword;
            }
        }

    }
}
