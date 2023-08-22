// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseCollection
    {
        internal virtual void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType) { }
        internal virtual void OnChannelUpdated(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel) { }
        internal virtual void OnChannelMemberCountChanged(List<SbGroupChannel> inGroupChannels) { }
        internal virtual void OnLeaveChannel(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel, SbUser inUser) { }
        internal virtual void OnChannelDeleted(string inChannelUrl, SbChannelType inChannelType) { }
        internal virtual void OnUserMuted(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser) { }
        internal virtual void OnUserUnmuted(SbBaseChannel inBaseChannel, SbUser inUser) { }
        internal virtual void OnMessageReceived(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage) { }
        internal virtual void OnMessageDeleted(SbBaseChannel inBaseChannel, long inMessageId) { }
        internal virtual void OnMessageSent(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage) { }
        internal virtual void OnMessagePending(SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage) { }
        internal virtual void OnMessageFailed(SbCollectionEventSource inEventSource, SbBaseChannel inBaseChannel, SbBaseMessage inBaseMessage) { }
        internal virtual void OnMessageUpdated(SbBaseChannel inBaseChannel, List<SbBaseMessage> inMessages) { }
        internal virtual void OnReactionUpdated(SbBaseChannel inBaseChannel, SbReactionEvent inReactionEvent) { }
        internal virtual void OnThreadInfoUpdated(SbBaseChannel inBaseChannel, SbThreadInfoUpdateEvent inThreadInfoUpdateEvent) { }
    }
}