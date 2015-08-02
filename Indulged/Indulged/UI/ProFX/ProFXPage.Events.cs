using Indulged.UI.Common.Controls;
using Indulged.UI.Common.Controls.Events;
using Indulged.UI.ProFX.Events;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Indulged.UI.ProFX
{
    public partial class ProFXPage
    {
        private void InitializeEventListeneres()
        {
            filterManager.InvalidatePreview += OnPreviewInvalidated;            
            filterManager.FilterAdded += OnFilterAdded;
            filterManager.FilterCountChanged += OnFilterCountChanged;

            FilterGalleryView.OnDismiss += OnFilterGalleryDismiss;
            FilterGalleryView.RequestFilter += OnFilterRequested;
            
            ActiveFilterView.OnDismiss += OnActiveFilterViewDismiss;
            ActiveFilterView.RequestFilter += OnFilterRequested;
            ActiveFilterView.RequestCropFilter += OnCropFilterRequested;
            ActiveFilterView.RequestFilterGallery += OnRequestFilterGalleryFromFilterList;

            FilterContainerView.OnDismiss += OnFilterContainerDismiss;
            FilterContainerView.OnDelete += OnFilterDeleted;

            ViewFinder.CropAreaChanged += OnCropAreaChanged;
            CropView.OnDismiss += OnCropFilterDismiss;
            CropView.OnDelete += OnCropFilterDelete;

            /*
            UploaderPage.RequestDismiss += OnUploaderRequestDismiss;
            UploaderPage.RequestExit += OnUploaderRequestExit;
            */
        }

        private void OnViewFinderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewFinder.SizeChanged -= OnViewFinderSizeChanged;

            SampleOriginalImage();
            ViewFinder.Source = currentPreviewBitmap;
        }

        private void OnPreviewInvalidated(object sender, EventArgs e)
        {
            UpdatePreviewAsync();
        }

        private void OnFilterGalleryDismiss(object sender, EventArgs e)
        {
            DismissFilterGallery(true);
        }

        private void OnActiveFilterViewDismiss(object sender, EventArgs e)
        {
            DismissActiveFilterList();
        }

        private void OnRequestFilterGalleryFromFilterList(object sender, EventArgs e)
        {
            DismissActiveFilterList(false, () =>
            {
                ShowFilterGallery();
            });
        }

        private void OnFilterContainerDismiss(object sender, EventArgs e)
        {
            DismissFilterOSD();
        }

        private void OnCropFilterRequested(object sender, EventArgs e)
        {
            DismissActiveFilterList(false, () =>
            {
                OnCropButtonClick(this, null);
            });
        }

        private void OnFilterRequested(object sender, RequestFilterEventArgs e)
        {
            if (FilterGalleryView.Visibility != Visibility.Collapsed)
            {
                DismissFilterGallery(false, () =>
                {
                    ShowFilterOSD(e.Filter);
                });
            }
            else if (ActiveFilterView.Visibility != Visibility.Collapsed)
            {
                DismissActiveFilterList(false, () =>
                {
                    ShowFilterOSD(e.Filter);
                });
            }
            else
            {
                ShowFilterOSD(e.Filter);
            }
        }

        private void OnFilterAdded(object sender, AddFilterEventArgs e)
        {
            e.Filter.CurrentImage = currentPreviewBitmap;
            e.Filter.OriginalImageWidth = originalImageWidth;
            e.Filter.OriginalImageHeight = originalImageHeight;
            e.Filter.OriginalPreviewImageWidth = originalPreviewBitmapWidth;
            e.Filter.OriginalPreviewImageHeight = originalPreviewBitmapHeight;

            if (FilterGalleryView.Visibility != Visibility.Collapsed)
            {
                DismissFilterGallery(false, () =>
                {
                    ShowFilterOSD(e.Filter);
                });
            }
            else
            {
                ShowFilterOSD(e.Filter);
            }
        }

        private void OnFilterDeleted(object sender, DeleteFilterEventArgs e)
        {
            filterManager.DeleteFilter(e.Filter);

            if (FilterContainerView.Visibility != Visibility.Collapsed)
            {
                DismissFilterOSD();
            }
        }

        private void OnFilterCountChanged(object sender, EventArgs e)
        {
            if (filterManager.AppliedFilters.Count == 0)
            {
                FilterCountLabel.Text = "0 FILTER";
                FilterListButton.IsEnabled = false;
            }
            else if (filterManager.AppliedFilters.Count == 1)
            {
                FilterCountLabel.Text = "1 FILTER";
                FilterListButton.IsEnabled = true;
            }
            else
            {
                FilterCountLabel.Text = filterManager.AppliedFilters.Count.ToString() + " FILTERS";
                FilterListButton.IsEnabled = true;
            }
        }

        private void AutoButton_Click(object sender, RoutedEventArgs e)
        {
            PerformAutoEnhance();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFilterGallery();
        }

        private void OnCropButtonClick(object sender, RoutedEventArgs e)
        {
            ViewFinder.Source = originalPreviewBitmap;

            ShowCropOSD(() =>
            {
                ViewFinder.ShowCropFinder();
            });
        }

        private void OnCropAreaChanged(object sender, CropAreaChangedEventArgs e)
        {
            filterManager.CropFilter.UpdateCropRect(e.X, e.Y, e.Width, e.Height);
        }

        private void OnCropFilterDismiss(object sender, EventArgs e)
        {
            ViewFinder.DismissCropFinder();

            DismissCropOSD(() =>
            {
                filterManager.CropFilter.CurrentImage = currentPreviewBitmap;
                filterManager.CropFilter.OriginalImageWidth = originalImageWidth;
                filterManager.CropFilter.OriginalImageHeight = originalImageHeight;
                filterManager.CropFilter.OriginalPreviewImageWidth = originalPreviewBitmapWidth;
                filterManager.CropFilter.OriginalPreviewImageHeight = originalPreviewBitmapHeight;

                ViewFinder.Source = currentPreviewBitmap;
                filterManager.ApplyCrop();
            });
        }

        private void OnCropFilterDelete(object sender, EventArgs e)
        {
            ViewFinder.DismissCropFinder();
            DismissCropOSD(() =>
            {
                ViewFinder.Source = currentPreviewBitmap;
                filterManager.DiscardCrop();
            });
        }

        private void OnFilterListButtonClick(object sender, RoutedEventArgs e)
        {
            ShowActiveFilterList();
        }

        private void OnResetTransformButtonClick(object sender, RoutedEventArgs e)
        {
            if (!filterManager.AppliedFilters.Contains(filterManager.CropFilter))
            {
                return;
            }

            var dialog = ModalPopup.Show("This will reset cropping and rotation settings.\n\nDo you wish to continue?",
                   "Reset Transform", new List<string> { "Confirm", "Cancel" });
            dialog.DismissWithButtonClick += (s, args) =>
            {
                int buttonIndex = (args as ModalPopupEventArgs).ButtonIndex;
                if (buttonIndex == 0)
                {
                    filterManager.ResetTransform();
                }
            };
        }

        private void OnClearFXFiltersButtonClick(object sender, RoutedEventArgs e)
        {
            if (!filterManager.HasAppliedFilterOtherThan(Filters.FilterCategory.Transform))
            {
                return;
            }

            var dialog = ModalPopup.Show("All filters (except for crop and rotation) will be removed. \n\nDo you with to continue?",
                   "Clear Effects", new List<string> { "Confirm", "Cancel" });
            dialog.DismissWithButtonClick += (s, args) =>
            {
                int buttonIndex = (args as ModalPopupEventArgs).ButtonIndex;
                if (buttonIndex == 0)
                {
                    filterManager.ClearAllFiltersOtherThan(Filters.FilterCategory.Transform);
                    OnFilterCountChanged(this, null);
                    UpdatePreviewAsync();
                }
            };

        }
    }
}
