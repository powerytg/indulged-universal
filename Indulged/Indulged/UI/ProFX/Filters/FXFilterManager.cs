using Indulged.UI.ProFX.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.ProFX.Filters
{
    public class FXFilterManager
    {
        // Events
        public EventHandler InvalidatePreview;
        public EventHandler<AddFilterEventArgs> FilterAdded;
        public EventHandler FilterCountChanged;

        // Available filters
        public List<FilterBase> AvailableFilters { get; set; }

        // Applied filters
        public List<FilterBase> AppliedFilters { get; set; }

        // Transform filters
        public FXCropFilter CropFilter { get; set; }
        public FXRotationFilter RotationFilter { get; set; }

        // Auto enhance filters
        public List<FilterBase> AutoEnhanceFilters { get; set; }
        public FXAutoEnhanceFilter AutoEnhanceFilter { get; set; }
        public FXVignetteFilter VignetteFilter { get; set; }

        // Constructor
        public FXFilterManager()
        {
            CropFilter = new FXCropFilter();
            RotationFilter = new FXRotationFilter();
            AutoEnhanceFilter = new FXAutoEnhanceFilter();
            VignetteFilter = new FXVignetteFilter();
            AutoEnhanceFilters = new List<FilterBase> { AutoEnhanceFilter, VignetteFilter };

            AppliedFilters = new List<FilterBase>();
            /*
            AvailableFilters = new List<FilterBase> {
                CropFilter, RotationFilter, AutoEnhanceFilter, VignetteFilter,
                new FXBlackWhiteFilter(),
                new FXSolarizeFilter(),
                new FXSharpenFilter(),
                new FXSepiaFilter(),
                new FXPosterizeFilter(),
                new FXOilFilter(),
                new FXPaintingFilter(),
                new FXNegativeFilter(),
                new FXMonoColorFilter(),
                new FXLomoFilter(),
                new FXClarityFilter(),
                new FXHueSaturationFilter(),
                new FXGrayscaleFilter(),
                new FXAntiqueFilter(),
                new FXBlurFilter(),
                new FXColorAdjustmentFilter(),
                new FXLevelFilter(),
                new FXCartoonFilter(),
                new FXColorBoostFilter(),
                new FXColorizationmentFilter(),
                new FXExposureFilter()
            };
             */

            AvailableFilters = new List<FilterBase> {
                CropFilter, RotationFilter, AutoEnhanceFilter, VignetteFilter,
                new FXAntiqueFilter(),
            };

            // Events
            Initialize();
        }

        private void Initialize()
        {
            foreach (var filter in AvailableFilters)
            {
                filter.FilterWillBeRemoved += OnFilterWillBeRemoved;
                filter.InvalidatePreview += OnFilterRequestInvalidatePreview;
            }
        }

        private void OnFilterWillBeRemoved(object sender, EventArgs e)
        {
            FilterBase filter = (FilterBase)sender;
            DeleteFilter(filter);
        }

        private void OnFilterRequestInvalidatePreview(object sender, EventArgs e)
        {
            PerformInvalidatePreview();
        }

        public void PerformInvalidatePreview()
        {
            if (InvalidatePreview != null)
            {
                InvalidatePreview(this, null);
            }
        }

        public void AddFilter(FilterBase filter)
        {
            if (AppliedFilters.Contains(filter))
            {
                return;
            }

            filter.IsFilterEnabled = true;
            AppliedFilters.Add(filter);

            if (FilterAdded != null)
            {
                var evt = new AddFilterEventArgs();
                evt.Filter = filter;
                FilterAdded(this, evt);
            }

            if (FilterCountChanged != null)
            {
                FilterCountChanged(this, null);
            }
        }

        public void DeleteFilter(FilterBase filter)
        {
            if (!AppliedFilters.Contains(filter))
            {
                return;
            }

            AppliedFilters.Remove(filter);

            if (FilterCountChanged != null)
            {
                FilterCountChanged(this, null);
            }

            PerformInvalidatePreview();
        }

        public bool HasAppliedFilterOtherThan(FilterCategory category)
        {
            bool hasOtherFilters = false;
            if (AppliedFilters.Count == 0)
            {
                hasOtherFilters = false;
            }
            else
            {
                foreach (var filter in AppliedFilters)
                {
                    if (filter.Category != category)
                    {
                        hasOtherFilters = true;
                        break;
                    }
                }
            }

            return hasOtherFilters;
        }

        public void ClearAllFiltersOtherThan(FilterCategory category)
        {
            List<FilterBase> filtersToBeRemoved = new List<FilterBase>();

            foreach (var filter in AppliedFilters)
            {
                if (filter.Category != category)
                {
                    filtersToBeRemoved.Add(filter);
                }
            }

            foreach (var filter in filtersToBeRemoved)
            {
                AppliedFilters.Remove(filter);
            }
        }

        public void AutoEnhance()
        {
            foreach (var filter in AutoEnhanceFilters)
            {
                filter.CreateFilter();
                filter.IsFilterEnabled = true;
                AppliedFilters.Add(filter);
            }

            if (FilterCountChanged != null)
            {
                FilterCountChanged(this, null);
            }

            PerformInvalidatePreview();
        }

        public void ApplyCrop()
        {
            CropFilter.CreateFilter();

            if (!AppliedFilters.Contains(CropFilter))
            {
                CropFilter.IsFilterEnabled = true;
                AppliedFilters.Add(CropFilter);

                if (FilterCountChanged != null)
                {
                    FilterCountChanged(this, null);
                }
            }

            PerformInvalidatePreview();
        }

        public void DiscardCrop()
        {
            if (AppliedFilters.Contains(CropFilter))
            {
                AppliedFilters.Remove(CropFilter);

                if (FilterCountChanged != null)
                {
                    FilterCountChanged(this, null);
                }

                PerformInvalidatePreview();
            }
        }

        public void ApplyRotationFilter()
        {
            RotationFilter.CreateFilter();

            if (!AppliedFilters.Contains(RotationFilter))
            {
                RotationFilter.IsFilterEnabled = true;
                AppliedFilters.Add(RotationFilter);

                if (FilterCountChanged != null)
                {
                    FilterCountChanged(this, null);
                }
            }

            PerformInvalidatePreview();
        }

        public void DiscardRotationFilter()
        {
            if (AppliedFilters.Contains(RotationFilter))
            {
                AppliedFilters.Remove(RotationFilter);

                if (FilterCountChanged != null)
                {
                    FilterCountChanged(this, null);
                }
            }

            RotationFilter.Degree = 0;
            PerformInvalidatePreview();
        }

        public void ResetTransform()
        {
            if (AppliedFilters.Contains(RotationFilter))
            {
                AppliedFilters.Remove(RotationFilter);
            }

            if (AppliedFilters.Contains(CropFilter))
            {
                AppliedFilters.Remove(CropFilter);
            }

            if (FilterCountChanged != null)
            {
                FilterCountChanged(this, null);
            }

            PerformInvalidatePreview();
        }
    }
}