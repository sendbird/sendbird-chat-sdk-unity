// Copyright (c) 2026 Sendbird, Inc.

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class JsonArrayPool : IArrayPool<char>
    {
        internal static readonly JsonArrayPool EVENT_INSTANCE = new JsonArrayPool();
        private static readonly SimpleArrayPool<char> POOL = new SimpleArrayPool<char>();
        private JsonArrayPool() { }

        public char[] Rent(int minimumLength)
            => POOL.Rent(minimumLength);

        public void Return(char[] array)
        {
            if (array != null)
                POOL.Return(array);
        }
    }
}
