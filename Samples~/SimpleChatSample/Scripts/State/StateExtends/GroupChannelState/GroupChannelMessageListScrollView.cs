// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Sendbird.Chat.Sample
{
    public class GroupChannelMessageListScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private MessageListScrollViewContent _otherMessageContentPrefab = null;
        [SerializeField] private MessageListScrollViewContent _myMessageContentPrefab = null;

        private readonly List<MessageListScrollViewContent> _activeScrollContents = new List<MessageListScrollViewContent>();
        private Coroutine _refreshScrollCoroutine = null;
        private GroupChannelState _ownerGroupChannelStateRef = null;
        private readonly Stopwatch _loadIntervalStopwatch = new Stopwatch();

        public void Initialize(GroupChannelState inGroupChannelListState)
        {
            _ownerGroupChannelStateRef = inGroupChannelListState;

            if (_ownerGroupChannelStateRef == null) Debug.LogError("GroupChannelListState is null in GroupChannelListScrollView.");
            if (_scrollRect == null) Debug.LogError("You need to set ScrollRect in GroupChannelListScrollView.");
            if (_otherMessageContentPrefab == null) Debug.LogError("You need to set OtherMessageContentPrefab in GroupChannelListScrollView.");
            if (_myMessageContentPrefab == null) Debug.LogError("You need to set MyMessageContentPrefab in GroupChannelListScrollView.");

            _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        public void Terminate()
        {
            _ownerGroupChannelStateRef = null;
        }

        public void OnOpenState()
        {
            if (_ownerGroupChannelStateRef != null)
            {
                _ownerGroupChannelStateRef.MessageManager.OnMessageListChanged += OnMessageListChanged;
            }

            gameObject.SetActive(true);
        }

        public void OnCloseState()
        {
            _ownerGroupChannelStateRef.MessageManager.OnMessageListChanged -= OnMessageListChanged;
            gameObject.SetActive(false);
            StopRefreshAndDeleteAllScrollViewContents();
        }

        public void OnMessageListChanged(IReadOnlyList<SbBaseMessage> inAllMessageList, bool inChangedFromLoad)
        {
            StopAndRefreshScrollContents(inAllMessageList, inChangedFromLoad);
        }

        private void OnScrollRectValueChanged(Vector2 inPosition)
        {
            if (_refreshScrollCoroutine != null)
                return;

            const int LOAD_INTERVAL_MILLISECONDS = 1000;
            if (_loadIntervalStopwatch.IsRunning && _loadIntervalStopwatch.ElapsedMilliseconds <= LOAD_INTERVAL_MILLISECONDS)
                return;

            if (1.0f <= inPosition.y)
            {
                if (_ownerGroupChannelStateRef.MessageManager.LoadPrevMessageListIfHasPrevious())
                {
                    _loadIntervalStopwatch.Restart();
                }
            }
        }

        private void StopAndRefreshScrollContents(IReadOnlyList<SbBaseMessage> inMessageList, bool inChangedFromLoad)
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            _refreshScrollCoroutine = StartCoroutine(RefreshScrollContentsCoroutine(inMessageList, inChangedFromLoad));
        }

        private IEnumerator RefreshScrollContentsCoroutine(IReadOnlyList<SbBaseMessage> inMessageList, bool inChangedFromLoad)
        {
            if (_scrollRect == null || _scrollRect.content == null || inMessageList == null)
                yield break;

            for (int index = 0; index < inMessageList.Count; index++)
            {
                if (inMessageList[index] == null)
                    continue;

                MessageListScrollViewContent roomListScrollViewContent = FindScrollViewContentFromActiveList(inMessageList[index]);
                if (roomListScrollViewContent == null)
                {
                    if (inMessageList[index].Sender != null && inMessageList[index].Sender.UserId == SendbirdChat.CurrentUser?.UserId)
                    {
                        roomListScrollViewContent = Instantiate(_myMessageContentPrefab);
                    }
                    else
                    {
                        roomListScrollViewContent = Instantiate(_otherMessageContentPrefab);
                    }

                    roomListScrollViewContent.transform.SetParent(_scrollRect.content);
                    _activeScrollContents.Add(roomListScrollViewContent);
                }

                roomListScrollViewContent.ResetFromGroupChannel(inMessageList[index]);
                roomListScrollViewContent.transform.SetSiblingIndex(index);

                yield return null;
            }

            if (inChangedFromLoad == false)
                _scrollRect.verticalNormalizedPosition = 0.0f;

            yield return null;
            _refreshScrollCoroutine = null;
        }

        private MessageListScrollViewContent FindScrollViewContentFromActiveList(SbBaseMessage inMessage)
        {
            if (inMessage == null)
                return null;

            return _activeScrollContents.Find(inContent => inContent.GetMessageId() == inMessage.MessageId);
        }

        private void StopRefreshAndDeleteAllScrollViewContents()
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            foreach (MessageListScrollViewContent scrollItem in _activeScrollContents)
            {
                GameObject.Destroy(scrollItem.gameObject);
            }

            _activeScrollContents.Clear();
        }
    }
}