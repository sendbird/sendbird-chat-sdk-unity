// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("prev_messages")] private readonly List<JObject> _prevMessageDtos;
            [JsonProperty("next_messages")] private readonly List<JObject> _nextMessageDtos;
            [JsonProperty("prev_has_more")] internal readonly bool prevHasMore;
            [JsonProperty("next_has_more")] internal readonly bool nextHasMore;
            [JsonProperty("is_huge_gap")] internal readonly bool isHugeGap;
            [JsonProperty("is_continuous_prev_messages")] internal readonly bool isContinuousPrevMessages;
            [JsonProperty("is_continuous_next_messages")] internal readonly bool isContinuousNextMessages;
            
            internal List<BaseMessageDto> PrevMessageDtos { get; private set; }
            internal List<BaseMessageDto> NextMessageDtos { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (_prevMessageDtos != null && 0 < _prevMessageDtos.Count)
                {
                    PrevMessageDtos = new List<BaseMessageDto>(_prevMessageDtos.Count);
                    foreach (JObject messageJObject in _prevMessageDtos)
                    {
                        if (messageJObject == null)
                            continue;

                        BaseMessageDto baseMessageDto = BaseMessageDto.JObjectToMessageDto(messageJObject);
                        if (baseMessageDto == null)
                            continue;

                        PrevMessageDtos.Add(baseMessageDto);
                    }
                }
                
                if (_nextMessageDtos != null && 0 < _nextMessageDtos.Count)
                {
                    NextMessageDtos = new List<BaseMessageDto>(_nextMessageDtos.Count);
                    foreach (JObject messageJObject in _nextMessageDtos)
                    {
                        if (messageJObject == null)
                            continue;

                        BaseMessageDto baseMessageDto = BaseMessageDto.JObjectToMessageDto(messageJObject);
                        if (baseMessageDto == null)
                            continue;

                        NextMessageDtos.Add(baseMessageDto);
                    }
                }
            }
        }
    }
}