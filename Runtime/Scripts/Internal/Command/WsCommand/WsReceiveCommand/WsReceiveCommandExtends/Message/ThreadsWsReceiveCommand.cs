// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ThreadsWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("channel_type")] private readonly string _channelType = null;
        [JsonProperty("parent_message_id")] internal readonly long rootMsgId;
        [JsonProperty("thread_info")] internal readonly ThreadInfoDto threadInfoDto;

        internal ThreadsWsReceiveCommand() : base(WsCommandType.Threads) { }

        internal SbChannelType ChannelType { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);
        }

        internal static ThreadsWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ThreadsWsReceiveCommand>(inJsonString);
        }
    }
}