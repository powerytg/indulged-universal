using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.Common;
using Indulged.UI.Common.Controls;
using Indulged.UI.Detail.Dialogs;
using Indulged.UI.Detail.Sections;
using System;
using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.Detail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private static string PAGE_STATE_PHOTO_ID = "photoId";

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private FlickrPhoto photo;

        private static SymbolIcon likeIcon = new SymbolIcon(Symbol.Like);
        private static SymbolIcon unlikeIcon = new SymbolIcon(Symbol.Dislike);

        private DetailSectionBase[] sections;

        private CommandBar normalCommandBar;
        private CommandBar composerCommandBar;
        private AppBarButton FavButton;
        private AppBarButton PostButton;
        private CommentComposer composer;
        private Flyout composerFlyout;

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailPage()
        {
            this.InitializeComponent();

            // Available sections
            sections = new DetailSectionBase[] { 
                BasicSectionView, 
                EXIFSectionView, 
                TagSectionView, 
                ReviewSectionView };

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
            var photoId = "";
            if (e.PageState != null && e.PageState.ContainsKey(PAGE_STATE_PHOTO_ID))
            {
                photoId = e.PageState[PAGE_STATE_PHOTO_ID] as string;
            }
            else
            {
                photoId = e.NavigationParameter as string;    
            }

            photo = StorageService.Instance.PhotoCache[photoId];

            // Command bar icons
            FavButton.Icon = (photo.IsFavourite) ? unlikeIcon : likeIcon;

            // Do not allow liking one's own photo
            if (photo.UserId == StorageService.Instance.CurrentUser.ResourceId)
            {
                FavButton.IsEnabled = false;
            }

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

            // Register events
            StorageService.Instance.PhotoAddedAsFavourite += OnFavouriteStatusChanged;
            StorageService.Instance.PhotoRemovedFromFavourite += OnFavouriteStatusChanged;

            foreach (var section in sections)
            {
                section.AddEventListeners();
            }

            // Fill in sections     
            foreach (var section in sections)
            {
                section.Photo = photo;
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
            e.PageState[PAGE_STATE_PHOTO_ID] = photo.ResourceId;

            // Remove all event listeners
            StorageService.Instance.PhotoAddedAsFavourite -= OnFavouriteStatusChanged;
            StorageService.Instance.PhotoRemovedFromFavourite -= OnFavouriteStatusChanged;

            foreach (var section in sections)
            {
                section.RemoveEventListeners();
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

        private void FavButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleFavourite();
        }

        private async void ToggleFavourite()
        {
            FavButton.IsEnabled = false;

            // Show status bar progress
            var sb = StatusBar.GetForCurrentView();
            sb.ProgressIndicator.Text = (photo.IsFavourite) ? "Removing from favourite" : "Adding to favourite";
            await sb.ProgressIndicator.ShowAsync();

            // Sending request
            if (photo.IsFavourite)
            {
                // Should remove from favourite
                var status = await APIService.Instance.RemoveFromFavouriteAsync(photo.ResourceId);
                if (!status.Success)
                {
                    OnFavRequestError(status.ErrorMessage);
                }
            }
            else
            {
                // Should add to favourite
                var status = await APIService.Instance.AddToFavouriteAsync(photo.ResourceId);
                if (!status.Success)
                {
                    OnFavRequestError(status.ErrorMessage);
                }
            }

        }

        private async void OnFavouriteStatusChanged(object sender, API.Storage.Events.StorageEventArgs e)
        {
            if (photo == null || e.PhotoId != photo.ResourceId)
            {
                return;
            }

            var sb = StatusBar.GetForCurrentView();
            await sb.ProgressIndicator.HideAsync();
            FavButton.Icon = (photo.IsFavourite) ? unlikeIcon : likeIcon;
            FavButton.IsEnabled = true;
        }


        private async void OnFavRequestError(string errorMessage)
        {
            var sb = StatusBar.GetForCurrentView();
            await sb.ProgressIndicator.HideAsync();
            FavButton.IsEnabled = true;

            ModalPopup.Show("An error has happened: " + errorMessage, "Error", new List<string> { "Dismiss" });
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

            FavButton = new AppBarButton() { Label = "like/unlike" };
            FavButton.Click += FavButton_Click;

            var commentButton = new AppBarButton() { Icon = new SymbolIcon(Symbol.Comment), Label = "comment" };
            commentButton.Click += CommentButton_Click;

            normalCommandBar.PrimaryCommands.Add(FavButton);
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


    }
}
