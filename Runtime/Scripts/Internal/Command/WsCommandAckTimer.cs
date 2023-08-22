// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections;

namespace Sendbird.Chat
{
    internal class WsCommandAckTimer
    {
        private WsSendCommandAbstract.AckHandler _completionHandler;
        private readonly long _waitingTime;
        private CoroutineJob _coroutineJob;

        internal WsCommandAckTimer(WsSendCommandAbstract.AckHandler inCompletionHandler, long inWaitingTime = NetworkConfig.DEFAULT_WEBSOCKET_RESPONSE_TIMEOUT_DURATION)
        {
            _completionHandler = inCompletionHandler;
            _waitingTime = inWaitingTime;
            _coroutineJob = null;
        }

        internal void Start()
        {
            _coroutineJob = CoroutineManager.Instance.StartCoroutine(WaitingForAckCoroutine());
        }

        internal void StopAndInvokeCompletionHandler(WsReceiveCommandAbstract inWsReceiveCommandAbstract, SbError inStopReason = null)
        {
            if (_coroutineJob != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutineJob);
                _coroutineJob = null;

                if (_completionHandler != null)
                {
                    SbError error = inStopReason;
                    if (inWsReceiveCommandAbstract == null && error == null)
                    {
                        error = new SbError(SbErrorCode.AckTimeout);
                    }

                    _completionHandler.Invoke(inWsReceiveCommandAbstract, error);
                    _completionHandler = null;
                }
            }
        }

        private IEnumerator WaitingForAckCoroutine()
        {
            yield return new WaitForSecondsYield(_waitingTime);

            SbError error = new SbError(SbErrorCode.AckTimeout);
            _completionHandler?.Invoke(null, error);

            _completionHandler = null;
            _coroutineJob = null;
        }
    }
}