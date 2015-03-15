using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.Common;
using Indulged.UI.Group.Sections;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Group
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupPage : Page
    {
        public static string PAGE_STATE_GROUP_ID = "groupId";

        private NavigationHelper navigationHelper;
        private GroupPhotoStreamSection streamSection;
        private GroupDiscussionSection discussionSection;
        private FlickrGroup group;

        public GroupPage()
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
            var groupId = "";
            if (e.PageState != null && e.PageState.ContainsKey(PAGE_STATE_GROUP_ID))
            {
                groupId = e.PageState[PAGE_STATE_GROUP_ID] as string;
            }
            else
            {
                groupId = e.NavigationParameter as string;
            }

            group = StorageService.Instance.GroupCache[groupId];

            if (streamSection != null)
            {
                streamSection.Group = group;
            }

            if (discussionSection != null)
            {
                discussionSection.Group = group;
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
            e.PageState[PAGE_STATE_GROUP_ID] = group.ResourceId;
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

        private void StreamSection_Loaded(object sender, RoutedEventArgs e)
        {
            streamSection = sender as GroupPhotoStreamSection;
            if (group != null)
            {
                streamSection.Group = group;
            }
        }

        private void DiscussionSection_Loaded(object sender, RoutedEventArgs e)
        {
            discussionSection = sender as GroupDiscussionSection;
            if (group != null)
            {
                discussionSection.Group = group;
            }
        }
    }
}
