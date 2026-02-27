// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class GetGroupChannelHugeGapApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal struct Params
            {
                internal string channelUrl;
                internal bool reverse;
                internal List<string> customTypes;
                internal List<string> senderUserIds;
                internal SbMessageTypeFilter messageTypeFilter;
                internal long prevStartTs;
                internal long prevEndTs;
                internal int prevCacheCount;
                internal long nextStartTs;
                internal long nextEndTs;
                internal int nextCacheCount;
                internal bool checkContinuousMessages;
            }

            internal Request(Params inParams, ResultHandler inResultHandler)
            {
                inParams.channelUrl = WebUtility.UrlEncode(inParams.channelUrl);
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}/{inParams.channelUrl}/messages_gap";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty("reverse", inParams.reverse);
                InsertQueryParamWithListIfNotNullOrEmpty("custom_types", inParams.customTypes);
                InsertQueryParamWithListIfNotNullOrEmpty("sender_user_id", inParams.senderUserIds);
                InsertQueryParamIfNotNullOrEmpty("message_type", inParams.messageTypeFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty("prev_start_ts", inParams.prevStartTs);
                InsertQueryParamIfNotNullOrEmpty("prev_end_ts", inParams.prevEndTs);
                InsertQueryParamIfNotNullOrEmpty("prev_cache_count", inParams.prevCacheCount);
                InsertQueryParamIfNotNullOrEmpty("next_start_ts", inParams.nextStartTs);
                InsertQueryParamIfNotNullOrEmpty("next_end_ts", inParams.nextEndTs);
                InsertQueryParamIfNotNullOrEmpty("next_cache_count", inParams.nextCacheCount);
                InsertQueryParamIfNotNullOrEmpty("checking_continuous_messages", inParams.checkContinuousMessages);
            }
        }

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal bool prevHasMore;
            internal bool nextHasMore;
            internal bool isHugeGap;
            internal bool isContinuousPrevMessages;
            internal bool isContinuousNextMessages;
            internal List<BaseMessageDto> PrevMessageDtos { get; private set; }
            internal List<BaseMessageDto> NextMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    ReadFromJsonReader(reader);
                }
            }

            internal override void OnResponseAfterDeserialize(byte[] inResponseBytes)
            {
                if (inResponseBytes == null || inResponseBytes.Length == 0)
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inResponseBytes))
                {
                    ReadFromJsonReader(reader);
                }
            }

            private void ReadFromJsonReader(JsonTextReader reader)
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
                        case "prev_messages": PrevMessageDtos = JsonStreamingHelper.ReadMessageDtoListDirect(reader); break;
                        case "next_messages": NextMessageDtos = JsonStreamingHelper.ReadMessageDtoListDirect(reader); break;
                        case "prev_has_more": prevHasMore = JsonStreamingHelper.ReadBool(reader); break;
                        case "next_has_more": nextHasMore = JsonStreamingHelper.ReadBool(reader); break;
                        case "is_huge_gap": isHugeGap = JsonStreamingHelper.ReadBool(reader); break;
                        case "is_continuous_prev_messages": isContinuousPrevMessages = JsonStreamingHelper.ReadBool(reader); break;
                        case "is_continuous_next_messages": isContinuousNextMessages = JsonStreamingHelper.ReadBool(reader); break;
                        default: JsonStreamingHelper.SkipValue(reader); break;
                    }
                }
            }
        }
    }
}