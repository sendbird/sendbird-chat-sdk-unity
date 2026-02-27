// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UnregisterAllPushTokenApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.DeleteRequest
        {
            internal Request(string inUserId, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
            }

            internal sealed class Response : ApiCommandAbstract.Response
            {
                internal long deviceTokenLastDeletedAt;

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
                                case "device_token_last_deleted_at": deviceTokenLastDeletedAt = JsonStreamingHelper.ReadLong(reader); break;
                                default: JsonStreamingHelper.SkipValue(reader); break;
                            }
                        }
                    }
                }
            }
        }
    }
}