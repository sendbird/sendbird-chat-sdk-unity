// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelState : StateAbstract
    {
        [SerializeField] private Text _channelNameText = null;
        [SerializeField] private Text _channelParticipantCountText = null;
        [SerializeField] private OpenChannelMessageListScrollView _messageListScrollView = null;
        [SerializeField] private InputField _messageInputField = null;
        [SerializeField] private Button _sendMessageButton = null;
        [SerializeField] private Button _backButton = null;

        public OpenChannelMessageManager MessageManager { get; } = new OpenChannelMessageManager();
        private SbOpenChannel _openChannel = null;

        public override StateType GetStateType()
        {
            return StateType.OpenChannel;
        }

        public override void InitializeState()
        {
            if (_messageListScrollView == null) Debug.LogError("You need to set OpenChannelListScrollView in OpenChannelState");
            if (_messageInputField == null) Debug.LogError("You need to set MessageInputField in OpenChannelState");
            if (_sendMessageButton == null) Debug.LogError("You need to set SendMessageButton in OpenChannelState");
            if (_backButton == null) Debug.LogError("You need to set BackButton in OpenChannelState");

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
            if (inOpenParamsAbstract is OpenChannelStateOpenParams openChannelStateOpenParams)
            {
                _openChannel = openChannelStateOpenParams.OpenChannel;
            }

            if (_openChannel != null)
            {
                MessageManager.OnOpenState(_openChannel);
                MessageManager.OnChannelChanged += OnChangedChannel;
                _messageListScrollView.OnOpenState();
                ResetChannelNameAndCount(_openChannel);
            }
            else
            {
                Debug.LogError("OpenChannelState::OpenState() OpenChannel is null.");
            }

            _messageInputField.text = string.Empty;
        }

        public override void CloseState()
        {
            _messageListScrollView.OnCloseState();
            MessageManager.OnChannelChanged -= OnChangedChannel;
            MessageManager.OnCloseState();
        }

        private void ResetChannelNameAndCount(SbOpenChannel inOpenChannel)
        {
            if (_channelNameText != null)
            {
                const int CHANNEL_NAME_MAX_LENGTH = 20;
                _channelNameText.text = inOpenChannel == null ? string.Empty : StringUtil.GetOpenChannelName(inOpenChannel, CHANNEL_NAME_MAX_LENGTH);
            }

            if (_channelParticipantCountText != null)
            {
                _channelParticipantCountText.text = inOpenChannel == null ? string.Empty : inOpenChannel.ParticipantCount.ToString();
            }
        }

        private void OnClickBackButton()
        {
            SampleChatMain.Instance.BlockUI();
            _openChannel.Exit(OnExitCompleteHandler);
            void OnExitCompleteHandler(SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                SampleChatMain.Instance.OpenState(StateType.OpenChannelList);
            }
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

        private void OnChangedChannel(SbOpenChannel inOpenChannel)
        {
            _openChannel = inOpenChannel;
            ResetChannelNameAndCount(_openChannel);
        }
    }
}