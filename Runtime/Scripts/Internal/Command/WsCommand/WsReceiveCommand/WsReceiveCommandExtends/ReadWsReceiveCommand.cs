// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ReadWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("channel_type")] private readonly string _channelType = null;
        [JsonProperty("ts")] internal readonly long timestamp;
        [JsonProperty("user")] internal readonly UserDto userDto = null;

        internal ReadWsReceiveCommand() : base(WsCommandType.Read) { }

        internal SbChannelType ChannelType { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);
        }

        internal static ReadWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ReadWsReceiveCommand>(inJsonString);
        }
    }
}