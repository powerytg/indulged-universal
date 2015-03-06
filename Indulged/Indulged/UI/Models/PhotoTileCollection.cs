using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.PhotoStream.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Data;

namespace Indulged.UI.Models
{
    public class PhotoTileCollection : ObservableCollection<PhotoTile>, ISupportIncrementalLoading
    {
        protected string displayStyle;
        public string DisplayStyle
        {
            get
            {
                return displayStyle;
            }

            set
            {
                if (displayStyle != value)
                {
                    displayStyle = value;
                    Reflow();
                }

            }
        }

        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        private int page = 1;
        private int maxPage = 10;

        private PhotoTileFactory factory;
        private FlickrPhotoStream stream;
        public FlickrPhotoStream Stream
        {
            get
            {
                return stream;
            }

            set
            {
                stream = value;
                this.Clear();

                // Reset page to 1
                page = 1;

                // Create a photo group factory
                factory = new PhotoTileFactory();
            }
        }

        public bool HasMoreItems
        {
            get
            {
                return (Stream.Photos.Count < Stream.PhotoCount && page < maxPage);
            }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (LoadingStarted != null)
            {
                LoadingStarted(this, null);
            }

            return AsyncInfo.Run(async c =>
            {
                var paramDict = new Dictionary<string, string>();
                paramDict["page"] = page.ToString();
                paramDict["extras"] = APIService.CommonPhotoExtraParameters;
                paramDict["per_page"] = APIService.PerPage.ToString();
                var retVal = await APIService.Instance.GetPhotoStreamAsync(Stream, paramDict);
                if (!retVal.Success)
                {
                    if (LoadingComplete != null)
                    {
                        LoadingComplete(this, null);
                    }

                    return new LoadMoreItemsResult()
                    {
                        Count = 0
                    };
                }

                var newPhotos = StorageService.Instance.OnPhotoStreamReturned(stream, retVal.Result);
                var tiles = GenerateTiles(newPhotos);
                foreach (var tile in tiles)
                {
                    this.Add(tile);
                }

                if (LoadingComplete != null)
                {
                    LoadingComplete(this, null);
                }

                page++;

                return new LoadMoreItemsResult()
                {
                    Count = (uint) APIService.PerPage
                };
            });
        }

        private List<PhotoTile> GenerateTiles(List<FlickrPhoto> photos)
        {
            if (displayStyle == StreamLayoutStyle.Journal)
            {
                return factory.GenerateJournalPhotoTiles(photos);
            }
            else if (displayStyle == StreamLayoutStyle.Magazine)
            {
                return factory.GenerateMagazineTiles(photos);
            }
            else if (displayStyle == StreamLayoutStyle.Linear)
            {
                return factory.GenerateLinearPhotoTiles(photos);
            }

            // Shouldn't ever reach here!
            return new List<PhotoTile>();
        }

        private void Reflow()
        {
            // Ignore if there're no items
            if (Count == 0)
            {
                return;
            }

            var photos = new List<FlickrPhoto>();
            foreach (var tile in Items)
            {
                photos.AddRange(tile.Photos);
            }

            // Re-generate tiles
            Clear();
            var tiles = GenerateTiles(photos);
            foreach (var tile in tiles)
            {
                this.Add(tile);
            }

        }

    }
}
