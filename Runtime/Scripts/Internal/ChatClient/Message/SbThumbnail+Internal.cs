// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbThumbnail
    {
        private readonly int _maxWidth = 0;
        private readonly int _maxHeight = 0;
        private readonly int _realWidth = 0;
        private readonly int _realHeight = 0;
        private readonly string _plainUrl;
        private readonly bool _requireAuth = false;
        private readonly SendbirdChatMainContext _chatMainContextRef;

        internal SbThumbnail(SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
        }

        internal SbThumbnail(ThumbnailDto inThumbnailDto, bool inRequireAuth, SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
            _requireAuth = inRequireAuth;
            if (inThumbnailDto != null)
            {
                _maxWidth = inThumbnailDto.width;
                _maxHeight = inThumbnailDto.height;
                _realWidth = inThumbnailDto.realWidth;
                _realHeight = inThumbnailDto.realHeight;
                _plainUrl = inThumbnailDto.url;
            }
        }
    }
}