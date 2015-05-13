using Indulged.UI.ProFX.Controls;
using Indulged.UI.ProFX.Events;
using Indulged.UI.ProFX.Filters;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.OSD
{
    public sealed partial class FilterGalleryHUD : UserControl
    {
        // Events
        public EventHandler OnDismiss;
        public EventHandler<RequestFilterEventArgs> RequestFilter;

        // Reference to the filter manager
        public FXFilterManager FilterManager { get; set; }

        private List<FilterDropletControl> allDroplets = new List<FilterDropletControl>();

        // Constructor
        public FilterGalleryHUD()
        {
            InitializeComponent();
        }

        public void InitializeFilterDroplets()
        {
            foreach (var filter in FilterManager.AvailableFilters)
            {
                FilterDropletControl droplet = new FilterDropletControl();
                droplet.Filter = filter;
                droplet.Selected = (FilterManager.AppliedFilters.Contains(filter));
                droplet.Margin = new Thickness(5, 5, 5, 5);
                droplet.Tapped += OnDropletTap;
                allDroplets.Add(droplet);

                if (filter.Category == FilterCategory.Enhancement)
                {
                    EnhcnacementFilterContainer.Children.Add(droplet);
                }
                else if (filter.Category == FilterCategory.Color)
                {
                    ColorFilterContainer.Children.Add(droplet);
                }
                else if (filter.Category == FilterCategory.Effect)
                {
                    EffectFilterContainer.Children.Add(droplet);
                }
                //else if (filter.Category == FilterCategory.Transform)
                //{
                //    TransformFilterContainer.Children.Add(droplet);
                //}
            }

        }

        public void UpdateFilterDroplets()
        {
            foreach (var droplet in allDroplets)
            {
                droplet.Selected = (FilterManager.AppliedFilters.Contains(droplet.Filter));
            }
        }

        private void OSDToggle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (OnDismiss != null)
            {
                OnDismiss(this, null);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnDismiss != null)
            {
                OnDismiss(this, null);
            }
        }

        private void OnDropletTap(object sender, TappedRoutedEventArgs e)
        {
            FilterDropletControl droplet = (FilterDropletControl)sender;
            if (!droplet.Selected)
            {
                droplet.Selected = true;
            }

            if (!FilterManager.AppliedFilters.Contains(droplet.Filter))
            {
                FilterManager.AddFilter(droplet.Filter);
            }
            else
            {
                if (RequestFilter != null)
                {
                    var evt = new RequestFilterEventArgs();
                    evt.Filter = droplet.Filter;
                    RequestFilter(this, evt);
                }
            }
        }

    }
}
