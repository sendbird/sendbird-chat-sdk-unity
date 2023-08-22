// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public abstract partial class SbBaseMessageCreateParams
    {
        private SbAppleCriticalAlertOptions _appleCriticalAlertOptions;
        private string _customType;
        private string _data;
        private bool _isPinnedMessage = false;
        private List<SbMessageMetaArray> _metaArrays;
        private long _parentMessageId;
        private SbPushNotificationDeliveryOption _pushNotificationDeliveryOption;
        private bool _replyToChannel = false;
        private SbMentionType _mentionType = SbMentionType.Users;
        private List<string> _mentionedUserIds;

        internal SbBaseMessageCreateParams() { }

        internal SbBaseMessageCreateParams(SbBaseMessage inBaseMessage)
        {
            if (inBaseMessage == null)
                return;

            _appleCriticalAlertOptions = inBaseMessage.AppleCriticalAlertOptions?.Clone();
            _customType = inBaseMessage.CustomType;
            _data = inBaseMessage.Data;

            if (inBaseMessage.MetaArrays != null && 0 < inBaseMessage.MetaArrays.Count)
                _metaArrays = new List<SbMessageMetaArray>(inBaseMessage.MetaArrays);

            _parentMessageId = inBaseMessage.ParentMessageId;
            _replyToChannel = inBaseMessage.IsReplyToChannel;
            _mentionType = inBaseMessage.MentionType;

            if (inBaseMessage.MentionedUsers != null && 0 < inBaseMessage.MentionedUsers.Count)
            {
                _mentionedUserIds = inBaseMessage.MentionedUsers.Select(inUser => inUser.UserId).ToList();
            }
        }

        internal virtual SbBaseMessageCreateParams Clone()
        {
            return null;
        }

        private protected SbBaseMessageCreateParams(SbBaseMessageCreateParams inOtherParams)
        {
            if (inOtherParams != null)
            {
                _customType = inOtherParams._customType;
                _data = inOtherParams._data;
                _isPinnedMessage = inOtherParams._isPinnedMessage;
                _parentMessageId = inOtherParams._parentMessageId;
                _pushNotificationDeliveryOption = inOtherParams._pushNotificationDeliveryOption;
                _replyToChannel = inOtherParams._replyToChannel;
                _mentionType = inOtherParams._mentionType;
                _parentMessageId = inOtherParams._parentMessageId;
                _appleCriticalAlertOptions = inOtherParams._appleCriticalAlertOptions?.Clone();

                if (inOtherParams._metaArrays != null && 0 < inOtherParams._metaArrays.Count)
                {
                    _metaArrays = new List<SbMessageMetaArray>(inOtherParams._metaArrays);
                }

                if (inOtherParams._mentionedUserIds != null && 0 < inOtherParams._mentionedUserIds.Count)
                {
                    _mentionedUserIds = new List<string>(inOtherParams._mentionedUserIds);
                }
            }
        }
    }
}