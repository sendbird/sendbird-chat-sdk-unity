// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelListScrollViewContent : MonoBehaviour
    {
        [SerializeField] private Button _enterChannelButton = null;
        [SerializeField] private Text _channelNameText = null;
        [SerializeField] private Text _lastChangedAtText = null;
        [SerializeField] private Text _lastMessageText = null;
        [SerializeField] private Text _memberCountText = null;
        [SerializeField] private GameObject _unreadMessageCountGameObject = null;
        [SerializeField] private Text _unreadMessageCountText = null;
        private SbGroupChannel _groupChannel = null;

        private void Awake()
        {
            if (_enterChannelButton == null)
            {
                Debug.LogError("You need to set EnterChannelButton in GroupChannelListScrollViewContent");
            }
            else
            {
                _enterChannelButton.onClick.AddListener(OnClickEnterChannelButton);
            }
        }

        public void ResetFromGroupChannel(SbGroupChannel inGroupChannel)
        {
            _groupChannel = inGroupChannel;
            if (_groupChannel == null)
                return;

            const int CHANNEL_NAME_MAX_LENGTH = 20;
            if (_channelNameText != null) _channelNameText.text = StringUtil.GetGroupChannelName(_groupChannel, CHANNEL_NAME_MAX_LENGTH);
            if (_lastMessageText != null) _lastMessageText.text = _groupChannel.LastMessage?.Message;
            if (_memberCountText != null) _memberCountText.text = _groupChannel.MemberCount.ToString();
            if (_unreadMessageCountText != null) _unreadMessageCountText.text = _groupChannel.UnreadMessageCount.ToString();
            if (_unreadMessageCountGameObject != null) _unreadMessageCountGameObject.SetActive(0 < _groupChannel.UnreadMessageCount);
            if (_lastChangedAtText != null)
            {
                if (_groupChannel.LastMessage != null)
                {
                    DateTimeOffset lastMessageCreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(_groupChannel.LastMessage.CreatedAt);
                    _lastChangedAtText.text = lastMessageCreatedAt.ToLocalTime().ToString("h:mm tt");
                }
                else
                {
                    DateTimeOffset createdAt = DateTimeOffset.FromUnixTimeSeconds(_groupChannel.CreatedAt);
                    _lastChangedAtText.text = createdAt.ToLocalTime().ToString("h:mm tt");
                }
            }
        }

        private void OnClickEnterChannelButton()
        {
            SampleChatMain.Instance.OpenState(StateType.GroupChannel, new GroupChannelStateOpenParams(_groupChannel));
        }

        public string GetChannelUrl()
        {
            return _groupChannel?.Url;
        }
    }
}