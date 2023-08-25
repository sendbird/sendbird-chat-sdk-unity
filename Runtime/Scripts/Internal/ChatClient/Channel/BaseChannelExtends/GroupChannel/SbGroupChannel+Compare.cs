// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    public partial class SbGroupChannel
    {
        internal int CompareTo(IGroupChannelComparable inOtherComparable, SbGroupChannelListOrder inOrder)
        {
            if (inOtherComparable == null)
                return -1;

            int channelUrlCompare = string.Compare(Url, inOtherComparable.GetChannelUrl(), StringComparison.CurrentCulture);
            switch (inOrder)
            {
                case SbGroupChannelListOrder.Chronological:
                {
                    //Descending order
                    int compare = CreatedAt.CompareTo(inOtherComparable.GetCreatedAt());
                    return compare * -1;
                }
                case SbGroupChannelListOrder.LatestLastMessage:
                {
                    if (_lastMessage == null && inOtherComparable.GetLastMessage() == null) return CreatedAt.CompareTo(inOtherComparable.GetCreatedAt()) * -1;
                    else if (_lastMessage != null && inOtherComparable.GetLastMessage() == null) return -1;
                    else if (_lastMessage == null && inOtherComparable.GetLastMessage() != null) return 1;

                    //Descending order
                    return LastMessage.CreatedAt.CompareTo(inOtherComparable.GetLastMessage().CreatedAt) * -1;
                }
                case SbGroupChannelListOrder.ChannelNameAlphabetical:
                {
                    if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(inOtherComparable.GetName())) return channelUrlCompare;
                    else if (string.IsNullOrEmpty(Name) == false && string.IsNullOrEmpty(inOtherComparable.GetName())) return -1;
                    else if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(inOtherComparable.GetName()) == false) return 1;

                    return string.Compare(Name, inOtherComparable.GetName(), StringComparison.CurrentCulture);
                }
                case SbGroupChannelListOrder.ChannelMetaDataValueAlphabetical: return 0;
            }

            return 0;
        }

        string IGroupChannelComparable.GetChannelUrl()
        {
            return Url;
        }

        string IGroupChannelComparable.GetName()
        {
            return Name;
        }

        long IGroupChannelComparable.GetCreatedAt()
        {
            return CreatedAt;
        }

        SbBaseMessage IGroupChannelComparable.GetLastMessage()
        {
            return _lastMessage;
        }
    }
}