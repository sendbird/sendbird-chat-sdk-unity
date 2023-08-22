// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class SelectUserComponent : MonoBehaviour
    {
        [SerializeField] private SelectUserScrollView _selectedUserScrollView = null;
        [SerializeField] private Text _selectedUserIdsText = null;
        [SerializeField] private Text _selectedUserIdsCountText = null;
        [SerializeField] private Button _backButton = null;
        [SerializeField] private Button _doneButton = null;
        private readonly List<string> _selectedUserIds = new List<string>();
        public Action<List<string>> OnDoneButtonClickHandler { get; set; } = null;
        public Action OnBackButtonClickHandler { get; set; } = null;

        private void Awake()
        {
            if (_selectedUserScrollView == null) Debug.LogError("You need to set SelectedUserIdScrollView in SelectUserComponent");
            if (_backButton == null) Debug.LogError("You need to set BackButton in SelectUserComponent");
            if (_doneButton == null) Debug.LogError("You need to set DoneButton in SelectUserComponent");

            _doneButton.onClick.AddListener(OnClickDoneButton);
            _backButton.onClick.AddListener(OnClickBackButton);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnChangedSelectedUsers(null);
            _selectedUserScrollView.Initialize(this);
        }

        public void Hide()
        {
            _selectedUserScrollView.Terminate();
            gameObject.SetActive(false);
        }

        public void OnChangedSelectedUsers(IReadOnlyList<SelectUserInfo> inSelectUserInfos)
        {
            StringBuilder selectedUserIdsStringBuilder = new StringBuilder();
            _selectedUserIds.Clear();
            if (inSelectUserInfos != null)
            {
                foreach (SelectUserInfo selectUserInfo in inSelectUserInfos)
                {
                    if (selectUserInfo.IsSelected)
                    {
                        if (0 < selectedUserIdsStringBuilder.Length)
                            selectedUserIdsStringBuilder.Append(", ");

                        _selectedUserIds.Add(selectUserInfo.UserId);
                        selectedUserIdsStringBuilder.Append(selectUserInfo.UserId);
                    }
                }
            }

            if (_selectedUserIdsText != null)
                _selectedUserIdsText.text = selectedUserIdsStringBuilder.ToString();

            if (_selectedUserIdsCountText != null)
            {
                const int MAX_MEMBER_COUNT_OF_GROUP_CHANNEL = 100;
                _selectedUserIdsCountText.text = $"{_selectedUserIds.Count}/{MAX_MEMBER_COUNT_OF_GROUP_CHANNEL}";
            }
        }

        private void OnClickDoneButton()
        {
            OnDoneButtonClickHandler?.Invoke(_selectedUserIds);
        }

        private void OnClickBackButton()
        {
            OnBackButtonClickHandler?.Invoke();
        }
    }
}