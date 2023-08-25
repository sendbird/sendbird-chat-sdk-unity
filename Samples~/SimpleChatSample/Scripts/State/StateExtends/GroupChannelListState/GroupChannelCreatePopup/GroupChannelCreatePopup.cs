// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelCreatePopup : MonoBehaviour
    {
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private GameObject _createParamsGameObject = null;
        [SerializeField] private InputField _channelNameInputField = null;
        [SerializeField] private InputField _customTypeInputField = null;
        [SerializeField] private Button _nextButton = null;
        [SerializeField] private SelectUserComponent _selectUserComponent = null;

        private void Awake()
        {
            if (_nextButton == null) Debug.LogError("You need to set NextButton in GroupChannelCreatePopup");
            if (_closeButton == null) Debug.LogError("You need to set CloseButton in GroupChannelCreatePopup");
            if (_createParamsGameObject == null) Debug.LogError("You need to set CreateParamsGameObject in GroupChannelCreatePopup");
            if (_selectUserComponent == null) Debug.LogError("You need to set SelectUserComponent in GroupChannelCreatePopup");
            if (_channelNameInputField == null) Debug.LogError("You need to set ChannelNameInputField in GroupChannelCreatePopup");
            if (_customTypeInputField == null) Debug.LogError("You need to set CustomTypeInputField in GroupChannelCreatePopup");

            _nextButton.onClick.AddListener(OnClickNextButton);
            _closeButton.onClick.AddListener(OnClickCloseButton);

            _selectUserComponent.OnDoneButtonClickHandler = OnClickSelectUserDoneButton;
            _selectUserComponent.OnBackButtonClickHandler = OnClickSelectUserBackButton;
        }

        private void OnEnable()
        {
            _channelNameInputField.text = string.Empty;
            _customTypeInputField.text = string.Empty;
            _createParamsGameObject.SetActive(true);
            _selectUserComponent.Hide();
        }

        private void OnClickNextButton()
        {
            _createParamsGameObject.SetActive(false);
            _selectUserComponent.Show();
        }

        private void OnClickSelectUserBackButton()
        {
            _createParamsGameObject.SetActive(true);
            _selectUserComponent.Hide();
        }

        private void OnClickCloseButton()
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            base.gameObject.SetActive(false);
        }

        private void OnClickSelectUserDoneButton(List<string> inSelectedUserIds)
        {
            SbGroupChannelCreateParams groupChannelCreateParams = new SbGroupChannelCreateParams();
            if (_channelNameInputField != null) groupChannelCreateParams.Name = _channelNameInputField.text;
            if (_customTypeInputField != null) groupChannelCreateParams.CustomType = _customTypeInputField.text;
            groupChannelCreateParams.UserIds = inSelectedUserIds;

            SampleChatMain.Instance.BlockUI();
            SendbirdChat.GroupChannel.CreateChannel(groupChannelCreateParams, GroupChannelCreateCompleteHandler);
        }

        private void GroupChannelCreateCompleteHandler(SbGroupChannel inChannel, SbError inError)
        {
            SampleChatMain.Instance.UnblockUI();
            if (inError != null)
            {
                SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                return;
            }

            SampleChatMain.Instance.OpenState(StateType.GroupChannel, new GroupChannelStateOpenParams(inChannel));
        }
    }
}