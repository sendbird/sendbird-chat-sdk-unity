// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Sendbird.Chat.Sample
{
    public class SelectUserScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private SelectUserScrollViewContent _scrollViewContentPrefab = null;

        private readonly List<SelectUserScrollViewContent> _activeScrollContents = new List<SelectUserScrollViewContent>();
        private readonly Queue<SelectUserScrollViewContent> _scrollContentsPool = new Queue<SelectUserScrollViewContent>();
        private Coroutine _refreshScrollCoroutine = null;
        private SelectUserComponent _ownerSelectUserComponentRef = null;
        private SbApplicationUserListQuery _userListQuery = null;
        private readonly List<SelectUserInfo> _userInfos = new List<SelectUserInfo>();
        private readonly Stopwatch _queryIntervalStopwatch = new Stopwatch();

        public void Initialize(SelectUserComponent inSelectUserComponent)
        {
            if (_scrollRect == null) Debug.LogError("You need to set ScrollRect in SelectUserScrollView");
            if (_scrollViewContentPrefab == null) Debug.LogError("You need to set ScrollViewContentPrefab in SelectUserScrollView");

            _ownerSelectUserComponentRef = inSelectUserComponent;
            _userInfos.Clear();

            SbApplicationUserListQueryParams queryParams = new SbApplicationUserListQueryParams
            {
                Limit = 20
            };
            _userListQuery = SendbirdChat.CreateApplicationUserListQuery(queryParams);
            _userListQuery.LoadNextPage(OnLoadNextPageCompleteHandler);

            _scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        public void Terminate()
        {
            StopRefreshAndRestoreAllScrollViewContents();
            _ownerSelectUserComponentRef = null;
            _userInfos.Clear();
            _scrollRect.onValueChanged.RemoveListener(OnScrollRectValueChanged);
            _userListQuery = null;
        }

        private void OnChangeSelectUsers()
        {
            if (_ownerSelectUserComponentRef != null)
            {
                _ownerSelectUserComponentRef.OnChangedSelectedUsers(_userInfos);
            }
        }

        private void OnLoadNextPageCompleteHandler(IReadOnlyList<SbUser> inUsers, SbError inError)
        {
            if (inError != null)
            {
                Debug.LogWarning($"OnLoadNextPageCompleteHandler() ErrorCode:{inError.ErrorCode}\nErrorMessage:{inError.ErrorMessage}");
                return;
            }

            if (inUsers == null || inUsers.Count <= 0)
                return;

            foreach (SbUser sbUser in inUsers)
            {
                if (_userInfos.Any(inUserInfo => inUserInfo.UserId == sbUser.UserId) == false)
                {
                    _userInfos.Add(new SelectUserInfo(sbUser.UserId, false));
                }
            }

            StopAndRefreshScrollContents();
        }

        private void OnScrollRectValueChanged(Vector2 inPosition)
        {
            if (_userListQuery == null || _userListQuery.IsLoading || _userListQuery.HasNext == false)
                return;

            if (_refreshScrollCoroutine != null)
                return;

            const int QUERY_INTERVAL_MILLISECONDS = 1000;
            if (_queryIntervalStopwatch.IsRunning && _queryIntervalStopwatch.ElapsedMilliseconds <= QUERY_INTERVAL_MILLISECONDS)
                return;

            if (inPosition.y <= 0.0f)
            {
                _queryIntervalStopwatch.Restart();
                _userListQuery.LoadNextPage(OnLoadNextPageCompleteHandler);
            }
        }

        private void StopAndRefreshScrollContents()
        {
            if (_refreshScrollCoroutine != null)
            {
                StopCoroutine(_refreshScrollCoroutine);
                _refreshScrollCoroutine = null;
            }

            _refreshScrollCoroutine = StartCoroutine(RefreshScrollContentsCoroutine());
        }

        private IEnumerator RefreshScrollContentsCoroutine()
        {
            if (_scrollRect == null || _scrollRect.content == null)
                yield break;

            foreach (SelectUserInfo userInfo in _userInfos)
            {
                SelectUserScrollViewContent roomListScrollViewContent = FindScrollViewContentFromActiveList(userInfo);
                if (roomListScrollViewContent == null)
                {
                    roomListScrollViewContent = GetScrollViewContentFromPool();
                    roomListScrollViewContent.transform.SetParent(_scrollRect.content);
                    roomListScrollViewContent.SetUserInfo(userInfo);
                    roomListScrollViewContent.OnChangedSelect = OnChangeSelectUsers;
                    _activeScrollContents.Add(roomListScrollViewContent);
                }

                yield return null;
            }

            yield return null;
            _refreshScrollCoroutine = null;
        }

        private SelectUserScrollViewContent FindScrollViewContentFromActiveList(SelectUserInfo inUserInfo)
        {
            if (inUserInfo == null || string.IsNullOrEmpty(inUserInfo.UserId))
                return null;

            return _activeScrollContents.Find(inContent => inContent.SelectUserInfo.UserId == inUserInfo.UserId);
        }

        private SelectUserScrollViewContent GetScrollViewContentFromPool()
        {
            SelectUserScrollViewContent scrollViewContent = null;
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

        private void RestoreScrollViewContentToPool(SelectUserScrollViewContent inScrollViewContent)
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

            foreach (SelectUserScrollViewContent scrollItem in _activeScrollContents)
            {
                RestoreScrollViewContentToPool(scrollItem);
            }

            _activeScrollContents.Clear();
        }
    }
}