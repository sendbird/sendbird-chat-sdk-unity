// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMessageListParams
    {
        internal SbMessageListParams Clone()
        {
            SbMessageListParams clonedListParams = new SbMessageListParams();
            clonedListParams.Reverse = Reverse;
            clonedListParams.IsInclusive = IsInclusive;
            clonedListParams.MessageTypeFilter = MessageTypeFilter;
            clonedListParams.IncludeMetaArray = IncludeMetaArray;
            clonedListParams.IncludeReactions = IncludeReactions;
            clonedListParams.IncludeThreadInfo = IncludeThreadInfo;
            clonedListParams.IncludeParentMessageInfo = IncludeParentMessageInfo;
            clonedListParams.ReplyType = ReplyType;
            clonedListParams.ShowSubChannelMessagesOnly = ShowSubChannelMessagesOnly;
            clonedListParams.PreviousResultSize = PreviousResultSize;
            clonedListParams.NextResultSize = NextResultSize;

            if (CustomTypes != null && 0 < CustomTypes.Count)
                clonedListParams.CustomTypes = new List<string>(CustomTypes);

            if (SenderUserIds != null && 0 < SenderUserIds.Count)
                clonedListParams.SenderUserIds = new List<string>(SenderUserIds);

            return clonedListParams;
        }

        private bool BelongsToInternal(SbBaseMessage inMessage)
        {
            if (inMessage == null)
                return false;

            if ((MessageTypeFilter == SbMessageTypeFilter.User && inMessage is SbUserMessage == false) ||
                (MessageTypeFilter == SbMessageTypeFilter.File && inMessage is SbFileMessage == false) ||
                (MessageTypeFilter == SbMessageTypeFilter.Admin && inMessage is SbAdminMessage == false))
                return false;

            if (SenderUserIds != null && 0 < SenderUserIds.Count)
            {
                string messageSenderId = inMessage.Sender?.UserId;
                if (string.IsNullOrEmpty(messageSenderId) || SenderUserIds.Contains(messageSenderId) == false)
                    return false;
            }

            if (CustomTypes != null && 0 < CustomTypes.Count)
            {
                if (CustomTypes.Contains(inMessage.CustomType) == false)
                    return false;
            }

            if (ReplyType == SbReplyType.None && SbBaseMessage.INVALID_MESSAGE_ID_MIN < inMessage.ParentMessageId)
            {
                return false;
            }
            else if (ReplyType == SbReplyType.OnlyReplyToChannel && SbBaseMessage.INVALID_MESSAGE_ID_MIN < inMessage.ParentMessageId && inMessage.IsReplyToChannel == false)
            {
                return false;
            }

            return true;
        }

        private bool BelongsToInternal(SbBaseMessageCreateParams inParams)
        {
            if (inParams == null)
                return false;

            if ((MessageTypeFilter == SbMessageTypeFilter.User && inParams is SbUserMessageCreateParams == false) ||
                (MessageTypeFilter == SbMessageTypeFilter.File && inParams is SbFileMessageCreateParams == false))
                return false;

            if (CustomTypes != null && 0 < CustomTypes.Count)
            {
                if (CustomTypes.Contains(inParams.CustomType) == false)
                    return false;
            }

            return true;
        }
    }
}