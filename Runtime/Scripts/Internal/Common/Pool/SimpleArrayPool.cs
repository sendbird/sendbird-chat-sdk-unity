// Copyright (c) 2026 Sendbird, Inc.

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal sealed class SimpleArrayPool<T>
    {
        private static readonly int[] DEFAULT_BUCKET_SIZES = { 256, 1024, 4096, 16384, 65536, 262144, 1048576 };
        private const int MAX_ARRAYS_PER_BUCKET = 4;

        private readonly int[] _bucketSizes;
        private readonly Stack<T[]>[] _buckets;
        private readonly object _lock = new object();

        internal SimpleArrayPool() : this(DEFAULT_BUCKET_SIZES) { }

        internal SimpleArrayPool(int[] inBucketSizes)
        {
            _bucketSizes = inBucketSizes;
            _buckets = new Stack<T[]>[_bucketSizes.Length];
            for (int i = 0; i < _buckets.Length; i++)
                _buckets[i] = new Stack<T[]>();
        }

        internal T[] Rent(int inMinimumLength)
        {
            int bucketIndex = FindBucketIndex(inMinimumLength);
            if (bucketIndex >= 0)
            {
                lock (_lock)
                {
                    if (_buckets[bucketIndex].Count > 0)
                        return _buckets[bucketIndex].Pop();
                }

                return new T[_bucketSizes[bucketIndex]];
            }

            return new T[inMinimumLength];
        }

        internal void Return(T[] inArray)
        {
            if (inArray == null)
                return;

            int bucketIndex = FindBucketIndex(inArray.Length);
            if (bucketIndex < 0 || inArray.Length != _bucketSizes[bucketIndex])
                return;

            lock (_lock)
            {
                if (_buckets[bucketIndex].Count < MAX_ARRAYS_PER_BUCKET)
                    _buckets[bucketIndex].Push(inArray);
            }
        }

        private int FindBucketIndex(int inLength)
        {
            for (int i = 0; i < _bucketSizes.Length; i++)
            {
                if (_bucketSizes[i] >= inLength)
                    return i;
            }

            return -1;
        }
    }
}
