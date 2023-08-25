// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class DeleteMessageWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("channel_type")] private readonly string _channelType = null;
        [JsonProperty("msg_id")] internal readonly long msgId;
        [JsonProperty("ts")] internal readonly long timestamp;

        internal DeleteMessageWsReceiveCommand() : base(WsCommandType.DeleteMessage) { }

        internal SbChannelType ChannelType { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);
        }

        internal static DeleteMessageWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<DeleteMessageWsReceiveCommand>(inJsonString);
        }
    }
}