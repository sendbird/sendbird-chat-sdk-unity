// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbGroupChannelModule
    {
        /// <summary>
        /// Creates a group channel with SbGroupChannelCreateParams class.
        /// </summary>
        /// <param name="inChannelCreateParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void CreateChannel(SbGroupChannelCreateParams inChannelCreateParams, SbGroupChannelCallbackHandler inCompletionHandler)
        {
            CreateChannelInternal(inChannelCreateParams, inCompletionHandler);
        }

        /// <summary>
        /// Gets a group channel instance from channel URL asynchronously.
        /// </summary>
        /// <param name="inChannelUrl"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetChannel(string inChannelUrl, SbGetGroupChannelHandler inCompletionHandler)
        {
            GetChannelInternal(inChannelUrl, inCompletionHandler);
        }

        /// <summary>
        /// Creates a query for my group channel list.
        /// </summary>
        /// <param name="inGroupChannelListQueryParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbGroupChannelListQuery CreateMyGroupChannelListQuery(SbGroupChannelListQueryParams inGroupChannelListQueryParams)
        {
            return CreateMyGroupChannelListQueryInternal(inGroupChannelListQueryParams);
        }

        /// <summary>
        /// Creates a query for public group channel list.
        /// </summary>
        /// <param name="inPublicGroupChannelListQueryParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbPublicGroupChannelListQuery CreatePublicGroupChannelListQuery(SbPublicGroupChannelListQueryParams inPublicGroupChannelListQueryParams)
        {
            return CreatePublicGroupChannelListQueryInternal(inPublicGroupChannelListQueryParams);
        }

        /// <summary>
        /// Adds a channel handler. All added handlers will be notified when events occur.
        /// </summary>
        /// <param name="inIdentifier">The identifier for handler.</param>
        /// <param name="inChannelHandler"></param>
        /// @since 4.0.0
        public void AddGroupChannelHandler(string inIdentifier, SbGroupChannelHandler inChannelHandler)
        {
            AddGroupChannelHandlerInternal(inIdentifier, inChannelHandler);
        }

        /// <summary>
        /// Removes a channel handler. The deleted handler no longer be notified.
        /// </summary>
        /// <param name="inIdentifier">The identifier for handler.</param>
        /// @since 4.0.0
        public void RemoveGroupChannelHandler(string inIdentifier)
        {
            RemoveGroupChannelHandlerInternal(inIdentifier);
        }

        /// <summary>
        /// Removes all channel handlers added by AddGroupChannelHandler.
        /// </summary>
        /// @since 4.0.0
        public void RemoveAllGroupChannelHandlers()
        {
            RemoveAllGroupChannelHandlersInternal();
        }

        /// <summary>
        /// Gets the subscribed total unread message of current user.
        /// </summary>
        /// <returns></returns>
        /// @since 4.0.0
        public SbUnreadMessageCount GetUnreadMessageCount()
        {
            return GetUnreadMessageCountInternal();
        }

        /// <summary>
        /// Gets the subscribed total number of unread message of all GroupChannels the current user has joined.
        /// </summary>
        /// <returns></returns>
        /// @since 4.0.0
        public int GetSubscribedTotalUnreadMessageCount()
        {
            return GetSubscribedTotalUnreadMessageCountInternal();
        }

        /// <summary>
        /// Gets the total number of unread message of GroupChannels with subscribed custom types.
        /// </summary>
        /// <returns></returns>
        /// @since 4.0.0
        public int GetSubscribedCustomTypeTotalUnreadMessageCount()
        {
            return GetSubscribedCustomTypeTotalUnreadMessageCountInternal();
        }

        /// <summary>
        /// Gets the number of unread message of GroupChannel with subscribed custom type.
        /// </summary>
        /// <param name="inCustomType"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public int GetSubscribedCustomTypeUnreadMessageCount(string inCustomType)
        {
            return GetSubscribedCustomTypeUnreadMessageCountInternal(inCustomType);
        }

        /// <summary>
        /// Gets the number of my GroupChannels.
        /// </summary>
        /// <param name="inMyMemberStateFilter"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetGroupChannelCount(SbMyMemberStateFilter inMyMemberStateFilter, SbCountHandler inCompletionHandler)
        {
            GetGroupChannelCountInternal(inMyMemberStateFilter, inCompletionHandler);
        }

        /// <summary>
        /// Gets the total number of unread message of GroupChannels with SbGroupChannelTotalUnreadMessageCountParams filter.
        /// </summary>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetTotalUnreadMessageCount(SbGroupChannelTotalUnreadMessageCountParams inParams, SbUnreadMessageCountHandler inCompletionHandler)
        {
            GetTotalUnreadMessageCountInternal(inParams, inCompletionHandler);
        }

        /// <summary>
        /// Gets the total number of unread GroupChannels the current user has joined.
        /// </summary>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetTotalUnreadChannelCount(SbGroupChannelTotalUnreadMessageCountParams inParams, SbCountHandler inCompletionHandler)
        {
            GetTotalUnreadChannelCountInternal(inParams, inCompletionHandler);
        }

        /// <summary>
        /// Gets the unread item count of GroupChannels from keys.
        /// </summary>
        /// <param name="inKeys"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetUnreadItemCount(List<SbUnreadItemKey> inKeys, SbUnreadItemCountHandler inCompletionHandler)
        {
            GetUnreadItemCountInternal(inKeys, inCompletionHandler);
        }

        /// <summary>
        /// Sends mark as read to all joined GroupChannels. This method has rate limit. You can send one request per second. It returns SendbirdException if you exceed the rate limit.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void MarkAsReadAll(SbErrorHandler inCompletionHandler)
        {
            MarkAsReadAllInternal(inCompletionHandler);
        }

        /// <summary>
        /// Sends mark as read to joined GroupChannels. This method has rate limit. You can send one request per second.
        /// </summary>
        /// <param name="inChannelUrls"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void MarkAsReadWithChannelUrls(List<string> inChannelUrls, SbErrorHandler inCompletionHandler)
        {
            MarkAsReadWithChannelUrlsInternal(inChannelUrls, inCompletionHandler);
        }

        /// <summary>
        /// Sends mark as delivered to this channel when you received push message from us.
        /// </summary>
        /// <param name="inData"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void MarkAsDelivered(Dictionary<string, string> inData, SbErrorHandler inCompletionHandler)
        {
            MarkAsDeliveredInternal(inData, inCompletionHandler);
        }

        /// <summary>
        /// Requests the channel changelogs after given timestamp. The result is passed to handler.
        /// </summary>
        /// <param name="inTimestamp"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMyGroupChannelChangeLogsByTimestamp(long inTimestamp, SbGroupChannelChangeLogsParams inParams, SbGroupChannelChangeLogsHandler inCompletionHandler)
        {
            GetMyGroupChannelChangeLogsByTimestampInternal(inTimestamp, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Requests the channel changelogs from given token. The result is passed to handler.
        /// </summary>
        /// <param name="inToken"></param>
        /// <param name="inParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void GetMyGroupChannelChangeLogsByToken(string inToken, SbGroupChannelChangeLogsParams inParams, SbGroupChannelChangeLogsHandler inCompletionHandler)
        {
            GetMyGroupChannelChangeLogsByTokenInternal(inToken, inParams, inCompletionHandler);
        }

        /// <summary>
        /// Creates GroupChannelCollection instance with the params.
        /// </summary>
        /// <param name="inGroupChannelCollectionCreateParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbGroupChannelCollection CreateGroupChannelCollection(SbGroupChannelCollectionCreateParams inGroupChannelCollectionCreateParams)
        {
            return CreateGroupChannelCollectionInternal(inGroupChannelCollectionCreateParams);
        }
    }
}