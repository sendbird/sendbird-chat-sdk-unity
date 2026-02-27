// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetMyMutedInfoApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inChannelUrl, SbChannelType inChannelType, string inUserId, ResultHandler inResultHandler)
            {
                inChannelUrl = WebUtility.UrlEncode(inChannelUrl);
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{ChannelTypeToUrlPrefix(inChannelType)}/{inChannelUrl}/mute/{inUserId}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal bool isMuted;
            internal long remainingDuration;
            internal long startAt;
            internal long endAt;
            internal string description;

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
                            case "is_muted": isMuted = JsonStreamingHelper.ReadBool(reader); break;
                            case "remaining_duration": remainingDuration = JsonStreamingHelper.ReadLong(reader); break;
                            case "start_at": startAt = JsonStreamingHelper.ReadLong(reader); break;
                            case "end_at": endAt = JsonStreamingHelper.ReadLong(reader); break;
                            case "description": description = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}