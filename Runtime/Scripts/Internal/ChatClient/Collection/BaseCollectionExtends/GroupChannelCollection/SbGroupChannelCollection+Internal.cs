// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelCollection
    {
        private enum UpdateActionType
        {
            Add,
            Update,
            Delete,
            None,
        }

        private readonly SbGroupChannelListQuery _groupChannelListQuery = null;
        private SbGroupChannelCollectionHandler _groupChannelCollectionHandler = null;
        private readonly List<SbGroupChannel> _cachedChannelList = new List<SbGroupChannel>();
        private GroupChannelSnapshot _lastChannelSnapshot = null;
        private long _defaultChangeLogsTimestamp = 0;
        private readonly SbGroupChannelChangeLogsParams _changeLogsParams = new SbGroupChannelChangeLogsParams();
        private string _lastSyncChangeLogsToken = null;
        private bool _isDisposed = false;
        private bool _isLoading = false;
        private bool _hasNext;

        internal SbGroupChannelCollection(SbGroupChannelCollectionCreateParams inCreateParams, SendbirdChatMainContext inChatMainContext) : base(inChatMainContext)
        {
            if (inCreateParams == null)
            {
                Logger.Error(Logger.CategoryType.Collection, "SbGroupChannelCollection() createParams is null.");
                return;
            }

            _hasNext = true;
            _isDisposed = false;
            _isLoading = false;

            _groupChannelListQuery = inCreateParams.Query;
            _groupChannelCollectionHandler = inCreateParams.GroupChannelCollectionHandler;

            if (_groupChannelListQuery != null)
            {
                _changeLogsParams.CustomTypes = _groupChannelListQuery.CustomTypesFilter as List<string>;
                _changeLogsParams.IncludeEmpty = _groupChannelListQuery.IncludeEmptyChannel;
                _changeLogsParams.IncludeFrozen = _groupChannelListQuery.IncludeFrozenChannel;
            }
        }

        private void LoadMoreInternal(SbGroupChannelListHandler inCompletionHandler)
        {
            SbError error = null;
            if (_isLoading) { error = SbErrorCodeExtension.QUERY_IN_PROGRESS; }
            else if (_isDisposed) { error = SbErrorCodeExtension.COLLECTION_DISPOSED; }
            else if (_hasNext == false) { error = SbErrorCodeExtension.LOCAL_DATABASE_ERROR; }

            if (error != null)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, error); });
                return;
            }

            void OnCompleteLoadNextPage(IReadOnlyList<SbGroupChannel> inGroupChannels, SbError inError)
            {
                _isLoading = false;
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                _hasNext = _groupChannelListQuery.HasNext;
                if (inGroupChannels != null && 0 < inGroupChannels.Count)
                {
                    SetDefaultChangeLogsTimestamps(inGroupChannels[0]);
                    InsertChannelsToCachedChannelList(inGroupChannels);
                }

                inCompletionHandler?.Invoke(inGroupChannels, null);
            }

            _isLoading = true;
            _groupChannelListQuery.LoadNextPage(OnCompleteLoadNextPage);
        }

        private protected override void DisposeInternal()
        {
            _isDisposed = true;
            _hasNext = false;
            _groupChannelCollectionHandler = null;
            _cachedChannelList.Clear();
            base.DisposeInternal();
        }

        private void SetDefaultChangeLogsTimestamps(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel != null)
            {
                _defaultChangeLogsTimestamp = inGroupChannel.CreatedAt;
                if (_groupChannelListQuery.Order == SbGroupChannelListOrder.LatestLastMessage && inGroupChannel.LastMessage != null)
                    _defaultChangeLogsTimestamp = inGroupChannel.LastMessage.CreatedAt;
            }
            else
            {
                _defaultChangeLogsTimestamp = 0;
            }
        }

        private void SetLastChannelSnapshot(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel == null)
                return;

            if (_lastChannelSnapshot == null)
                _lastChannelSnapshot = new GroupChannelSnapshot();

            _lastChannelSnapshot.ResetFromGroupChannel(inGroupChannel);
        }

        private void RequestAllChangeLogs()
        {
            List<SbGroupChannel> updatedGroupChannels = null;
            List<string> deletedChannelUrls = null;
            string lastSucceededResultToken = _lastSyncChangeLogsToken;
            void OnCompleteGetChangeLogs(IReadOnlyList<SbGroupChannel> inGroupChannels, IReadOnlyList<string> inDeletedChannelUrls, bool inHasMore, string inToken, SbError inError)
            {
                if (inError == null)
                {
                    if (inGroupChannels != null && 0 < inGroupChannels.Count)
                    {
                        if (updatedGroupChannels == null) { updatedGroupChannels = new List<SbGroupChannel>(); }

                        updatedGroupChannels.AddRange(inGroupChannels);
                    }

                    if (inDeletedChannelUrls != null && 0 < inDeletedChannelUrls.Count)
                    {
                        if (deletedChannelUrls == null) { deletedChannelUrls = new List<string>(); }

                        deletedChannelUrls.AddRange(inDeletedChannelUrls);
                    }

                    lastSucceededResultToken = inToken;
                    if (inHasMore)
                    {
                        chatMainContextRef.GroupChannelManager.GetMyGroupChannelChangeLogs(inTimestamp: null, lastSucceededResultToken, _changeLogsParams, OnCompleteGetChangeLogs);
                        return;
                    }
                }

                if (updatedGroupChannels != null || deletedChannelUrls != null)
                {
                    UpsertChannelsToCache(SbCollectionEventSource.ChannelChangelog.ToCachedGroupChannelContext(), updatedGroupChannels, deletedChannelUrls);
                }

                _lastSyncChangeLogsToken = lastSucceededResultToken;
            }

            if (string.IsNullOrEmpty(_lastSyncChangeLogsToken) == false)
            {
                chatMainContextRef.GroupChannelManager.GetMyGroupChannelChangeLogs(inTimestamp: null, _lastSyncChangeLogsToken, _changeLogsParams, OnCompleteGetChangeLogs);
            }
            else
            {
                chatMainContextRef.GroupChannelManager.GetMyGroupChannelChangeLogs(_defaultChangeLogsTimestamp, inToken: null, _changeLogsParams, OnCompleteGetChangeLogs);
            }
        }
    }
}