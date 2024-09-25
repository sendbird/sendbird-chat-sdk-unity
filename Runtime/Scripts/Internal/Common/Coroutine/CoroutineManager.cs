// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class CoroutineManager : SingletonAbstract<CoroutineManager>
    {
        private CoroutineManager() { }
        private readonly List<CoroutineJob> _coroutineJobs = new List<CoroutineJob>();
        private readonly List<CoroutineJob> _finishedCoroutineJobs = new List<CoroutineJob>();
        private readonly List<CoroutineJob> _startedCoroutineJobs = new List<CoroutineJob>();

        internal CoroutineJob StartCoroutine(IEnumerator inEnumerator)
        {
            CoroutineJob coroutine = new CoroutineJob(inEnumerator);
            _startedCoroutineJobs.Add(coroutine);
            SdkManager.Instance.StartAsyncProcessIfNotRunning();
            return coroutine;
        }

        internal void StopCoroutine(CoroutineJob inCoroutineJob)
        {
            _finishedCoroutineJobs.AddIfNotContains(inCoroutineJob);
        }

        internal void Update()
        {
            if (0 < _startedCoroutineJobs.Count)
            {
                _coroutineJobs.AddRange(_startedCoroutineJobs);
                _startedCoroutineJobs.Clear();
            }
            
            foreach (CoroutineJob finishedCoroutineJob in _finishedCoroutineJobs)
            {
                _coroutineJobs.RemoveIfContains(finishedCoroutineJob);
            }
            _finishedCoroutineJobs.Clear();
            
            foreach (CoroutineJob coroutineJob in _coroutineJobs)
            {
                coroutineJob.MoveNext();
                if (coroutineJob.IsFinished())
                {
                    _finishedCoroutineJobs.Add(coroutineJob);
                }
            }
        }

        //public void
        internal CoroutineJob CallOnNextFrame(Action inAction)
        {
            return StartCoroutine(CallOnNextFrameCoroutine(inAction));
        }

        private IEnumerator CallOnNextFrameCoroutine(Action inAction)
        {
            if (inAction == null)
                yield break;

            yield return null;
            inAction.Invoke();
        }
    }
}