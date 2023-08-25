// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class GroupChannelSnapshot : IGroupChannelComparable
    {
        private string _channelUrl;
        private string _name;
        private long _createdAt;
        private SbBaseMessage _lastMessage;

        internal void ResetFromGroupChannel(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel != null)
            {
                _channelUrl = inGroupChannel.Url;
                _name = inGroupChannel.Name;
                _createdAt = inGroupChannel.CreatedAt;
                _lastMessage = inGroupChannel.LastMessage;
            }
        }

        string IGroupChannelComparable.GetChannelUrl()
        {
            return _channelUrl;
        }

        string IGroupChannelComparable.GetName()
        {
            return _name;
        }

        long IGroupChannelComparable.GetCreatedAt()
        {
            return _createdAt;
        }

        SbBaseMessage IGroupChannelComparable.GetLastMessage()
        {
            return _lastMessage;
        }
    }
}