using Indulged.UI.ProFX.Filters;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Controls
{
    public sealed partial class FilterDropletControl : UserControl
    {
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(FilterDropletControl), new PropertyMetadata(false, OnSelectedPropertyChanged));

        private static SolidColorBrush normalBackgroundBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x1d, 0x27, 0x33));
        private static SolidColorBrush normalStroke = new SolidColorBrush(Color.FromArgb(0x00, 0x02, 0xb9, 0xa2));
        private static SolidColorBrush selectedBackgroundBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x01, 0x40, 0x3b));
        private static SolidColorBrush selectedStroke = new SolidColorBrush(Color.FromArgb(0xff, 0x02, 0xb9, 0xa2));

        public bool Selected
        {
            get
            {
                return (bool)GetValue(SelectedProperty);
            }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }

        public static void OnSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((FilterDropletControl)sender).OnSelectedChanged();
        }

        private void OnSelectedChanged()
        {
            if (Selected)
            {
                Icon.Visibility = Visibility.Visible;
                BackgroundBorder.Background = selectedBackgroundBrush;
                BackgroundBorder.BorderBrush = selectedStroke;
            }
            else
            {
                Icon.Visibility = Visibility.Collapsed;
                BackgroundBorder.Background = normalBackgroundBrush;
                BackgroundBorder.BorderBrush = normalStroke;
            }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(FilterBase), typeof(FilterDropletControl), new PropertyMetadata(null, OnFilterPropertyChanged));

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
            ((FilterDropletControl)sender).OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            Label.Text = Filter.DisplayName;
        }

        // Constructor
        public FilterDropletControl()
        {
            InitializeComponent();
        }
    }
}
