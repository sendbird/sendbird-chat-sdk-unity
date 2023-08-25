// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelCollection
    {
        internal override void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType)
        {
            if (inChangedStateType == ConnectionStateInternalType.Connected)
            {
                RequestAllChangeLogs();
            }
        }

        internal override void OnChannelUpdated(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel is SbGroupChannel groupChannel)
            {
                List<SbGroupChannel> addOrUpdateChannels = new List<SbGroupChannel> { groupChannel };
                UpsertChannelsToCache(inCollectionEventSource.ToCachedGroupChannelContext(), addOrUpdateChannels);
            }
        }

        internal override void OnUserMuted(SbBaseChannel inBaseChannel, SbRestrictedUser inRestrictedUser)
        {
            OnChannelUpdated(SbCollectionEventSource.EventUserMuted, inBaseChannel);
        }

        internal override void OnUserUnmuted(SbBaseChannel inBaseChannel, SbUser inUser)
        {
            OnChannelUpdated(SbCollectionEventSource.EventUserUnmuted, inBaseChannel);
        }

        internal override void OnChannelMemberCountChanged(List<SbGroupChannel> inGroupChannels)
        {
            if (inGroupChannels != null && inGroupChannels.Count > 0)
            {
                UpsertChannelsToCache(SbCollectionEventSource.EventChannelMemberCountChanged.ToCachedGroupChannelContext(), inGroupChannels);
            }
        }

        internal override void OnLeaveChannel(SbCollectionEventSource inCollectionEventSource, SbBaseChannel inBaseChannel, SbUser inUser)
        {
            if (inBaseChannel is SbGroupChannel groupChannel && inUser != null)
            {
                if (inUser.UserId == chatMainContextRef.CurrentUserId)
                {
                    List<string> deletedChannelUrls = new List<string> { groupChannel.Url };
                    UpsertChannelsToCache(inCollectionEventSource.ToCachedGroupChannelContext(), inAddOrUpdateGroupChannels: null, deletedChannelUrls);
                }
                else
                {
                    List<SbGroupChannel> addOrUpdateChannels = new List<SbGroupChannel> { groupChannel };
                    UpsertChannelsToCache(inCollectionEventSource.ToCachedGroupChannelContext(), addOrUpdateChannels);
                }
            }
        }

        internal override void OnChannelDeleted(string inChannelUrl, SbChannelType inChannelType)
        {
            if (inChannelType == SbChannelType.Group)
            {
                List<string> deletedChannelUrls = new List<string> { inChannelUrl };
                UpsertChannelsToCache(SbCollectionEventSource.EventChannelDeleted.ToCachedGroupChannelContext(), inAddOrUpdateGroupChannels: null, deletedChannelUrls);
            }
        }
    }
}