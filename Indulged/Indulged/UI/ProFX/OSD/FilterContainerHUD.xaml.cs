using Indulged.UI.ProFX.Events;
using Indulged.UI.ProFX.Filters;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.OSD
{
    public sealed partial class FilterContainerHUD : UserControl
    {
        // Events
        public EventHandler OnDismiss;
        public EventHandler<DeleteFilterEventArgs> OnDelete;

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(FilterBase), typeof(FilterContainerHUD), new PropertyMetadata(null, OnFilterPropertyChanged));

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
            ((FilterContainerHUD)sender).OnFilterChanged();
        }

        private void OnFilterChanged()
        {
            TitleLabel.Text = Filter.StatusBarName;
            FilterContainer.Children.Clear();
            FilterContainer.Children.Add(Filter);
            //FilterContainer.Content = Filter;

            // Re-calculate height
            this.Height = Filter.Height + 140;
        }

        // Constructor
        public FilterContainerHUD()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDismiss != null)
            {
                OnDismiss(this, null);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDelete != null)
            {
                var evt = new DeleteFilterEventArgs();
                evt.Filter = Filter;
                OnDelete(this, evt);
            }
        }

        private void OSDToggle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (OnDismiss != null)
            {
                OnDismiss(this, null);
            }
        }
    }
}
