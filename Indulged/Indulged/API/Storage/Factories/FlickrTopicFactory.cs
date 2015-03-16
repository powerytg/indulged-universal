using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Utils;
using Windows.Data.Html;

namespace Indulged.API.Storage.Factories
{
    public class FlickrTopicFactory
    {
        public static FlickrTopic TopicWithJObject(JObject json, FlickrGroup group)
        {
            // Get topic id
            string topicId = json["id"].ToString();
            FlickrTopic topic = null;
            if (group.TopicCache.ContainsKey(topicId))
            {
                topic = group.TopicCache[topicId];
            }
            else
            {
                topic = new FlickrTopic();
                topic.ResourceId = topicId;
                group.TopicCache[topicId] = topic;
            }


            // Parse user
            topic.Author = FlickrUserFactory.UserWithTopicJObject(json);
            topic.IsAdmin = (json["role"].ToString() == "admin");
            topic.CreationDate = json["datecreate"].ToString().ToDateTime();

            // Subject
            topic.Subject = json["subject"].ToString();
            if (topic.Subject != null)
            {
                topic.CleanSubject = HtmlUtilities.ConvertToText(topic.Subject);
            }

            // Message
            topic.Message = json["message"]["_content"].ToString();
            if (topic.Message != null)
            {
                topic.CleanMessage = HtmlUtilities.ConvertToText(topic.Message);
            }

            // Replies
            topic.CanReply = (json["can_reply"].ToString() == "1");
            topic.LastReplyDate = json["datelastpost"].ToString().ToDateTime();
            topic.ReplyCount = int.Parse(json["count_replies"].ToString());

            return topic;
        }
    }
}
