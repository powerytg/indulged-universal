using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.PhotoStream;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Indulged.UI.Models
{
    public class PhotoTileCollection : ObservableCollection<PhotoTile>, ISupportIncrementalLoading
    {
        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        private int page = 1;
        private int maxPage = 8;

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

                var tiles = factory.GeneratePhotoTiles(newPhotos);
                //var tiles = factory.GenerateLinearPhotoTiles(newPhotos);
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
    }
}
