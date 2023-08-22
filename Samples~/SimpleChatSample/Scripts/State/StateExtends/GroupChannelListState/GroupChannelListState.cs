// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelListState : StateAbstract
    {
        [SerializeField] private SummaryUserInfoComponent _summaryUserInfo = null;
        [SerializeField] private Button _createChannelButton = null;
        [SerializeField] private Button _openChannelListButton = null;
        [SerializeField] private GroupChannelCreatePopup _groupChannelCreatePopup = null;
        [SerializeField] private GroupChannelListScrollView _groupChannelListScrollView = null;
        public GroupChannelManager GroupChannelManager { get; } = new GroupChannelManager();

        public override void InitializeState()
        {
            if (_summaryUserInfo == null) Debug.LogError("You need to set ChannelListUserInfo in GroupChannelListState");
            if (_createChannelButton == null) Debug.LogError("You need to set CreateChannelButton in GroupChannelListState");
            if (_openChannelListButton == null) Debug.LogError("You need to set OpenChannelListButton in GroupChannelListState");
            if (_groupChannelCreatePopup == null) Debug.LogError("You need to set GroupChannelCreatePopup in GroupChannelListState");
            if (_groupChannelListScrollView == null) Debug.LogError("You need to set GroupChannelListScrollView in GroupChannelListState");

            _createChannelButton.onClick.AddListener(OnClickCreateChannelButton);
            _openChannelListButton.onClick.AddListener(OnClickOpenChannelListButton);
            _groupChannelListScrollView.Initialize(this);
        }

        public override void TerminateState()
        {
            _groupChannelListScrollView.Terminate();
        }

        public override StateType GetStateType()
        {
            return StateType.GroupChannelList;
        }

        public override void OpenState(OpenParamsAbstract inOpenParamsAbstract)
        {
            _summaryUserInfo.ResetFromUser(SendbirdChat.CurrentUser);
            GroupChannelManager.OnOpenState();
            _groupChannelListScrollView.OnOpenState();
            _groupChannelCreatePopup.gameObject.SetActive(false);
        }

        public override void CloseState()
        {
            _groupChannelListScrollView.OnCloseState();
            GroupChannelManager.OnCloseState();
        }

        private void OnClickCreateChannelButton()
        {
            _groupChannelCreatePopup.gameObject.SetActive(true);
        }

        private void OnClickOpenChannelListButton()
        {
            SampleChatMain.Instance.OpenState(StateType.OpenChannelList);
        }
    }
}