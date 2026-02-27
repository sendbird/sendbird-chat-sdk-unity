//
//  Copyright (c) 2026 Sendbird, Inc.
//

using System;
using System.IO;

namespace Sendbird.Chat
{
    /// <summary>
    /// A TextReader that reads from a string starting at a specified offset,
    /// avoiding the need to allocate a substring copy via String.Substring().
    /// </summary>
    internal sealed class OffsetStringReader : TextReader
    {
        private string _source;
        private readonly int _length;
        private int _position;

        internal OffsetStringReader(string source, int offset)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _position = Math.Min(offset, source.Length);
            _length = source.Length;
        }

        public override int Read()
        {
            if (_position >= _length)
                return -1;

            return _source[_position++];
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            int available = _length - _position;
            if (available <= 0)
                return 0;

            int toRead = Math.Min(count, available);
            _source.CopyTo(_position, buffer, index, toRead);
            _position += toRead;
            return toRead;
        }

        public override int Peek()
        {
            if (_position >= _length)
                return -1;

            return _source[_position];
        }

        protected override void Dispose(bool disposing)
        {
            _source = null;
            base.Dispose(disposing);
        }
    }
}
