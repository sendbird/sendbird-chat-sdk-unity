// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class SummaryUserInfoComponent : MonoBehaviour
    {
        [SerializeField] private Text _userId = null;
        [SerializeField] private Text _userName = null;
        [SerializeField] private Button _infoButton = null;

        private void Awake()
        {
            if (_infoButton == null) { Debug.LogError("You need to set DetailUserInfoButton in SummaryUserInfoComponent"); }

            _infoButton.onClick.AddListener(OnClickDetailUserInfoButton);
        }

        public void ResetFromUser(SbUser inUser)
        {
            if (_userId != null) _userId.text = inUser.UserId;
            if (_userName != null) _userName.text = inUser.Nickname;
        }

        private void OnClickDetailUserInfoButton()
        {
            SampleChatMain.Instance.OpenInfoPopup();
        }
    }
}