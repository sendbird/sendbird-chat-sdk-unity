// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections;

namespace Sendbird.Chat
{
    internal class CoroutineJob
    {
        private IEnumerator _enumerator = null;
        private CoroutineJob _waitForCoroutine = null;
    
        public CoroutineJob(IEnumerator inEnumerator)
        {
            _enumerator = inEnumerator;
        }
    
        public void MoveNext()
        {
            if (_enumerator == null)
                return;

            if (_enumerator.Current is IEnumerator currentEnumerator)
            {
                if (currentEnumerator.MoveNext())
                    return;
            }
    
            if (_enumerator.Current is CoroutineJob job)
                _waitForCoroutine = job;
    
            if (_waitForCoroutine != null && _waitForCoroutine.IsFinished())
                _waitForCoroutine = null;
    
            if (_waitForCoroutine != null)
                return;
    
            if (_enumerator.MoveNext() == false)
                _enumerator = null;
        }
    
        public void Stop()
        {
            if (_enumerator != null && _enumerator.Current is CoroutineJob job)
                job.Stop();
    
            _enumerator = null;
        }
    
        public bool IsFinished()
        {
            return _enumerator == null;
        }
    }
}