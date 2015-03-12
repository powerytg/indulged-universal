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
    public class GroupCollection : ObservableCollection<FlickrGroup>, ISupportIncrementalLoading
    {
        // Events
        public EventHandler LoadingStarted;
        public EventHandler LoadingComplete;

        private int page = 1;
        private int maxPage = 10;

        public string Query { get; set; }
        public int TotalCount { get; set; }

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
                return (this.Count < TotalCount && page < maxPage);
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
                var retVal = await APIService.Instance.SearchGroupsAsync(Query, paramDict);
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

                var evt = StorageService.Instance.OnGroupSearchResult(retVal.Result);
                TotalCount = evt.TotalCount;
                foreach (var group in evt.NewGroups)
                {
                    Add(group);
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
