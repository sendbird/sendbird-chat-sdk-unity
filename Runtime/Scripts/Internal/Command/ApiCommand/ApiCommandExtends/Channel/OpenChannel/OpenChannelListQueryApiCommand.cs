// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class OpenChannelListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, string inNameContains, string inUrlContains, string inCustomType,
                             bool inShowFrozen, bool inShowMetadata, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Open)}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("name_contains", inNameContains);
                InsertQueryParamIfNotNullOrEmpty("url_contains", inUrlContains);
                InsertQueryParamIfNotNullOrEmpty("custom_type", inCustomType);
                InsertQueryParamIfNotNullOrEmpty("show_frozen", inShowFrozen);
                InsertQueryParamIfNotNullOrEmpty("show_metadata", inShowMetadata);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal string token;
            internal List<OpenChannelDto> openChannelDtos;

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
                            case "next": token = JsonStreamingHelper.ReadString(reader); break;
                            case "channels": openChannelDtos = OpenChannelDto.ReadListFromJson(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}