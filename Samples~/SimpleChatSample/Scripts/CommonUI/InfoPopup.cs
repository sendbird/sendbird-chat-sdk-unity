// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class InfoPopup : MonoBehaviour
    {
        [SerializeField] private Text _sdkVersion = null;
        [SerializeField] private Text _osVersion = null;
        [SerializeField] private Text _applicationId = null;
        [SerializeField] private Text _userId = null;
        [SerializeField] private Text _userName = null;
        [SerializeField] private Button _closeButton = null;


        private void Awake()
        {
            if (_closeButton != null) _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnEnable()
        {
            if (SendbirdChat.CurrentUser != null)
            {
                if (_userId != null) _userId.text = SendbirdChat.CurrentUser.UserId;
                if (_userName != null) _userName.text = SendbirdChat.CurrentUser.Nickname;
            }

            if (_applicationId != null) _applicationId.text = SendbirdChat.ApplicationId;
            if (_sdkVersion != null) _sdkVersion.text = SendbirdChat.SDKVersion;
            if (_osVersion != null) _osVersion.text = SendbirdChat.OSVersion;
        }
        
        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}