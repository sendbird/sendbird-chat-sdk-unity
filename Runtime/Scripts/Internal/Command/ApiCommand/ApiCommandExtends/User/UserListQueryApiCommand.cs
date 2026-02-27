// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UserListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inToken, int inLimit, List<string> inUserIds, string inMetaDataKye, List<string> inMetaDataValues, 
                             string inNicknameStartsWith, ResultHandler inResultHandler)
            {
                Url = $"{USERS_PREFIX_URL}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("token", inToken);
                InsertQueryParamIfNotNullOrEmpty("limit", inLimit);
                InsertQueryParamIfNotNullOrEmpty("nickname_startswith", inNicknameStartsWith);
                InsertQueryParamWithListIfNotNullOrEmpty("user_ids", inUserIds);
                
                InsertQueryParamIfNotNullOrEmpty("metadatakey", inMetaDataKye);
                InsertQueryParamWithListIfNotNullOrEmpty("metadatavalues_in", inMetaDataValues);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal string token;
            internal List<UserDto> userDtos;

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
                            case "users": userDtos = UserDto.ReadUserDtoListFromJson(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }
        }
    }
}