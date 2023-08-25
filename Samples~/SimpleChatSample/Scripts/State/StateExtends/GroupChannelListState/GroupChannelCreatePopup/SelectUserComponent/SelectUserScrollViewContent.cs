// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class SelectUserScrollViewContent : MonoBehaviour
    {
        [SerializeField] private Text _userIdText = null;
        [SerializeField] private Toggle _selectToggle = null;

        public SelectUserInfo SelectUserInfo { get; private set; }
        public Action OnChangedSelect { get; set; }

        private void Awake()
        {
            if (_userIdText == null) Debug.LogError("You need to set UserIdText in SelectUserScrollViewContent");
            if (_selectToggle == null) Debug.LogError("You need to set SelectToggle in SelectUserScrollViewContent");

            _selectToggle.onValueChanged.AddListener(OnChangeSelectToggle);
        }

        public void SetUserInfo(SelectUserInfo inUserInfo)
        {
            SelectUserInfo = inUserInfo;
            if (SelectUserInfo != null)
            {
                _userIdText.text = SelectUserInfo.UserId;
                _selectToggle.SetIsOnWithoutNotify(SelectUserInfo.IsSelected);
            }
        }

        private void OnChangeSelectToggle(bool inIsOn)
        {
            if (SelectUserInfo != null)
            {
                SelectUserInfo.IsSelected = inIsOn;
            }

            OnChangedSelect?.Invoke();
        }
    }
}