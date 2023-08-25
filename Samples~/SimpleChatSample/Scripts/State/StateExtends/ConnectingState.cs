// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class ConnectingState : StateAbstract
    {
        [SerializeField] private Button _connectButton = null;
        [SerializeField] private InputField _appIdInputField = null;
        [SerializeField] private InputField _userIdInputField = null;
        [SerializeField] private InputField _authTokenInputField = null;

        public override void InitializeState()
        {
            if (_appIdInputField != null) { _appIdInputField.onValueChanged.AddListener(OnChangeAppId); }
            else { Debug.LogError("You have to set AppId InputField"); }

            if (_userIdInputField != null) { _userIdInputField.onValueChanged.AddListener(OnChangeUserId); }
            else { Debug.LogError("You have to set UserId InputField"); }

            if (_connectButton != null) { _connectButton.onClick.AddListener(OnClickConnectButton); }
            else { Debug.LogError("You have to set Authenticate Button"); }
        }

        private void Start()
        {
            if (_appIdInputField != null)
            {
                _appIdInputField.text = SampleChatMain.Instance.AppId;
            }
        }

        public override StateType GetStateType()
        {
            return StateType.Connecting;
        }

        public override void OpenState(OpenParamsAbstract inOpenParamsAbstract)
        {
            CheckAuthenticateButtonInteractable();
        }

        private void OnChangeAppId(string inText)
        {
            CheckAuthenticateButtonInteractable();
        }

        private void OnChangeUserId(string inText)
        {
            CheckAuthenticateButtonInteractable();
        }

        private void OnClickConnectButton()
        {
            string appId = _appIdInputField.text;
            if (string.IsNullOrEmpty(appId))
            {
                SampleChatMain.Instance.OpenNotifyPopup("You must enter the AppId");
                return;
            }

            SbInitParams initParams = new SbInitParams(appId, SbLogLevel.Info, inAppVersion: "YourApplicationVersion");
            SendbirdChat.Init(initParams);

            string userId = _userIdInputField.text;
            if (string.IsNullOrEmpty(userId))
            {
                SampleChatMain.Instance.OpenNotifyPopup("You must enter the UserId");
                return;
            }

            string authTokenIfExist = string.Empty;
            if (_authTokenInputField != null)
                authTokenIfExist = _authTokenInputField.text;

            SampleChatMain.Instance.BlockUI();

            _connectButton.interactable = false;

            SendbirdChat.Connect(userId, authTokenIfExist, null, null, ConnectResultHandler);
        }

        private void ConnectResultHandler(SbUser inUser, SbError inError)
        {
            SampleChatMain.Instance.UnblockUI();

            if (inError == null)
            {
                SampleChatMain.Instance.OpenState(StateType.OpenChannelList);
            }
            else
            {
                SampleChatMain.Instance.OpenNotifyPopup($"ErrorCode:{inError.ErrorCode} ErrorMessage:{inError.ErrorMessage}");
            }

            CheckAuthenticateButtonInteractable();
        }

        private void CheckAuthenticateButtonInteractable()
        {
            _connectButton.interactable = !string.IsNullOrEmpty(_userIdInputField.text);
        }
    }
}