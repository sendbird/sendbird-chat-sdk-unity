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
    public class OpenChannelListScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private OpenChannelListScrollViewContent _scrollViewContentPrefab = null;
        [SerializeField] private GameObject _emptyNotifyGameObject = null;

        private readonly List<OpenChannelListScrollViewContent> _activeScrollContents = new List<OpenChannelListScrollViewContent>();
        private readonly Queue<OpenChannelListScrollViewContent> _scrollContentsPool = new Queue<OpenChannelListScrollViewContent>();
        private Coroutine _refreshScrollCoroutine = null;
        private OpenChannelListState _ownerOpenChannelListStateRef = null;
        private readonly Stopwatch _loadIntervalStopwatch = new Stopwatch();

        public void Initialize(OpenChannelListState inOpenChannelListState)
        {
            _ownerOpenChannelListStateRef = inOpenChannelListState;

            if (_ownerOpenChannelListStateRef == null) Debug.LogError("OpenChannelListState is null in OpenChannelListScrollView.");
            if (_scrollRect == null) Debug.LogError("You need to set ScrollRect in OpenChannelListScrollView.");
            if (_scrollViewContentPrefab == null) Debug.LogError("You need to set RoomScrollViewContentPrefab in OpenChannelListScrollView.");

            _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        public void Terminate()
        {
            _ownerOpenChannelListStateRef = null;
        }

        public void OnOpenState()
        {
            _ownerOpenChannelListStateRef.OpenChannelManager.OnChannelListChanged += OnChannelListChanged;
            gameObject.SetActive(true);
        }

        public void OnCloseState()
        {
            _ownerOpenChannelListStateRef.OpenChannelManager.OnChannelListChanged -= OnChannelListChanged;
            gameObject.SetActive(false);
            StopRefreshAndRestoreAllScrollViewContents();
        }

        private void OnChannelListChanged(IReadOnlyList<SbOpenChannel> inAllOpenChannels)
        {
            StopAndRefreshScrollContents(inAllOpenChannels);
        }

        private void OnScrollRectValueChanged(Vector2 inPosition)
        {
            if (_refreshScrollCoroutine != null)
                return;

            const int LOAD_INTERVAL_MILLISECONDS = 1000;
            if (_loadIntervalStopwatch.IsRunning && _loadIntervalStopwatch.ElapsedMilliseconds <= LOAD_INTERVAL_MILLISECONDS)
                return;

            if (inPosition.y <= 0.0f)
            {
                if (_ownerOpenChannelListStateRef.OpenChannelManager.LoadPreviousChannelListIfHasPrevious())
                {
                    _loadIntervalStopwatch.Restart();
                }
            }
            else if (1.0f <= inPosition.y)
            {
                if (_ownerOpenChannelListStateRef.OpenChannelManager.LoadLatestChannelList())
                {
                    _loadIntervalStopwatch.Restart();
                }
            }
        }

        private void StopAndRefreshScrollContents(IReadOnlyList<SbOpenChannel> inChannelList)
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            _refreshScrollCoroutine = StartCoroutine(RefreshScrollContentsCoroutine(inChannelList));
        }

        private IEnumerator RefreshScrollContentsCoroutine(IReadOnlyList<SbOpenChannel> inChannelList)
        {
            if (_scrollRect == null || _scrollRect.content == null || inChannelList == null)
                yield break;

            if (_emptyNotifyGameObject != null)
            {
                _emptyNotifyGameObject.gameObject.SetActive(inChannelList.Count <= 0);
            }

            for (int index = 0; index < inChannelList.Count; index++)
            {
                if (inChannelList[index] == null)
                    continue;

                OpenChannelListScrollViewContent roomListScrollViewContent = FindScrollViewContentFromActiveList(inChannelList[index]);
                if (roomListScrollViewContent == null)
                {
                    roomListScrollViewContent = GetScrollViewContentFromPool();
                    roomListScrollViewContent.transform.SetParent(_scrollRect.content);
                    _activeScrollContents.Add(roomListScrollViewContent);
                }

                roomListScrollViewContent.ResetFromOpenChannel(inChannelList[index]);
                roomListScrollViewContent.transform.SetSiblingIndex(index);

                yield return null;
            }

            yield return null;
            _refreshScrollCoroutine = null;
        }

        private OpenChannelListScrollViewContent FindScrollViewContentFromActiveList(SbOpenChannel inOpenChannel)
        {
            if (inOpenChannel == null || string.IsNullOrEmpty(inOpenChannel.Url))
                return null;

            return _activeScrollContents.Find(inContent => inContent.GetChannelUrl() == inOpenChannel.Url);
        }

        private OpenChannelListScrollViewContent GetScrollViewContentFromPool()
        {
            OpenChannelListScrollViewContent scrollViewContent = null;
            if (0 < _scrollContentsPool.Count)
            {
                scrollViewContent = _scrollContentsPool.Dequeue();
            }
            else
            {
                scrollViewContent = Instantiate(_scrollViewContentPrefab, transform);
            }

            scrollViewContent.gameObject.SetActive(true);
            return scrollViewContent;
        }

        private void RestoreScrollViewContentToPool(OpenChannelListScrollViewContent inScrollViewContent)
        {
            if (inScrollViewContent == null)
                return;

            inScrollViewContent.transform.SetParent(transform);
            inScrollViewContent.gameObject.SetActive(false);
            _scrollContentsPool.Enqueue(inScrollViewContent);
        }

        private void StopRefreshAndRestoreAllScrollViewContents()
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            foreach (OpenChannelListScrollViewContent scrollItem in _activeScrollContents)
            {
                RestoreScrollViewContentToPool(scrollItem);
            }

            _activeScrollContents.Clear();
        }
    }
}