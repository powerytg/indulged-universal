using Indulged.UI.ProFX.Filters;
using System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Controls
{
    public sealed partial class FilterEntryControl : UserControl
    {
        // Events
        public EventHandler OnDelete;
        public EventHandler OnVisibilityChanged;
        public EventHandler RequestFilterView;

        private static BitmapImage enabledImage = new BitmapImage(new Uri("ms-appx:///Assets/ProFX/FXFilterEnabled.png", UriKind.Relative));
        private static BitmapImage disabledImage = new BitmapImage(new Uri("ms-appx:///Assets/ProFX/FXFilterDisabled.png", UriKind.Relative));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(FilterBase), typeof(FilterEntryControl), new PropertyMetadata(null, OnFilterPropertyChanged));

        public FilterBase Filter
        {
            get
            {
                return (FilterBase)GetValue(FilterProperty);
            }
            set
            {
                SetValue(FilterProperty, value);
            }
        }

        public static void OnFilterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((FilterEntryControl)sender).OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            Label.Text = Filter.DisplayName;
            UpdateVisibilityIcon();
        }

        private void UpdateVisibilityIcon()
        {
            if (Filter.IsFilterEnabled)
            {
                Icon.Source = enabledImage;
            }
            else
            {
                Icon.Source = disabledImage;
            }
        }

        // Constructor
        public FilterEntryControl()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this, null);
            }
        }

        private void VisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            Filter.IsFilterEnabled = !Filter.IsFilterEnabled;
            UpdateVisibilityIcon();

            if (OnVisibilityChanged != null)
            {
                OnVisibilityChanged(this, null);
            }
        }

        private void Label_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (RequestFilterView != null)
            {
                RequestFilterView(this, null);
            }
        }
    }
}
