// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelState : StateAbstract
    {
        [SerializeField] private Text _channelNameText = null;
        [SerializeField] private Text _channelMemberCountText = null;
        [SerializeField] private GroupChannelMessageListScrollView _messageListScrollView = null;
        [SerializeField] private InputField _messageInputField = null;
        [SerializeField] private Button _sendMessageButton = null;
        [SerializeField] private Button _backButton = null;

        public GroupChannelMessageManager MessageManager { get; } = new GroupChannelMessageManager();
        private SbGroupChannel _groupChannel = null;

        public override StateType GetStateType()
        {
            return StateType.GroupChannel;
        }

        public override void InitializeState()
        {
            if (_messageListScrollView == null) Debug.LogError("You need to set GroupChannelListScrollView in GroupChannelState");
            if (_messageInputField == null) Debug.LogError("You need to set MessageInputField in GroupChannelState");
            if (_sendMessageButton == null) Debug.LogError("You need to set SendMessageButton in GroupChannelState");
            if (_backButton == null) Debug.LogError("You need to set BackButton in GroupChannelState");

            _backButton.onClick.AddListener(OnClickBackButton);
            _sendMessageButton.onClick.AddListener(OnClickSendMessageButton);
            _messageInputField.onValueChanged.AddListener(OnChangedMessageInputField);
            _messageListScrollView.Initialize(this);
        }

        public override void TerminateState()
        {
            _messageListScrollView.Terminate();
        }

        public override void OpenState(OpenParamsAbstract inOpenParamsAbstract)
        {
            if (inOpenParamsAbstract is GroupChannelStateOpenParams groupChannelStateOpenParams)
            {
                _groupChannel = groupChannelStateOpenParams.GroupChannel;
            }

            if (_groupChannel != null)
            {
                MessageManager.OnOpenState(_groupChannel);
                _messageListScrollView.OnOpenState();
                ResetChannelNameAndCount(_groupChannel);
                _groupChannel.MarkAsRead(null);
            }
            else
            {
                Debug.LogError("GroupChannelState::OpenState() GroupChannel is null.");
            }

            _messageInputField.text = string.Empty;
        }

        public override void CloseState()
        {
            _messageListScrollView.OnCloseState();
            MessageManager.OnCloseState();
        }

        private void ResetChannelNameAndCount(SbGroupChannel inGroupChannel)
        {
            if (_channelNameText != null)
            {
                const int CHANNEL_NAME_MAX_LENGTH = 20;
                _channelNameText.text = inGroupChannel == null ? string.Empty : StringUtil.GetGroupChannelName(inGroupChannel, CHANNEL_NAME_MAX_LENGTH);
            }

            if (_channelMemberCountText != null)
            {
                _channelMemberCountText.text = inGroupChannel == null ? string.Empty : inGroupChannel.MemberCount.ToString();
            }
        }

        private void OnClickBackButton()
        {
            SampleChatMain.Instance.OpenState(StateType.GroupChannelList);
        }

        private void OnClickSendMessageButton()
        {
            void OnSendCompletedHandler()
            {
                _messageInputField.text = string.Empty;
            }

            if (string.IsNullOrEmpty(_messageInputField.text) == false)
            {
                MessageManager.SendMessage(_messageInputField.text, OnSendCompletedHandler);
            }
        }

        private void OnChangedMessageInputField(string inText)
        {
            _sendMessageButton.interactable = string.IsNullOrEmpty(inText) == false;
        }
    }
}