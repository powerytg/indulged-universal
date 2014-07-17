using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Utils;

namespace Indulged.API.Storage.Factories
{
    public class TopicReplyFactory
    {
        public static FlickrReply TopicReplyWithJObject(JObject json, FlickrTopic topic)
        {
            // Get reply id
            string replyId = json["id"].ToString();
            FlickrReply reply = null;
            if (topic.ReplyCache.ContainsKey(replyId))
            {
                reply = topic.ReplyCache[replyId];
            }
            else
            {
                reply = new FlickrReply();
                reply.ResourceId = replyId;
                topic.ReplyCache[replyId] = reply;
            }

            // Parse user
            reply.Author = FlickrUserFactory.UserWithTopicJObject(json);
            reply.IsAdmin = (json["role"].ToString() == "admin");
            reply.CreationDate = json["datecreate"].ToString().ToDateTime();

            // Message
            reply.Message = json["message"]["_content"].ToString();

            // Replies
            reply.CanDelete = (json["can_delete"].ToString() == "1");

            return reply;
        }
    }
}
