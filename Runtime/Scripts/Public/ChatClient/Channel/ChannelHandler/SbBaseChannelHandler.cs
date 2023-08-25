// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Channel handler. This handler provides callbacks for events related OpenChannel or GroupChannel. All callbacks are called only when the currently logged-in User is a participant or member of OpenChannel or GroupChannel respectively.
    /// </summary>
    /// @since 4.0.0
    public abstract class SbBaseChannelHandler
    {
        /// @since 4.0.0
        public delegate void MessageReceivedDelegate(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage);

        /// @since 4.0.0
        public delegate void MentionReceivedDelegate(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage);

        /// @since 4.0.0
        public delegate void MessageUpdatedDelegate(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage);

        /// @since 4.0.0
        public delegate void MessageDeletedDelegate(SbBaseChannel inBaseChannel, long inMessageId);

        /// @since 4.0.0
        public delegate void ChannelChangedDelegate(SbBaseChannel inBaseChannel);

        /// @since 4.0.0
        public delegate void ChannelDeletedDelegate(string inChannelUrl, SbChannelType inChannelType);

        /// @since 4.0.0
        public delegate void ReactionUpdatedDelegate(SbBaseChannel inBaseChannel, SbReactionEvent inReactionEvent);

        /// @since 4.0.0
        public delegate void UserMutedDelegate(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser);

        /// @since 4.0.0
        public delegate void UserUnmutedDelegate(SbBaseChannel inBaseChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void UserBannedDelegate(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser);

        /// @since 4.0.0
        public delegate void UserUnbannedDelegate(SbBaseChannel inBaseChannel, SbUser inUser);

        /// @since 4.0.0
        public delegate void ChannelFrozenDelegate(SbBaseChannel inBaseChannel);

        /// @since 4.0.0
        public delegate void ChannelUnfrozenDelegate(SbBaseChannel inBaseChannel);

        /// @since 4.0.0
        public delegate void MetaDataCreatedDelegate(SbBaseChannel inBaseChannel, Dictionary<string, string> inMetaData);

        /// @since 4.0.0
        public delegate void MetaDataUpdatedDelegate(SbBaseChannel inBaseChannel, Dictionary<string, string> inMetaData);

        /// @since 4.0.0
        public delegate void MetaDataDeletedDelegate(SbBaseChannel inBaseChannel, List<string> inKeys);

        /// @since 4.0.0
        public delegate void MetaCountersCreatedDelegate(SbBaseChannel inBaseChannel, Dictionary<string, int> inMetaCounters);

        /// @since 4.0.0
        public delegate void MetaCountersUpdatedDelegate(SbBaseChannel inBaseChannel, Dictionary<string, int> inMetaCounters);

        /// @since 4.0.0
        public delegate void MetaCountersDeletedDelegate(SbBaseChannel inBaseChannel, List<string> inKeys);

        /// @since 4.0.0
        public delegate void OperatorUpdatedDelegate(SbBaseChannel inBaseChannel);

        /// @since 4.0.0
        public delegate void ThreadInfoUpdatedDelegate(SbBaseChannel inBaseChannel, SbThreadInfoUpdateEvent inThreadInfoUpdateEvent);

        /// <summary>
        /// A callback for when a message is received.
        /// </summary>
        /// @since 4.0.0
        public MessageReceivedDelegate OnMessageReceived { get; set; }

        /// <summary>
        /// A callback for when a mention is received.
        /// </summary>
        /// @since 4.0.0
        public MentionReceivedDelegate OnMentionReceived { get; set; }

        /// <summary>
        /// A callback for when a message is deleted.
        /// </summary>
        /// @since 4.0.0
        public MessageDeletedDelegate OnMessageDeleted { get; set; }

        /// <summary>
        /// A callback for when a message is updated.
        /// </summary>
        /// @since 4.0.0
        public MessageUpdatedDelegate OnMessageUpdated { get; set; }

        /// <summary>
        /// A callback for when channel property is changed.
        /// </summary>
        /// @since 4.0.0
        public ChannelChangedDelegate OnChannelChanged { get; set; }

        /// <summary>
        /// A callback for when channel is deleted.
        /// </summary>
        /// @since 4.0.0
        public ChannelDeletedDelegate OnChannelDeleted { get; set; }

        /// <summary>
        /// A callback for when a reactionEvent is updated.
        /// </summary>
        /// @since 4.0.0
        public ReactionUpdatedDelegate OnReactionUpdated { get; set; }

        /// <summary>
        /// A callback for when a User is muted from channel.
        /// </summary>
        /// @since 4.0.0
        public UserMutedDelegate OnUserMuted { get; set; }

        /// <summary>
        /// A callback for when User is unmuted from channel.
        /// </summary>
        /// @since 4.0.0
        public UserUnmutedDelegate OnUserUnmuted { get; set; }

        /// <summary>
        /// A callback for when user is banned from channel.
        /// </summary>
        /// @since 4.0.0
        public UserBannedDelegate OnUserBanned { get; set; }

        /// <summary>
        /// A callback for when user is unbanned from channel.
        /// </summary>
        /// @since 4.0.0
        public UserUnbannedDelegate OnUserUnbanned { get; set; }

        /// <summary>
        /// A callback for when channel is frozen (Users can't send messages).
        /// </summary>
        /// @since 4.0.0
        public ChannelFrozenDelegate OnChannelFrozen { get; set; }

        /// <summary>
        /// A callback for when channel is unfrozen (Users can send messages).
        /// </summary>
        /// @since 4.0.0
        public ChannelUnfrozenDelegate OnChannelUnfrozen { get; set; }

        /// <summary>
        /// A callback for when channel meta data is created.
        /// </summary>
        /// @since 4.0.0
        public MetaDataCreatedDelegate OnMetaDataCreated { get; set; }

        /// <summary>
        /// A callback for when channel meta data is updated.
        /// </summary>
        /// @since 4.0.0
        public MetaDataUpdatedDelegate OnMetaDataUpdated { get; set; }

        /// <summary>
        /// A callback for when channel meta data is deleted.
        /// </summary>
        /// @since 4.0.0
        public MetaDataDeletedDelegate OnMetaDataDeleted { get; set; }

        /// <summary>
        /// A callback for when channel meta counters is created.
        /// </summary>
        /// @since 4.0.0
        public MetaCountersCreatedDelegate OnMetaCountersCreated { get; set; }

        /// <summary>
        /// A callback for when channel meta counters is updated.
        /// </summary>
        /// @since 4.0.0
        public MetaCountersUpdatedDelegate OnMetaCountersUpdated { get; set; }

        /// <summary>
        /// A callback for when channel meta counters are deleted.
        /// </summary>
        /// @since 4.0.0
        public MetaCountersDeletedDelegate OnMetaCountersDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public OperatorUpdatedDelegate OnOperatorUpdated { get; set; }

        /// <summary>
        /// A callback for when the thread information is updated.
        /// </summary>
        /// @since 4.0.0
        public ThreadInfoUpdatedDelegate OnThreadInfoUpdated { get; set; }
    }
}