using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Data;

namespace Indulged.UI.Models
{
    public class TopicCollection : ObservableCollection<FlickrTopic>, ISupportIncrementalLoading
    {
        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        private int page = 1;
        private int maxPage = 10;

        private FlickrGroup group;
        public FlickrGroup Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                this.Clear();

                // Reset page to 1
                page = 1;
            }
        }

        public bool HasMoreItems
        {
            get
            {
                return (this.Count < group.TopicCount && page < maxPage);
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
                paramDict["per_page"] = APIService.PerPage.ToString();
                var retVal = await APIService.Instance.GetGroupTopicsAsync(group.ResourceId, paramDict);
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

                var newTopics = StorageService.Instance.OnGroupTopicsReturned(group.ResourceId, retVal.Result);
                foreach (var topic in newTopics)
                {
                    Add(topic);
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
