using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Utils;
using Indulged.API.Networking.Events;

namespace Indulged.API.Networking
{
    public partial class APIService
    {        
        public async Task<APIResponse> GetGroupInfoAsync(string groupId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.groups.getInfo";
            paramDict["group_id"] = UrlUtils.Encode(groupId);

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Response = retVal.Result;
                GroupInfoReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupInfoFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> JoinGroupAsync(string groupId, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            paramDict["method"] = "flickr.groups.join";
            paramDict["group_id"] = groupId;

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Response = retVal.Result;
                GroupJoinReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupJoinFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> SendJoinGroupRequestAsync(string groupId, string message, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            paramDict["method"] = "flickr.groups.join";
            paramDict["group_id"] = UrlUtils.Encode(groupId);
            paramDict["message"] = UrlUtils.Encode(message);

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Response = retVal.Result;
                GroupJoinRequestReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupJoinRequestFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> AddPhotoToGroupAsync(string photoId, string groupId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();

            paramDict["method"] = "flickr.groups.pools.add";
            paramDict["group_id"] = UrlUtils.Encode(groupId);
            paramDict["photo_id"] = UrlUtils.Encode(photoId);

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.PhotoId = photoId;
                evt.Response = retVal.Result;
                GroupAddPhotoReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.PhotoId = photoId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupAddPhotoFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> RemovePhotoFromGroupAsync(string photoId, string groupId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();

            paramDict["method"] = "flickr.groups.pools.remove";
            paramDict["group_id"] = UrlUtils.Encode(groupId);
            paramDict["photo_id"] = UrlUtils.Encode(photoId);

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.PhotoId = photoId;
                evt.Response = retVal.Result;
                GroupRemovePhotoReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.PhotoId = photoId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupRemovePhotoFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> GetGroupTopicsAsync(string groupId, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            paramDict["method"] = "flickr.groups.discuss.topics.getList";
            paramDict["group_id"] = UrlUtils.Encode(groupId);

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Response = retVal.Result;
                GroupTopicsReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupTopicsFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> AddTopicToGroupAsync(string groupId, string subject, string message, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();

            paramDict["method"] = "flickr.groups.discuss.topics.add";
            paramDict["subject"] = UrlUtils.Encode(subject);
            paramDict["message"] = UrlUtils.Encode(message);

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Subject = subject;
                evt.Message = message;
                evt.Response = retVal.Result;
                GroupAddTopicReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Subject = subject;
                evt.Message = message;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupAddTopicFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> GetTopicRepliesAsync(string topicId, string groupId, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            paramDict["method"] = "flickr.groups.discuss.replies.getList";
            paramDict["topic_id"] = UrlUtils.Encode(topicId);

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.TopicId = topicId;
                evt.Response = retVal.Result;
                TopicRepliesReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.TopicId = topicId;
                evt.ErrorMessage = retVal.ErrorMessage;
                TopicRepliesFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> AddTopicReplyAsync(string topicId, string groupId, string message, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();

            paramDict["method"] = "flickr.groups.discuss.replies.add";
            paramDict["topic_id"] = UrlUtils.Encode(topicId);
            paramDict["message"] = UrlUtils.Encode(message);

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.TopicId = topicId;
                evt.Message = message;
                evt.Response = retVal.Result;
                AddTopicReplyReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.TopicId = topicId;
                evt.Message = message;
                evt.ErrorMessage = retVal.ErrorMessage;
                AddTopicReplyFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
