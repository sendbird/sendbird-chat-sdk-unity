// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetChannelInvitationPreferenceApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/channel_invitation_preference";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }
        
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal bool autoAccept;

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "auto_accept": autoAccept = JsonStreamingHelper.ReadBool(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}