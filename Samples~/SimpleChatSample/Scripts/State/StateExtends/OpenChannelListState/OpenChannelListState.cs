// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class OpenChannelListState : StateAbstract
    {
        [SerializeField] private SummaryUserInfoComponent _summaryUserInfo = null;
        [SerializeField] private Button _createChannelButton = null;
        [SerializeField] private Button _groupChannelListButton = null;
        [SerializeField] private OpenChannelCreatePopup _OpenChannelCreatePopup = null;
        [SerializeField] private OpenChannelListScrollView _OpenChannelListScrollView = null;
        public OpenChannelManager OpenChannelManager { get; } = new OpenChannelManager();

        public override void InitializeState()
        {
            if (_summaryUserInfo == null) Debug.LogError("You need to set ChannelListUserInfo in OpenChannelListState");
            if (_createChannelButton == null) Debug.LogError("You need to set CreateChannelButton in OpenChannelListState");
            if (_groupChannelListButton == null) Debug.LogError("You need to set OpenChannelListButton in OpenChannelListState");
            if (_OpenChannelCreatePopup == null) Debug.LogError("You need to set OpenChannelCreatePopup in OpenChannelListState");
            if (_OpenChannelListScrollView == null) Debug.LogError("You need to set OpenChannelListScrollView in OpenChannelListState");

            _createChannelButton.onClick.AddListener(OnClickCreateChannelButton);
            _groupChannelListButton.onClick.AddListener(OnClickGroupChannelListButton);
            _OpenChannelListScrollView.Initialize(this);
        }

        public override void TerminateState()
        {
            _OpenChannelListScrollView.Terminate();
        }

        public override StateType GetStateType()
        {
            return StateType.OpenChannelList;
        }

        public override void OpenState(OpenParamsAbstract inOpenParamsAbstract)
        {
            _summaryUserInfo.ResetFromUser(SendbirdChat.CurrentUser);
            OpenChannelManager.OnOpenState();
            _OpenChannelListScrollView.OnOpenState();
            _OpenChannelCreatePopup.gameObject.SetActive(false);
        }

        public override void CloseState()
        {
            _OpenChannelListScrollView.OnCloseState();
            OpenChannelManager.OnCloseState();
        }

        private void OnClickCreateChannelButton()
        {
            _OpenChannelCreatePopup.gameObject.SetActive(true);
        }

        private void OnClickGroupChannelListButton()
        {
            SampleChatMain.Instance.OpenState(StateType.GroupChannelList);
        }
    }
}