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

        private CommandBar normalCommandBar;
        private CommandBar composerCommandBar;

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

            // Title
            HeaderView.Title = group.Name;

            if (streamSection != null)
            {
                streamSection.AddEventListeners();
                streamSection.Group = group;
            }

            if (discussionSection != null)
            {
                discussionSection.AddEventListeners();
                discussionSection.Group = group;
            }

            // Command bar
            PrepareCommandBars();
            BottomAppBar = normalCommandBar;
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

            if (streamSection != null)
            {
                streamSection.RemoveEventListeners();
            }

            if (discussionSection != null)
            {
                discussionSection.RemoveEventListeners();
            }
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

        private void PrepareCommandBars()
        {
            // Photo stream commandbar
            normalCommandBar = new CommandBar();
            normalCommandBar.IsOpen = false;

            var addPhotoButton = new AppBarButton() { Icon = new SymbolIcon(Symbol.Add), Label = "add photo" };
            addPhotoButton.Click += AddPhotoButton_Click;

            var addTopicButton = new AppBarButton() { Icon = new SymbolIcon(Symbol.Comment), Label = "add topic" };
            addTopicButton.Click += AddTopicButton_Click;

            normalCommandBar.PrimaryCommands.Add(addPhotoButton);
            normalCommandBar.PrimaryCommands.Add(addTopicButton);

            // Comment composer commandbar
            composerCommandBar = new CommandBar();
            composerCommandBar.IsOpen = false;

            var postButton = new AppBarButton() { Label = "post topic", Icon = new SymbolIcon(Symbol.Send) };
            postButton.Click += PostButton_Click;
            composerCommandBar.PrimaryCommands.Add(postButton);
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddTopicButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
    }
}
