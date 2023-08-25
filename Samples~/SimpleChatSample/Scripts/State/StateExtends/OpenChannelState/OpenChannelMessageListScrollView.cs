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
    public class OpenChannelMessageListScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private MessageListScrollViewContent _otherMessageContentPrefab = null;
        [SerializeField] private MessageListScrollViewContent _myMessageContentPrefab = null;

        private readonly List<MessageListScrollViewContent> _activeScrollContents = new List<MessageListScrollViewContent>();
        private Coroutine _refreshScrollCoroutine = null;
        private OpenChannelState _ownerOpenChannelStateRef = null;
        private readonly Stopwatch _loadIntervalStopwatch = new Stopwatch();

        public void Initialize(OpenChannelState inOpenChannelListState)
        {
            _ownerOpenChannelStateRef = inOpenChannelListState;

            if (_ownerOpenChannelStateRef == null) Debug.LogError("GroupChannelListState is null in OpenChannelMessageListScrollView.");
            if (_scrollRect == null) Debug.LogError("You need to set ScrollRect in OpenChannelMessageListScrollView.");
            if (_otherMessageContentPrefab == null) Debug.LogError("You need to set RoomScrollViewContentPrefab in OpenChannelMessageListScrollView.");

            _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        public void Terminate()
        {
            _ownerOpenChannelStateRef = null;
        }

        public void OnOpenState()
        {
            if (_ownerOpenChannelStateRef != null)
            {
                _ownerOpenChannelStateRef.MessageManager.OnMessageListChanged += OnMessageListChanged;
            }

            gameObject.SetActive(true);
        }

        public void OnCloseState()
        {
            _ownerOpenChannelStateRef.MessageManager.OnMessageListChanged -= OnMessageListChanged;
            gameObject.SetActive(false);
            StopRefreshAndDeleteAllScrollViewContents();
        }

        private void OnMessageListChanged(IReadOnlyList<SbBaseMessage> inAllMessageList, bool inChangedFromLoad)
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
                if (_ownerOpenChannelStateRef.MessageManager.LoadPrevMessageListIfHasPrevious())
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