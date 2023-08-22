// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelManager
    {
        private SbGroupChannelCollection _groupChannelCollection = null;
        private bool _isLoadingChannels = false;
        private bool _isOpened = false;
        public Action<IReadOnlyList<SbGroupChannel>> OnChannelListChanged { get; set; }

        public void OnOpenState()
        {
            _isOpened = true;
            SbGroupChannelListQueryParams groupChannelListQueryParams = new SbGroupChannelListQueryParams
            {
                Order = SbGroupChannelListOrder.LatestLastMessage
            };
            SbGroupChannelListQuery groupChannelListQuery = SendbirdChat.GroupChannel.CreateMyGroupChannelListQuery(groupChannelListQueryParams);
            SbGroupChannelCollectionHandler groupChannelCollectionHandler = new SbGroupChannelCollectionHandler
            {
                OnChannelsAdded = OnChannelsAddedHandler,
                OnChannelsUpdated = OnChannelsUpdatedHandler,
                OnChannelsDeleted = OnChannelsDeletedHandler
            };
            SbGroupChannelCollectionCreateParams createParams = new SbGroupChannelCollectionCreateParams(groupChannelListQuery, groupChannelCollectionHandler);

            _groupChannelCollection = SendbirdChat.GroupChannel.CreateGroupChannelCollection(createParams);
            LoadNextChannelListIfHasNext();
        }

        public void OnCloseState()
        {
            _isOpened = false;
            if (_groupChannelCollection != null)
            {
                _groupChannelCollection.Dispose();
                _groupChannelCollection = null;
            }

            OnChannelListChanged = null;
        }

        public bool LoadNextChannelListIfHasNext()
        {
            if (_groupChannelCollection == null || _isLoadingChannels)
                return false;

            void OnLoadMoreCompletionHandler(IReadOnlyList<SbGroupChannel> inChannels, SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                _isLoadingChannels = false;
                if( _isOpened == false)
                    return;
                
                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                }
                else if (inChannels != null && 0 < inChannels.Count)
                {
                    OnChannelListChanged?.Invoke(_groupChannelCollection.ChannelList);
                }
            }

            if (_groupChannelCollection.HasNext)
            {
                SampleChatMain.Instance.BlockUI();
                _isLoadingChannels = true;
                _groupChannelCollection.LoadMore(OnLoadMoreCompletionHandler);
            }

            return true;
        }

        private void OnChannelsAddedHandler(SbGroupChannelContext inContext, IReadOnlyList<SbGroupChannel> inAddedChannels)
        {
            OnChannelListChanged?.Invoke(_groupChannelCollection.ChannelList);
        }

        private void OnChannelsUpdatedHandler(SbGroupChannelContext inContext, IReadOnlyList<SbGroupChannel> inUpdatedChannels)
        {
            OnChannelListChanged?.Invoke(_groupChannelCollection.ChannelList);
        }

        private void OnChannelsDeletedHandler(SbGroupChannelContext inContext, IReadOnlyList<string> inDeletedChannelUrls)
        {
            OnChannelListChanged?.Invoke(_groupChannelCollection.ChannelList);
        }
    }
}