using Lumia.Imaging;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Indulged.UI.ProFX.Filters
{
    public enum FilterCategory
    {
        Color,
        Transform,
        Effect,
        Enhancement
    };

    public class FilterBase : UserControl
    {
        // Events
        public EventHandler FilterWillBeRemoved;
        public EventHandler InvalidatePreview;

        private bool _isFilterEnabled = true;
        public bool IsFilterEnabled
        {
            get
            {
                return _isFilterEnabled;
            }

            set
            {
                _isFilterEnabled = value;
            }
        }

        public double OriginalImageWidth { get; set; }
        public double OriginalImageHeight { get; set; }

        public double OriginalPreviewImageWidth { get; set; }
        public double OriginalPreviewImageHeight { get; set; }

        // Current preview image
        public WriteableBitmap CurrentImage { get; set; }

        public string DisplayName { get; set; }
        public string StatusBarName { get; set; }

        public FilterCategory Category { get; set; }

        public virtual bool hasEditorUI
        {
            get
            {
                return true;
            }
        }

        // Filter reference
        public IFilter Filter { get; set; }

        // Filter for final output
        public virtual IFilter FinalOutputFilter
        {
            get
            {
                return Filter;
            }
        }

        public virtual void CreateFilter()
        {
            // Do nothing
        }

        public virtual void OnFilterUIAdded()
        {
            UpdatePreviewAsync();
        }

        public virtual void OnFilterUIDismissed()
        {
            // Do nothing
        }

        public void OnDeleteFilter(object sender, RoutedEventArgs e)
        {
            DeleteFilter();
        }

        protected virtual void DeleteFilter()
        {
            if (FilterWillBeRemoved != null)
            {
                FilterWillBeRemoved(this, null);
            }
        }

        protected virtual void DeleteFilterAsync()
        {
            DeleteFilter();
        }

        public virtual void UpdatePreviewAsync()
        {
            CreateFilter();

            if (InvalidatePreview != null)
            {
                InvalidatePreview(this, null);
            }
        }
    }
}
