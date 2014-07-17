using Indulged.API.Utils;
using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Factories
{
    public class FlickrActivityFactory
    {
        public static FlickrPhotoActivity ActivityWithJObject(JObject json)
        {
            // Filter out unsupported item types
            string itemType = json["type"].ToString();
            if (itemType != "photo")
                return null;

            string itemId = json["id"].ToString();
            FlickrPhotoActivity activity = null;
            if(StorageService.Instance.ActivityCache.ContainsKey(itemId))
            {
                activity = StorageService.Instance.ActivityCache[itemId];
            }
            else
            {
                activity = new FlickrPhotoActivity();
                activity.ResourceId = itemId;
                StorageService.Instance.ActivityCache[itemId] = activity;
            }

            // Title
            activity.Title = json["title"]["_content"].ToString();

            // Photo
            activity.TargetPhoto = FlickrPhotoFactory.PhotoWithPhotoActivityJObject(json);

            // Events
            activity.Events.Clear();
            foreach (JObject eventJson in json["activity"]["event"])
            {
                FlickrPhotoActivityEventBase evt = null;
                string eventType = eventJson["type"].ToString();
                if (eventType == "fave")
                {
                    FlickrUser favUser = FlickrUserFactory.UserWithActivityEventJObject(eventJson);
                    if (favUser != null)
                    {
                        activity.FavUsers.Add(favUser);
                    }
                }
                else if (eventType == "comment")
                {
                    evt = new FlickrPhotoActivityCommentEvent();
                    FlickrPhotoActivityCommentEvent commentEvt = evt as FlickrPhotoActivityCommentEvent;
                    commentEvt.Message = eventJson["_content"].ToString();
                    evt.EventUser = FlickrUserFactory.UserWithActivityEventJObject(eventJson);
                    evt.CreationDate = eventJson["dateadded"].ToString().ToDateTime();

                    activity.Events.Add(evt);
                }
            }


            return activity;
        }
    }
}
