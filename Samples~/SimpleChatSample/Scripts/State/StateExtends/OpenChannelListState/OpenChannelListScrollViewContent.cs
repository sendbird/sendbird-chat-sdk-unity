// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelListScrollViewContent : MonoBehaviour
    {
        [SerializeField] private Button _enterChannelButton = null;
        [SerializeField] private Text _channelNameText = null;
        [SerializeField] private Text _createdAtText = null;
        [SerializeField] private Text _participantCountText = null;
        private SbOpenChannel _openChannel = null;

        private void Awake()
        {
            if (_enterChannelButton == null)
            {
                Debug.LogError("You need to set EnterChannelButton in OpenChannelListScrollViewContent");
            }
            else
            {
                _enterChannelButton.onClick.AddListener(OnClickEnterChannelButton);
            }
        }

        public void ResetFromOpenChannel(SbOpenChannel inOpenChannel)
        {
            _openChannel = inOpenChannel;
            if (_openChannel == null)
                return;

            const int CHANNEL_NAME_MAX_LENGTH = 20;
            if (_channelNameText != null) _channelNameText.text = StringUtil.GetOpenChannelName(_openChannel, CHANNEL_NAME_MAX_LENGTH);
            if (_participantCountText != null) _participantCountText.text = _openChannel.ParticipantCount.ToString();
            if (_createdAtText != null)
            {
                DateTimeOffset createdAt = DateTimeOffset.FromUnixTimeSeconds(_openChannel.CreatedAt);
                _createdAtText.text = createdAt.ToLocalTime().ToString("h:mm tt");
            }
        }

        private void OnClickEnterChannelButton()
        {
            SampleChatMain.Instance.BlockUI();
            _openChannel.Enter(OnEnterCompleteHandler);

            void OnEnterCompleteHandler(SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                    return;
                }

                SampleChatMain.Instance.OpenState(StateType.OpenChannel, new OpenChannelStateOpenParams(_openChannel));
            }
        }

        public string GetChannelUrl()
        {
            return _openChannel?.Url;
        }
    }
}