// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class JoinChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("users")] internal readonly List<MemberDto> members = null;
        }

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Join;

        internal List<MemberDto> MemberDtos { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null && dataProperties.members != null)
                {
                    MemberDtos = dataProperties.members;
                }
                
                if (MemberDtos == null)
                {
                    MemberDto member = base.data.ToObjectIgnoreException<MemberDto>();
                    MemberDtos = new List<MemberDto>(new MemberDto[] { member });
                }
            }
        }

        internal static JoinChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<JoinChannelEventData>(inJsonString);
        }
    }
}