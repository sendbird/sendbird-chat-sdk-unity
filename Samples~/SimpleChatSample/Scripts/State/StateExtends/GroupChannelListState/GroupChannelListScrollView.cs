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
    public class GroupChannelListScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private GroupChannelListScrollViewContent _scrollViewContentPrefab = null;
        [SerializeField] private GameObject _emptyNotifyGameObject = null;
        private readonly List<GroupChannelListScrollViewContent> _activeScrollContents = new List<GroupChannelListScrollViewContent>();
        private readonly Queue<GroupChannelListScrollViewContent> _scrollContentsPool = new Queue<GroupChannelListScrollViewContent>();
        private Coroutine _refreshScrollCoroutine = null;
        private GroupChannelListState _ownerGroupChannelListStateRef = null;
        private readonly Stopwatch _loadIntervalStopwatch = new Stopwatch();

        public void Initialize(GroupChannelListState inGroupChannelListState)
        {
            _ownerGroupChannelListStateRef = inGroupChannelListState;

            if (_ownerGroupChannelListStateRef == null) Debug.LogError("GroupChannelListState is null in GroupChannelListScrollView.");
            if (_scrollRect == null) Debug.LogError("You need to set ScrollRect in GroupChannelListScrollView.");
            if (_scrollViewContentPrefab == null) Debug.LogError("You need to set RoomScrollViewContentPrefab in GroupChannelListScrollView.");

            _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        public void Terminate()
        {
            _ownerGroupChannelListStateRef = null;
        }

        public void OnOpenState()
        {
            _ownerGroupChannelListStateRef.GroupChannelManager.OnChannelListChanged += OnChannelListChanged;
            gameObject.SetActive(true);
        }

        public void OnCloseState()
        {
            _ownerGroupChannelListStateRef.GroupChannelManager.OnChannelListChanged -= OnChannelListChanged;
            gameObject.SetActive(false);
            StopRefreshAndRestoreAllScrollViewContents();
        }

        private void OnChannelListChanged(IReadOnlyList<SbGroupChannel> inAllChannelList)
        {
            StopAndRefreshScrollContents(inAllChannelList);
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
                if (_ownerGroupChannelListStateRef.GroupChannelManager.LoadNextChannelListIfHasNext())
                {
                    _loadIntervalStopwatch.Restart();
                }
            }
        }

        private void StopAndRefreshScrollContents(IReadOnlyList<SbGroupChannel> inChannelList)
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            _refreshScrollCoroutine = StartCoroutine(RefreshScrollContentsCoroutine(inChannelList));
        }

        private IEnumerator RefreshScrollContentsCoroutine(IReadOnlyList<SbGroupChannel> inChannelList)
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

                GroupChannelListScrollViewContent roomListScrollViewContent = FindScrollViewContentFromActiveList(inChannelList[index]);
                if (roomListScrollViewContent == null)
                {
                    roomListScrollViewContent = GetScrollViewContentFromPool();
                    roomListScrollViewContent.transform.SetParent(_scrollRect.content);
                    _activeScrollContents.Add(roomListScrollViewContent);
                }

                roomListScrollViewContent.ResetFromGroupChannel(inChannelList[index]);
                roomListScrollViewContent.transform.SetSiblingIndex(index);

                yield return null;
            }

            yield return null;
            _refreshScrollCoroutine = null;
        }

        private GroupChannelListScrollViewContent FindScrollViewContentFromActiveList(SbGroupChannel inGroupChannel)
        {
            if (inGroupChannel == null || string.IsNullOrEmpty(inGroupChannel.Url))
                return null;

            return _activeScrollContents.Find(inContent => inContent.GetChannelUrl() == inGroupChannel.Url);
        }

        private GroupChannelListScrollViewContent GetScrollViewContentFromPool()
        {
            GroupChannelListScrollViewContent scrollViewContent = null;
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

        private void RestoreScrollViewContentToPool(GroupChannelListScrollViewContent inScrollViewContent)
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

            foreach (GroupChannelListScrollViewContent scrollItem in _activeScrollContents)
            {
                RestoreScrollViewContentToPool(scrollItem);
            }

            _activeScrollContents.Clear();
        }
    }
}