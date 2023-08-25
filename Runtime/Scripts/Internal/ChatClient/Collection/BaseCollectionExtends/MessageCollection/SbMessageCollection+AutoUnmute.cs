// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections;

namespace Sendbird.Chat
{
    public partial class SbMessageCollection
    {
        private void StopAndStartAutoUnmuteCoroutine(long inUnmuteRemainTimeMs)
        {
            StopAutoUnmuteCoroutineIfStarted();
            _autoUnmuteJob = CoroutineManager.Instance.StartCoroutine(AutoUnmuteCoroutine(inUnmuteRemainTimeMs));
        }

        private void StopAutoUnmuteCoroutineIfStarted()
        {
            if (_autoUnmuteJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_autoUnmuteJob);
                _autoUnmuteJob = null;
            }
        }

        private IEnumerator AutoUnmuteCoroutine(long inUnmuteRemainTimeMs)
        {
            const long NETWORK_BUFFER_TIME_MS = 1000;
            yield return new WaitForSecondsYield(TimeUtil.MillisecondsToSeconds(inUnmuteRemainTimeMs + NETWORK_BUFFER_TIME_MS));

            if (_stateType != StateType.Disposed && _groupChannel != null)
            {
                bool waitingForResponse = true;
                void OnMuteInfoHandler(bool inIsMuted, string inDescription, long inStartAt, long inEndAt, long inRemainingDuration, SbError inError)
                {
                    waitingForResponse = false;
                    if (inError == null && inIsMuted == false && _groupChannel.MyMutedState == SbMutedState.Muted)
                    {
                        _groupChannel.SetMyMutedState(SbMutedState.Unmuted);
                        chatMainContextRef.CollectionManager.OnUserUnmuted(_groupChannel, chatMainContextRef.CurrentUserRef);
                    }
                }

                _groupChannel.GetMyMutedInfo(OnMuteInfoHandler);
                while (waitingForResponse)
                    yield return null;
            }

            _autoUnmuteJob = null;
        }
    }
}