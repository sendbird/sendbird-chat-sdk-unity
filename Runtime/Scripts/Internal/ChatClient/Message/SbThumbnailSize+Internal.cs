// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbThumbnailSize
    {
        private int _maxWidth = 0;
        private int _maxHeight = 0;

        internal SbThumbnailSize(SbThumbnailSize inThumbnailSize)
        {
            _maxWidth = inThumbnailSize._maxWidth;
            _maxHeight = inThumbnailSize._maxHeight;
        }
    }
}