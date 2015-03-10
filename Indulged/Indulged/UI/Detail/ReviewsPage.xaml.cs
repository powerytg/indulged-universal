using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.Common;
using Indulged.UI.Detail.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Detail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableCollection<FlickrComment> comments = new ObservableCollection<FlickrComment>();

        private FlickrPhoto photo;

        private CommandBar normalCommandBar;
        private CommandBar composerCommandBar;
        private AppBarButton PostButton;
        private CommentComposer composer;
        private Flyout composerFlyout;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            PrepareCommandBars();
            BottomAppBar = normalCommandBar;
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
            var photoId = "";
            if (e.PageState != null && e.PageState.ContainsKey(DetailPage.PAGE_STATE_PHOTO_ID))
            {
                photoId = e.PageState[DetailPage.PAGE_STATE_PHOTO_ID] as string;
            }
            else
            {
                photoId = e.NavigationParameter as string;
            }

            photo = StorageService.Instance.PhotoCache[photoId];

            // Image
            ImageView.Source = new BitmapImage(new Uri(photo.GetImageUrl(), UriKind.Absolute));

            // Background
            if (PolKit.PolicyKit.Instance.UseBlurredBackground)
            {
                BackgroundView.Visibility = Visibility.Visible;
                BackgroundView.PhotoSource = photo;
            }
            else
            {
                BackgroundView.Visibility = Visibility.Collapsed;
            }

            // Author and date
            var user = StorageService.Instance.UserCache[photo.UserId];
            AuthorLabel.Text = user.Name + " · " + photo.DateTaken;

            // Data source
            comments.Clear();
            foreach (var comment in photo.Comments)
            {
                comments.Add(comment);
            }

            CommentListView.ItemsSource = comments;

            // Events
            StorageService.Instance.CommentAdded += OnCommentsUpdated;
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
            e.PageState[DetailPage.PAGE_STATE_PHOTO_ID] = photo.ResourceId;

            // Remove event handlers
            StorageService.Instance.CommentAdded -= OnCommentsUpdated;
        }

        private void OnCommentsUpdated(object sender, API.Storage.Events.StorageEventArgs e)
        {
            foreach (var comment in e.NewComments)
            {
                comments.Add(comment);
            }
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            composer = new CommentComposer();
            composer.PhotoId = photo.ResourceId;
            composerFlyout = new Flyout();
            composerFlyout.Content = composer;
            composerFlyout.ShowAt(this);
            composerFlyout.Closed += OnComposerClosed;

            // Events
            composer.PostButtonClicked += PostButton_Click;

            // Change command bar to composer mode
            BottomAppBar = composerCommandBar;
        }

        private void OnComposerClosed(object sender, object e)
        {
            composer.PostButtonClicked -= PostButton_Click;
            composer = null;

            // Change back to normal command bar
            BottomAppBar = normalCommandBar;
        }

        private void PrepareCommandBars()
        {
            // Normal commandbar
            normalCommandBar = new CommandBar();
            normalCommandBar.IsOpen = false;

            var commentButton = new AppBarButton() { Icon = new SymbolIcon(Symbol.Comment), Label = "comment" };
            commentButton.Click += CommentButton_Click;
            normalCommandBar.PrimaryCommands.Add(commentButton);

            // Comment composer commandbar
            composerCommandBar = new CommandBar();
            composerCommandBar.IsOpen = false;

            PostButton = new AppBarButton() { Label = "post comment", Icon = new SymbolIcon(Symbol.Send) };
            PostButton.Click += PostButton_Click;
            composerCommandBar.PrimaryCommands.Add(PostButton);
        }

        private async void PostButton_Click(object sender, RoutedEventArgs e)
        {
            // Freeze "send" button
            PostButton.IsEnabled = false;

            var success = await composer.PostCommentAsync();
            if (success)
            {
                // Dismiss composer
                composerFlyout.Hide();

                // Change command bar to normal mode
                BottomAppBar = normalCommandBar;
            }
            else
            {
                // Unfreeze send button
                PostButton.IsEnabled = true;
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
    }
}
