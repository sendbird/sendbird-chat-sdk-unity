// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelModule
    {
        private readonly SendbirdChatMain _chatMainRef;

        internal SbGroupChannelModule(SendbirdChatMain inChatMain)
        {
            _chatMainRef = inChatMain;
        }

        private void CreateChannelInternal(SbGroupChannelCreateParams inChannelCreateParams, SbGroupChannelCallbackHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.CreateChannel(inChannelCreateParams, inCompletionHandler);
        }

        private void GetChannelInternal(string inChannelUrl, SbGetGroupChannelHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.GetChannel(inChannelUrl, false, false, inCompletionHandler);
        }

        private SbGroupChannelCollection CreateGroupChannelCollectionInternal(SbGroupChannelCollectionCreateParams inGroupChannelCollectionCreateParams)
        {
            return _chatMainRef.ChatMainContext.CollectionManager.CreateGroupChannelCollection(inGroupChannelCollectionCreateParams);
        }

        private SbGroupChannelListQuery CreateMyGroupChannelListQueryInternal(SbGroupChannelListQueryParams inGroupChannelListQueryParams)
        {
            return new SbGroupChannelListQuery(inGroupChannelListQueryParams, _chatMainRef.ChatMainContext);
        }

        private SbPublicGroupChannelListQuery CreatePublicGroupChannelListQueryInternal(SbPublicGroupChannelListQueryParams inPublicGroupChannelListQueryParams)
        {
            return new SbPublicGroupChannelListQuery(inPublicGroupChannelListQueryParams, _chatMainRef.ChatMainContext);
        }

        private void AddGroupChannelHandlerInternal(string inIdentifier, SbGroupChannelHandler inChannelHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.AddChannelHandler(inIdentifier, inChannelHandler);
        }

        private void RemoveGroupChannelHandlerInternal(string inIdentifier)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.RemoveChannelHandlerIfContains(inIdentifier);
        }

        private void RemoveAllGroupChannelHandlersInternal()
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.RemoveAllChannelHandlers();
        }

        private SbUnreadMessageCount GetUnreadMessageCountInternal()
        {
            return _chatMainRef.GetUnreadMessageCount();
        }

        private int GetSubscribedTotalUnreadMessageCountInternal()
        {
            return _chatMainRef.GetSubscribedTotalUnreadMessageCount();
        }

        private int GetSubscribedCustomTypeTotalUnreadMessageCountInternal()
        {
            return _chatMainRef.GetSubscribedCustomTypeTotalUnreadMessageCount();
        }

        private int GetSubscribedCustomTypeUnreadMessageCountInternal(string inCustomType)
        {
            return _chatMainRef.GetSubscribedCustomTypeUnreadMessageCount(inCustomType);
        }

        private void GetGroupChannelCountInternal(SbMyMemberStateFilter inMyMemberStateFilter, SbCountHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.GetGroupChannelCount(inMyMemberStateFilter, inCompletionHandler);
        }

        private void GetTotalUnreadMessageCountInternal(SbGroupChannelTotalUnreadMessageCountParams inParams, SbUnreadMessageCountHandler inCompletionHandler)
        {
            _chatMainRef.GetTotalUnreadMessageCount(inParams, inCompletionHandler);
        }

        private void GetTotalUnreadChannelCountInternal(SbGroupChannelTotalUnreadMessageCountParams inParams, SbCountHandler inCompletionHandler)
        {
            _chatMainRef.GetTotalUnreadChannelCount(inParams, inCompletionHandler);
        }

        private void GetUnreadItemCountInternal(List<SbUnreadItemKey> inKeys, SbUnreadItemCountHandler inCompletionHandler)
        {
            _chatMainRef.GetUnreadItemCount(inKeys, inCompletionHandler);
        }

        private void MarkAsReadAllInternal(SbErrorHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.MarkAsReadWithChannelUrls(inChannelUrls: null, inCompletionHandler);
        }

        private void MarkAsReadWithChannelUrlsInternal(List<string> inChannelUrls, SbErrorHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.MarkAsReadWithChannelUrls(inChannelUrls, inCompletionHandler);
        }

        private void MarkAsDeliveredInternal(Dictionary<string, string> inData, SbErrorHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.MarkAsDelivered(inData, inCompletionHandler);
        }

        private void GetMyGroupChannelChangeLogsByTimestampInternal(long inTimestamp, SbGroupChannelChangeLogsParams inParams, SbGroupChannelChangeLogsHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.GetMyGroupChannelChangeLogs(inTimestamp, inToken: null, inParams, inCompletionHandler);
        }

        private void GetMyGroupChannelChangeLogsByTokenInternal(string inToken, SbGroupChannelChangeLogsParams inParams, SbGroupChannelChangeLogsHandler inCompletionHandler)
        {
            _chatMainRef.ChatMainContext.GroupChannelManager.GetMyGroupChannelChangeLogs(inTimestamp: null, inToken, inParams, inCompletionHandler);
        }
    }
}