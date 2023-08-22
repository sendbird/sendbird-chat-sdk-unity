// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sendbird.Chat.Sample
{
    public class MessageListScrollViewContent : MonoBehaviour
    {
        [SerializeField] private Text _userIdOrNameText = null;
        [SerializeField] private Text _messageText = null;
        [SerializeField] private Text _messageCreateAtText = null;
        private SbBaseMessage _message = null;

        private void Awake()
        {
            if (_userIdOrNameText == null) Debug.LogError("You need to set UserIdOrNameText in MessageListScrollViewContent");
            if (_messageText == null) Debug.LogError("You need to set MessageText in MessageListScrollViewContent");
            if (_messageCreateAtText == null) Debug.LogError("You need to set MessageCreateAtText in MessageListScrollViewContent");
        }

        public void ResetFromGroupChannel(SbBaseMessage inMessage)
        {
            _message = inMessage;
            if (_message == null)
                return;

            if (_userIdOrNameText != null)
            {
                if (_message is SbAdminMessage)
                {
                    _userIdOrNameText.text = "Admin";
                }
                else if (_message.Sender != null)
                {
                    if (string.IsNullOrEmpty(_message.Sender.Nickname) == false)
                    {
                        _userIdOrNameText.text = _message.Sender.Nickname;
                    }
                    else if (string.IsNullOrEmpty(_message.Sender.UserId) == false)
                    {
                        _userIdOrNameText.text = _message.Sender.UserId;
                    }
                }
                else
                {
                    _userIdOrNameText.text = "Unknown";
                }
            }

            if (_messageText != null)
            {
                _messageText.text = _message.Message;
            }

            if (_messageCreateAtText != null)
            {
                DateTimeOffset lastMessageCreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(_message.CreatedAt);
                _messageCreateAtText.text = lastMessageCreatedAt.ToLocalTime().ToString("h:mm tt");
            }
        }

        public long GetMessageId()
        {
            if (_message != null)
                return _message.MessageId;

            //A valid MessageID is greater than zero.
            const long INVALID_MESSAGE_ID = -1;
            return INVALID_MESSAGE_ID;
        }
    }
}