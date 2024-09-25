// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class HiddenChannelEventData : ChannelEventDataAbstract
    {
        [Serializable]
        private class DataProperties
        {
            [JsonProperty("allow_auto_unhide")] internal readonly bool allowAutoUnhide = false;
            [JsonProperty("hide_previous_messages")] internal readonly bool hidePreviousMessages = false;
        }

        [JsonProperty("ts_message_offset")] internal readonly long messageOffset;

        internal override ChannelReceiveWsReceiveCommand.CategoryType CategoryType => ChannelReceiveWsReceiveCommand.CategoryType.Hidden;

        internal SbGroupChannelHiddenState HiddenState { get; private set; }
        internal bool HidePreviousMessages { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (base.data != null)
            {
                DataProperties dataProperties = base.data.ToObjectIgnoreException<DataProperties>();
                if (dataProperties != null)
                {
                    HidePreviousMessages = dataProperties.hidePreviousMessages;
                    HiddenState = dataProperties.allowAutoUnhide ? SbGroupChannelHiddenState.HiddenAllowAutoUnhide : SbGroupChannelHiddenState.HiddenPreventAutoUnhide;
                }
            }
        }

        internal static HiddenChannelEventData DeserializeFromJson(string inJsonString)
        {
            return NewtonsoftJsonExtension.DeserializeObjectIgnoreException<HiddenChannelEventData>(inJsonString);
        }
    }
}