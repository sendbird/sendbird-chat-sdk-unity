namespace Sendbird.Chat
{
    internal static class BufferPool
    {
        private const int MAX_POOLABLE_SIZE = 256 * 1024;
        private static readonly SimpleArrayPool<byte> POOL = new SimpleArrayPool<byte>();

        internal static byte[] Rent(int inMinimumLength)
        {
            if (inMinimumLength <= MAX_POOLABLE_SIZE)
                return POOL.Rent(inMinimumLength);
            return new byte[inMinimumLength];
        }

        internal static void Return(byte[] inBuffer)
        {
            if (inBuffer == null) return;
            if (inBuffer.Length <= MAX_POOLABLE_SIZE)
                POOL.Return(inBuffer);
        }
    }
}
