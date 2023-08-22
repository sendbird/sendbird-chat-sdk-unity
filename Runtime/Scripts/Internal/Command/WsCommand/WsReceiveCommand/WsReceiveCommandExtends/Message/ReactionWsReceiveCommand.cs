// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class ReactionWsReceiveCommand : WsReceiveCommandAbstract
    {
        [JsonProperty("channel_url")] internal readonly string channelUrl = null;
        [JsonProperty("channel_type")] private readonly string _channelType = null;

        internal ReactionWsReceiveCommand() : base(WsCommandType.Reaction) { }

        internal SbChannelType ChannelType { get; private set; }
        internal ReactionEventDto ReactionEventDto { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_channelType) == false)
                ChannelType = SbChannelTypeExtension.JsonNameToType(_channelType);
        }

        internal static ReactionWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            ReactionWsReceiveCommand wsReceiveCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ReactionWsReceiveCommand>(inJsonString);
            if (wsReceiveCommand != null)
            {
                wsReceiveCommand.ReactionEventDto = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<ReactionEventDto>(inJsonString);
            }

            return wsReceiveCommand;
        }
    }
}