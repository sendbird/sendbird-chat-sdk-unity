// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelCreatePopup : MonoBehaviour
    {
        [SerializeField] private Button _createButton = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private InputField _channelNameInputField = null;
        [SerializeField] private InputField _customTypeInputField = null;
        private SbOpenChannel _createdChannel = null;

        private void Awake()
        {
            if (_createButton == null) Debug.LogError("You need to set CreateButton in OpenChannelCreatePopup");
            if (_closeButton == null) Debug.LogError("You need to set CloseButton in OpenChannelCreatePopup");
            if (_channelNameInputField == null) Debug.LogError("You need to set ChannelNameInputField in OpenChannelCreatePopup");
            if (_customTypeInputField == null) Debug.LogError("You need to set CustomTypeInputField in OpenChannelCreatePopup");

            _createButton.onClick.AddListener(OnClickCreateButton);
            _closeButton.onClick.AddListener(OnClickCloseButton);
        }

        private void OnEnable()
        {
            _channelNameInputField.text = string.Empty;
            _customTypeInputField.text = string.Empty;
        }

        private void OnClickCloseButton()
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            base.gameObject.SetActive(false);
        }

        private void OnClickCreateButton()
        {
            SbOpenChannelCreateParams openChannelCreateParams = new SbOpenChannelCreateParams();
            if (_channelNameInputField != null) openChannelCreateParams.Name = _channelNameInputField.text;
            if (_customTypeInputField != null) openChannelCreateParams.CustomType = _customTypeInputField.text;

            SampleChatMain.Instance.BlockUI();
            SendbirdChat.OpenChannel.CreateChannel(openChannelCreateParams, OpenChannelCreateCompleteHandler);

            void OpenChannelCreateCompleteHandler(SbOpenChannel inChannel, SbError inError)
            {
                if (inError != null)
                {
                    SampleChatMain.Instance.UnblockUI();
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                    return;
                }

                _createdChannel = inChannel;
                _createdChannel.Enter(OnEnterCompleteHandler);
            }

            void OnEnterCompleteHandler(SbError inError)
            {
                SampleChatMain.Instance.UnblockUI();
                if (inError != null)
                {
                    SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                    return;
                }

                SampleChatMain.Instance.OpenState(StateType.OpenChannel, new OpenChannelStateOpenParams(_createdChannel));
            }
        }
    }
}