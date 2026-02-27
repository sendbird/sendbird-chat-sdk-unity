// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetPushTokensApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, SbPushTokenType inTokenType, string inToken, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/push/{inTokenType.ToJsonName()}/device_tokens";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
            }
        }
        
        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal List<string> deviceTokens;
            private string _pushTokenType;
            internal string token;
            internal bool hasMore;

            internal SbPushTokenType PushTokenType { get; private set; }

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
                            case "device_tokens": deviceTokens = JsonStreamingHelper.ReadStringList(reader); break;
                            case "type": _pushTokenType = JsonStreamingHelper.ReadString(reader); break;
                            case "token": token = JsonStreamingHelper.ReadString(reader); break;
                            case "has_more": hasMore = JsonStreamingHelper.ReadBool(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(_pushTokenType) == false)
                {
                    PushTokenType = SbPushTokenTypeExtension.JsonNameToType(_pushTokenType);
                }
            }
        }
    }
}