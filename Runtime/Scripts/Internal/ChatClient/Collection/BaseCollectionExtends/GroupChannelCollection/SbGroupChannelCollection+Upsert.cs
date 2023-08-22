// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelCollection
    {
        private void UpsertChannelsToCache(SbGroupChannelContext inGroupChannelContext, IReadOnlyCollection<SbGroupChannel> inAddOrUpdateGroupChannels,
                                           IReadOnlyCollection<string> inDeletedChannelUrls = null)
        {
            List<SbGroupChannel> addedResultChannels = null;
            List<SbGroupChannel> updatedResultChannels = null;
            List<string> deletedResultChannelUrls = null;
            bool isChangedCachedChannelList = false;

            if (inAddOrUpdateGroupChannels != null && 0 < inAddOrUpdateGroupChannels.Count)
            {
                foreach (SbGroupChannel channel in inAddOrUpdateGroupChannels)
                {
                    if (channel == null)
                        continue;

                    UpdateActionType updateAction = CalculateUpdateAction(channel);
                    switch (updateAction)
                    {
                        case UpdateActionType.Add:
                        {
                            if (addedResultChannels == null) { addedResultChannels = new List<SbGroupChannel>(); }

                            addedResultChannels.Add(channel);
                            InsertChannelToCachedChannelList(channel);
                            isChangedCachedChannelList = true;
                            break;
                        }
                        case UpdateActionType.Update:
                        {
                            if (updatedResultChannels == null) { updatedResultChannels = new List<SbGroupChannel>(); }

                            updatedResultChannels.Add(channel);
                            InsertChannelToCachedChannelList(channel);
                            isChangedCachedChannelList = true;
                            break;
                        }
                        case UpdateActionType.Delete:
                        {
                            if (deletedResultChannelUrls == null) { deletedResultChannelUrls = new List<string>(); }

                            deletedResultChannelUrls.Add(channel.Url);
                            isChangedCachedChannelList = true;
                            break;
                        }
                        case UpdateActionType.None: break;
                    }
                }
            }

            if (inDeletedChannelUrls != null && 0 < inDeletedChannelUrls.Count)
            {
                if (deletedResultChannelUrls == null) { deletedResultChannelUrls = new List<string>(); }

                foreach (string channelUrl in inDeletedChannelUrls)
                {
                    int foundIndex = _cachedChannelList.FindIndex(inCachedChannel => inCachedChannel.Url == channelUrl);
                    if (0 <= foundIndex)
                    {
                        deletedResultChannelUrls.Add(channelUrl);
                        isChangedCachedChannelList = true;
                    }
                }
            }

            if (isChangedCachedChannelList == false)
                return;

            if (deletedResultChannelUrls != null)
            {
                foreach (string channelUrl in deletedResultChannelUrls)
                {
                    int foundIndex = _cachedChannelList.FindIndex(inCachedChannel => inCachedChannel.Url == channelUrl);
                    if (0 <= foundIndex)
                    {
                        _cachedChannelList.RemoveAt(foundIndex);
                    }
                }
            }

            _cachedChannelList.Sort(CachedGroupChannelComparison);

            if (0 < _cachedChannelList.Count)
                SetLastChannelSnapshot(_cachedChannelList[_cachedChannelList.Count - 1]);

            if (inGroupChannelContext != null)
            {
                if (addedResultChannels != null && 0 < addedResultChannels.Count)
                {
                    _groupChannelCollectionHandler?.OnChannelsAdded?.Invoke(inGroupChannelContext, addedResultChannels);
                }

                if (updatedResultChannels != null && 0 < updatedResultChannels.Count)
                {
                    _groupChannelCollectionHandler?.OnChannelsUpdated?.Invoke(inGroupChannelContext, updatedResultChannels);
                }

                if (deletedResultChannelUrls != null && 0 < deletedResultChannelUrls.Count)
                {
                    _groupChannelCollectionHandler?.OnChannelsDeleted?.Invoke(inGroupChannelContext, deletedResultChannelUrls);
                }
            }
        }

        private UpdateActionType CalculateUpdateAction(SbGroupChannel inGroupChannel)
        {
            bool shouldAddToView = ShouldAddToView(inGroupChannel);
            bool contains = _cachedChannelList.Any(inChannel => inChannel.Url == inGroupChannel.Url);

            if (shouldAddToView) { return contains ? UpdateActionType.Update : UpdateActionType.Add; }
            else { return contains ? UpdateActionType.Delete : UpdateActionType.None; }
        }

        private bool ShouldAddToView(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel == null || _groupChannelListQuery.BelongsTo(inGroupChannel) == false)
                return false;

            if (_lastChannelSnapshot == null)
                return _hasNext == false;

            if (_hasNext == false)
                return true;

            return inGroupChannel.CompareTo(_lastChannelSnapshot, _groupChannelListQuery.Order) <= 0;
        }

        private int CachedGroupChannelComparison(SbGroupChannel inGroupChannelA, SbGroupChannel inGroupChannelB)
        {
            if (inGroupChannelA != null)
            {
                return inGroupChannelA.CompareTo(inGroupChannelB, _groupChannelListQuery.Order);
            }

            return inGroupChannelB == null ? 0 : 1;
        }

        private void InsertChannelToCachedChannelList(SbGroupChannel inChannel)
        {
            if (inChannel == null)
                return;

            int foundIndex = _cachedChannelList.FindIndex(inCachedChannel => inCachedChannel.Url == inChannel.Url);
            if (0 <= foundIndex)
            {
                _cachedChannelList[foundIndex] = inChannel;
            }
            else
            {
                _cachedChannelList.Add(inChannel);
            }
        }

        private void InsertChannelsToCachedChannelList(IReadOnlyList<SbGroupChannel> inGroupChannels)
        {
            if (inGroupChannels == null || inGroupChannels.Count <= 0)
                return;

            foreach (SbGroupChannel groupChannel in inGroupChannels)
            {
                InsertChannelToCachedChannelList(groupChannel);
            }

            _cachedChannelList.Sort(CachedGroupChannelComparison);

            if (0 < _cachedChannelList.Count)
                SetLastChannelSnapshot(_cachedChannelList[_cachedChannelList.Count - 1]);
        }
    }
}