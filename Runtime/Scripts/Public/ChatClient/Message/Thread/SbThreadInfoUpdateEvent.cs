// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Objects representing a thread info event.
    /// </summary>
    /// @since 4.0.0
    public partial class SbThreadInfoUpdateEvent
    {
        /// <summary>
        /// The type of the channel where threaded messages belong.
        /// </summary>
        /// @since 4.0.0
        public SbChannelType ChannelType => _channelType;

        /// <summary>
        /// The unique URL of the channel where threaded messages belong.
        /// </summary>
        /// @since 4.0.0
        public string ChannelUrl => _channelUrl;

        /// <summary>
        /// The unique ID of the message that has threaded replies and holds thread information.
        /// </summary>
        /// @since 4.0.0
        public long TargetMessageId => _targetMessageId;

        /// <summary>
        /// The SbThreadInfo that has information about threaded messages.
        /// </summary>
        /// @since 4.0.0
        public SbThreadInfo ThreadInfo => _threadInfo;
    }
}