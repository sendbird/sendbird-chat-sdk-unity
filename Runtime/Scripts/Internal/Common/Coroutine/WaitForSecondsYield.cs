// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Collections;

namespace Sendbird.Chat
{
    internal class WaitForSecondsYield : IEnumerator
    {
        private long _endUnixTimeMilliseconds = 0;
        private readonly long _waitMilliseconds = 0;
        
        internal WaitForSecondsYield(float inSeconds)
        {
            _waitMilliseconds = (long)(inSeconds * 1000.0f);
            _endUnixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + _waitMilliseconds;
        }
        
        public bool MoveNext()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() < _endUnixTimeMilliseconds;
        }

        public void Reset()
        {
            _endUnixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + _waitMilliseconds;
        }

        public object Current => null;
    }
}