using Indulged.API.Utils;
using Indulged.API.Networking.Events;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Factories;
using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        private void OnGroupInfoReturned(object sender, APIEventArgs e)
        {
            JObject rootJson = JObject.Parse(e.Response);
            JObject json = (JObject)rootJson["group"];
            FlickrGroup group = FlickrGroupFactory.GroupWithJObject(json);
            if (group == null)
                return;

            group.IsInfoRetrieved = true;

            var evt = new StorageEventArgs();
            evt.GroupId = group.ResourceId;

            GroupInfoUpdated.DispatchEvent(this, evt);
        }
       
        private void OnGroupTopicsReturned(object sender, APIEventArgs e)
        {
            if (!GroupCache.ContainsKey(e.GroupId))
                return;

            FlickrGroup group = GroupCache[e.GroupId];

            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["topics"];
            int TotalCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["per_page"].ToString());

            List<FlickrTopic> newTopics = new List<FlickrTopic>();
            if (TotalCount > 0)
            {
                foreach (var entry in rootJson["topic"])
                {
                    JObject json = (JObject)entry;
                    FlickrTopic topic = FlickrTopicFactory.TopicWithJObject(json, group);

                    if (!group.Topics.Contains(topic))
                    {
                        group.Topics.Add(topic);
                        newTopics.Add(topic);
                    }
                }

            }

            // Dispatch event            
            var evt = new StorageEventArgs();
            evt.GroupId = group.ResourceId;
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewTopics = newTopics;
            GroupTopicsUpdated.DispatchEvent(this, evt);
        }

        private void OnTopicAdded(object sender, APIEventArgs e)
        {
            FlickrGroup group = GroupCache[e.GroupId];

            JObject rawJson = JObject.Parse(e.Response);
            string newTopicId = rawJson["topic"]["id"].ToString();

            FlickrTopic newTopic = new FlickrTopic();
            newTopic.ResourceId = newTopicId;
            newTopic.Subject = e.Subject;
            newTopic.Message = e.Message;
            newTopic.Author = CurrentUser;
            newTopic.CreationDate = DateTime.Now;

            group.TopicCache[newTopicId] = newTopic;
            group.Topics.Insert(0, newTopic);
            group.TopicCount++;

            var evt = new StorageEventArgs();
            evt.SessionId = e.SessionId;
            evt.GroupId = group.ResourceId;
            evt.NewTopics = new List<FlickrTopic> { newTopic };
            GroupTopicAdded.DispatchEvent(this, evt);
        }

        private void OnTopicReplyAdded(object sender, APIEventArgs e)
        {
            FlickrGroup group = GroupCache[e.GroupId];
            FlickrTopic topic = group.TopicCache[e.TopicId];

            JObject rawJson = JObject.Parse(e.Response);
            string newReplyId = rawJson["reply"]["id"].ToString();

            FlickrReply newReply = new FlickrReply();
            newReply.ResourceId = newReplyId;
            newReply.Message = e.Message;
            newReply.Author = CurrentUser;
            newReply.CreationDate = DateTime.Now;

            topic.ReplyCache[newReplyId] = newReply;
            topic.Replies.Insert(0, newReply);
            topic.ReplyCount++;

            var evt = new StorageEventArgs();
            evt.SessionId = e.SessionId;
            evt.GroupId = group.ResourceId;
            evt.TopicId = topic.ResourceId;
            evt.NewReplies = new List<FlickrReply> { newReply };
            GroupTopicReplyAdded.DispatchEvent(this, evt);
        }

        private void OnPhotoAddedToGroup(object sender, APIEventArgs e)
        {
            // Update group throttle info
            FlickrGroup group = GroupCache[e.GroupId];
            FlickrPhoto photo = PhotoCache[e.PhotoId];

            if (group.ThrottleMode != "none")
                group.ThrottleRemainingCount--;

            if (!group.PhotoStream.Photos.Contains(photo))
            {
                group.PhotoStream.Photos.Insert(0, photo);
                group.PhotoStream.PhotoCount++;

                // Dispatch event
                var ae = new StorageEventArgs();
                ae.PhotoId = photo.ResourceId;
                ae.GroupId = group.ResourceId;
                GroupPhotoAdded.DispatchEvent(this, ae);
            }
        }

        private void OnPhotoRemovedFromGroup(object sender, APIEventArgs e)
        {
            // Update group throttle info
            FlickrGroup group = GroupCache[e.GroupId];
            FlickrPhoto photo = PhotoCache[e.PhotoId];

            if (group.ThrottleMode != "none")
                group.ThrottleRemainingCount++;

            if (group.PhotoStream.Photos.Contains(photo))
            {
                group.PhotoStream.Photos.Remove(photo);
                group.PhotoStream.PhotoCount--;

                // Dispatch event
                var evt = new StorageEventArgs();
                evt.PhotoId = photo.ResourceId;
                evt.GroupId = group.ResourceId;
                GroupPhotoRemoved.DispatchEvent(this, evt);
            }
        }

        private void OnTopicRepliesReturned(object sender, APIEventArgs e)
        {
            if (!GroupCache.ContainsKey(e.GroupId))
                return;

            FlickrGroup group = GroupCache[e.GroupId];

            if (!group.TopicCache.ContainsKey(e.TopicId))
                return;

            var topic = group.TopicCache[e.TopicId];

            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["replies"];
            JObject topicJson = (JObject)rootJson["topic"];

            int TotalCount = int.Parse(topicJson["total"].ToString());
            int page = int.Parse(topicJson["page"].ToString());
            int numPages = int.Parse(topicJson["pages"].ToString());
            int perPage = int.Parse(topicJson["per_page"].ToString());

            List<FlickrReply> newReplies = new List<FlickrReply>();
            if (TotalCount > 0)
            {
                foreach (var entry in rootJson["reply"])
                {
                    JObject json = (JObject)entry;
                    FlickrReply reply = TopicReplyFactory.TopicReplyWithJObject(json, topic);

                    if (!topic.Replies.Contains(reply))
                    {
                        topic.Replies.Add(reply);
                        newReplies.Add(reply);
                    }
                }

            }

            // Dispatch event            
            var evt = new StorageEventArgs();
            evt.GroupId = group.ResourceId;
            evt.TopicId = topic.ResourceId;
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewReplies = newReplies;
            TopicRepliesUpdated.DispatchEvent(this, evt);
        }

        private void OnGroupJoined(object sender, APIEventArgs e)
        {
            FlickrGroup group = GroupCache[e.GroupId];
            if (group == null)
                return;

            CurrentUser.GroupIds.Add(group.ResourceId);

            var evt = new StorageEventArgs();
            evt.GroupId = group.ResourceId;
            GroupJoined.DispatchEvent(this, evt);
        }
    }
}
