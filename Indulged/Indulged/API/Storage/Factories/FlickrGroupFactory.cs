using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;

namespace Indulged.API.Storage.Factories
{
    public class FlickrGroupFactory
    {
        public static FlickrGroup GroupWithJObject(JObject json)
        {
            FlickrGroup group = null;

            // Get group id
            string groupId;
            JToken nsidValue;
            if (json.TryGetValue("nsid", out nsidValue))
            {
                groupId = json["nsid"].ToString();
            }
            else
            {
                groupId = json["id"].ToString();
            }

            
            if (StorageService.Instance.GroupCache.ContainsKey(groupId))
                group = StorageService.Instance.GroupCache[groupId];
            else
            {
                group = new FlickrGroup(groupId);
                StorageService.Instance.GroupCache[group.ResourceId] = group;
            }

            group.Farm = json["iconfarm"].ToString();
            group.Server = json["iconserver"].ToString();

            JToken nameValue;
            if (json.TryGetValue("name", out nameValue))
            {
                if (nameValue.GetType() == typeof(JValue))
                {
                    group.Name = json["name"].ToString();
                }
                else
                {
                    group.Name = json["name"]["_content"].ToString();
                }

            }

            JToken descValue;
            if (json.TryGetValue("description", out descValue))
            {
                if(descValue.GetType() == typeof(JValue))
                {
                    group.Description = json["description"].ToString();
                }
                else
                {
                    group.Description = json["description"]["_content"].ToString();
                }

            }

            // Is invitation_only
            JToken invitationValue;
            if(json.TryGetValue("invitation_only", out invitationValue)){
                group.IsInvitationOnly = (json["invitation_only"].ToString() == "1");
            }

            // Is admin
            JToken adminValue;
            if (json.TryGetValue("admin", out adminValue))
            {
                group.IsAdmin = (json["admin"].ToString() == "1");
            }

            // Is 18+
            JToken explictValue;
            if (json.TryGetValue("eighteenplus", out explictValue))
            {
                group.IsEighteenPlus = (json["eighteenplus"].ToString() == "1");
            }

            JToken rulesValue;
            if (json.TryGetValue("rules", out rulesValue))
            {
                group.Rules = json["rules"]["_content"].ToString();
            }

            JToken poolValue;
            if (json.TryGetValue("pool_count", out poolValue))
            {
                if(poolValue.GetType() == typeof(JValue))
                    group.PhotoStream.PhotoCount = int.Parse(json["pool_count"].ToString());
                else
                    group.PhotoStream.PhotoCount = int.Parse(json["pool_count"]["_content"].ToString());
            }

            JToken topicValue;
            if (json.TryGetValue("topic_count", out topicValue))
            {
                if (topicValue.GetType() == typeof(JValue))
                    group.TopicCount = int.Parse(json["topic_count"].ToString());
                else
                    group.TopicCount = int.Parse(json["topic_count"]["_content"].ToString());
            }

            JToken membersValue;
            if (json.TryGetValue("members", out membersValue))
            {
                if (membersValue.GetType() == typeof(JValue))
                    group.MemberCount = int.Parse(json["members"].ToString());

                else
                    group.MemberCount = int.Parse(json["members"]["_content"].ToString());
            }

            JToken privacyValue;
            if (json.TryGetValue("privacy", out privacyValue))
            {
                int privacyId = int.Parse(json["privacy"]["_content"].ToString());
                group.Privacy = (FlickrGroupPrivicy)privacyId;
            }

            JToken throttleValue;
            if (json.TryGetValue("throttle", out throttleValue))
            {
                JObject throttleObject = (JObject)json["throttle"];

                JToken modeValue;
                if(throttleObject.TryGetValue("mode", out modeValue))
                {
                    group.ThrottleMode = json["throttle"]["mode"].ToString();
                }

                JToken countValue;
                if(throttleObject.TryGetValue("count", out countValue))
                {
                    group.ThrottleMaxCount = int.Parse(json["throttle"]["count"].ToString());
                }

                JToken remainingValue;
                if (throttleObject.TryGetValue("remaining", out remainingValue))
                {
                    group.ThrottleRemainingCount = int.Parse(json["throttle"]["remaining"].ToString());
                }
            }

            return group;
        }

    }
}
