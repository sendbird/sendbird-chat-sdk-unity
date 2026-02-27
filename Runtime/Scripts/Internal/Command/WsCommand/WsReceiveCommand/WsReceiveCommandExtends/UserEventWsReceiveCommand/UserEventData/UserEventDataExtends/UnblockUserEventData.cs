// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UnblockUserEventData : UserEventDataAbstract
    {
        internal override UserEventWsReceiveCommand.CategoryType CategoryType => UserEventWsReceiveCommand.CategoryType.Unblock;

        internal UserDto BlockerUserDto { get; private set; }
        internal UserDto BlockeeUserDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                JToken blockerToken = base.data["blocker"];
                if (blockerToken != null)
                    BlockerUserDto = UserDto.ReadUserDtoFromJsonString(blockerToken.ToString(Formatting.None));

                JToken blockeeToken = base.data["blockee"];
                if (blockeeToken != null)
                    BlockeeUserDto = UserDto.ReadUserDtoFromJsonString(blockeeToken.ToString(Formatting.None));
            }
        }

        internal static UnblockUserEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UnblockUserEventData>(inJsonString);
        }
    }
}