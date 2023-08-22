// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelManager
    {
        private readonly List<SbOpenChannel> _channelList = new List<SbOpenChannel>();
        private SbOpenChannelListQuery _openChannelListQuery = null;
        private SbOpenChannelHandler _openChannelHandler = null;
        private readonly string _channelHandlerId = Guid.NewGuid().ToString();
        private bool _isOpened = false;
        private bool _isLoadingLatestChannels = false;
        public Action<IReadOnlyList<SbOpenChannel>> OnChannelListChanged { get; set; }

        public void OnOpenState()
        {
            _isOpened = true;
            _openChannelListQuery = SendbirdChat.OpenChannel.CreateOpenChannelListQuery(new SbOpenChannelListQueryParams());
            _openChannelHandler = new SbOpenChannelHandler
            {
                OnChannelChanged = OnChannelChangedHandler,
                OnChannelDeleted = OnChannelDeletedHandler
            };
            SendbirdChat.OpenChannel.AddOpenChannelHandler(_channelHandlerId, _openChannelHandler);
            _channelList.Clear();

            LoadPreviousChannelListIfHasPrevious();
        }

        public void OnCloseState()
        {
            _isOpened = false;
            _channelList.Clear();
            if (_openChannelHandler != null)
            {
                SendbirdChat.OpenChannel.RemoveOpenChannelHandler(_channelHandlerId);
                _openChannelHandler = null;
            }

            _openChannelListQuery = null;
            OnChannelListChanged = null;
        }

        public bool LoadPreviousChannelListIfHasPrevious()
        {
            if (_openChannelListQuery == null || _openChannelListQuery.IsLoading || _openChannelListQuery.HasNext == false)
                return false;

            SampleChatMain.Instance.BlockUI();
            _openChannelListQuery.LoadNextPage(OnLoadNextPageCompleteHandler);
            return true;
        }
        
        public bool LoadLatestChannelList()
        {
            if (_isLoadingLatestChannels)
                return false;

            SampleChatMain.Instance.BlockUI();
            SbOpenChannelListQuery query = SendbirdChat.OpenChannel.CreateOpenChannelListQuery(new SbOpenChannelListQueryParams());
            query.LoadNextPage(OnLoadNextPageCompleteHandler);
            return true;
        }
        
        private void OnLoadNextPageCompleteHandler(IReadOnlyList<SbOpenChannel> inChannels, SbError inError)
        {
            SampleChatMain.Instance.UnblockUI();
            if (_isOpened == false)
                return;

            if (inError != null)
            {
                SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
            }
            else if (inChannels != null && 0 < inChannels.Count)
            {
                foreach (SbOpenChannel openChannel in inChannels)
                {
                    InsertChannel(openChannel);
                }

                _channelList.Sort(ChannelDescendingComparison);
                OnChannelListChanged?.Invoke(_channelList);
            }
        }

        private void InsertChannel(SbOpenChannel inNewOrUpdateChannel)
        {
            if (inNewOrUpdateChannel == null)
                return;

            int foundIndex = _channelList.FindIndex(inChannel => inChannel.Url == inNewOrUpdateChannel.Url);
            if (0 <= foundIndex)
            {
                _channelList[foundIndex] = inNewOrUpdateChannel;
            }
            else
            {
                _channelList.Add(inNewOrUpdateChannel);
            }
        }

        private void OnChannelChangedHandler(SbBaseChannel inBaseChannel)
        {
            InsertChannel(inBaseChannel as SbOpenChannel);
            OnChannelListChanged?.Invoke(_channelList);
        }

        private void OnChannelDeletedHandler(string inChannelUrl, SbChannelType inChannelType)
        {
            int foundIndex = _channelList.FindIndex(inChannel => inChannel.Url == inChannelUrl);
            if (0 <= foundIndex)
            {
                _channelList.RemoveAt(foundIndex);
                OnChannelListChanged?.Invoke(_channelList);
            }
        }
        
        private int ChannelDescendingComparison(SbBaseChannel inChannelA, SbBaseChannel inChannelB)
        {
            if (inChannelA == null && inChannelB == null)
                return 0;

            int compare = 0;
            if (inChannelA == null)
            {
                compare = 1;
            }
            else if (inChannelB == null)
            {
                compare = -1;
            }
            else
            {
                compare = inChannelA.CreatedAt.CompareTo(inChannelB.CreatedAt);
            }

            return -compare;
        }
    }
}