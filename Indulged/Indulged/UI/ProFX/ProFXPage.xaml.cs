using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indulged.UI.ProFX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProFXPage : Page
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProFXPage()
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
            var file = e.Parameter as StorageFile;
            if (file == null)
            {
                ShowFatalError("Fatal error: cannot access image");
                return;
            }

            // Prepare for sampling
            originalFile = file;
            ViewFinder.SizeChanged += OnViewFinderSizeChanged;

            // Filters
            filterManager = new Filters.FXFilterManager();
            FilterGalleryView.FilterManager = filterManager;
            FilterGalleryView.InitializeFilterDroplets();

            // Events
            InitializeEventListeneres();
        }

        private void ShowFatalError(string message) {
            ErrorView.Text = message;
            ErrorView.Visibility = Visibility.Visible;
        }

    }
}
