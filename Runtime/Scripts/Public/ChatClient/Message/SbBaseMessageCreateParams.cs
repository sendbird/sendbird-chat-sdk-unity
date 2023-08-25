// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a base message params.
    /// </summary>
    /// @since 4.0.0
    public abstract partial class SbBaseMessageCreateParams
    {
        /// <summary>
        /// The apple critical alert options of the message.
        /// </summary>
        /// @since 4.0.0
        public SbAppleCriticalAlertOptions AppleCriticalAlertOptions { get => _appleCriticalAlertOptions; set => _appleCriticalAlertOptions = value; }

        /// <summary>
        /// The custom type of the message.
        /// </summary>
        /// @since 4.0.0
        public string CustomType { get => _customType; set => _customType = value; }

        /// <summary>
        /// The data of the message.
        /// </summary>
        /// @since 4.0.0
        public string Data { get => _data; set => _data = value; }

        /// <summary>
        /// Whether the message should be pinned to the channel. Defaults to false.
        /// </summary>
        /// @since 4.0.0
        public bool IsPinnedMessage { get => _isPinnedMessage; set => _isPinnedMessage = value; }

        /// <summary>
        /// The meta arrays of the message.
        /// </summary>
        /// @since 4.0.0
        public List<SbMessageMetaArray> MetaArrays { get => _metaArrays; set => _metaArrays = value; }

        /// <summary>
        /// The parent message id of the message.
        /// </summary>
        /// @since 4.0.0
        public long ParentMessageId { get => _parentMessageId; set => _parentMessageId = value; }

        /// <summary>
        /// The push notification delivery option user of the message.
        /// </summary>
        /// @since 4.0.0
        public SbPushNotificationDeliveryOption PushNotificationDeliveryOption { get => _pushNotificationDeliveryOption; set => _pushNotificationDeliveryOption = value; }

        /// <summary>
        /// Whether the message should also be sent to the channel. Defaults to false. Only works when the ParentMessageId is set.
        /// </summary>
        /// @since 4.0.0
        public bool ReplyToChannel { get => _replyToChannel; set => _replyToChannel = value; }

        /// <summary>
        /// The mention type of the message. Defaults to SbMentionType.Users.
        /// </summary>
        /// @since 4.0.0
        public SbMentionType MentionType { get => _mentionType; set => _mentionType = value; }

        /// <summary>
        /// The mentioned user ids of the message. If it hasn't set before, it returns null.
        /// </summary>
        /// @since 4.0.0
        public List<string> MentionedUserIds { get => _mentionedUserIds; set => _mentionedUserIds = value; }
    }
}