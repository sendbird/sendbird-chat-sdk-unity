// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UpdateOperatorChannelEventData : ChannelEventDataAbstract
    {
        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.UpdateOperator;
        internal List<MemberDto> OperatorMemberDtos { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken operatorsToken = base.data["operators"];
                if (operatorsToken != null)
                {
                    OperatorMemberDtos = MemberDto.ReadListFromJsonString(operatorsToken.ToString(Formatting.None));
                }
            }
        }

        internal static UpdateOperatorChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UpdateOperatorChannelEventData>(inJsonString);
        }
    }
}