// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbFileMessage : SbBaseMessage
    {
        private readonly string _filePlainUrl = null;
        private readonly string _fileName = null;
        private readonly int _fileSize = 0;
        private readonly string _mimeType = null;
        private readonly List<SbThumbnail> _thumbnails = new List<SbThumbnail>();
        private readonly bool _requireAuth = false;

        private SbFileMessage(SbFileMessage inOtherMessage) : base(inOtherMessage)
        {
            if (inOtherMessage != null)
            {
                _filePlainUrl = inOtherMessage._filePlainUrl;
                _fileName = inOtherMessage._fileName;
                _fileSize = inOtherMessage._fileSize;
                _mimeType = inOtherMessage._mimeType;
                _requireAuth = inOtherMessage._requireAuth;

                if (inOtherMessage._thumbnails != null && 0 < inOtherMessage._thumbnails.Count)
                {
                    _thumbnails.Clear();
                    _thumbnails.AddRange(inOtherMessage._thumbnails);
                }
            }
        }

        private SbFileMessage(SbFileMessageCreateParams inFileMessageCreateParams, SendbirdChatMainContext inChatMainContext, 
                              SbSender inSender, SbBaseChannel inBaseChannel, string inRequestId = null)
            : base(inFileMessageCreateParams, inChatMainContext, inSender, inBaseChannel, inRequestId)
        {
            if (inFileMessageCreateParams != null)
            {
                _filePlainUrl = inFileMessageCreateParams.FileUrl;
                _fileName = inFileMessageCreateParams.FileName;
                _fileSize = inFileMessageCreateParams.FileSize;
                _mimeType = inFileMessageCreateParams.MimeType;
            }
        }

        internal SbFileMessage(FileMessageDto inFileMessageDto, SendbirdChatMainContext inChatMainContext)
            : base(inFileMessageDto, inChatMainContext)
        {
            if (inFileMessageDto != null)
            {
                _filePlainUrl = inFileMessageDto.FileUrl;
                _fileName = inFileMessageDto.FileName;
                _fileSize = inFileMessageDto.FileSize;
                _mimeType = inFileMessageDto.MimeType;
                _requireAuth = inFileMessageDto.RequireAuth;

                if (inFileMessageDto.thumbnailDtos != null && 0 < inFileMessageDto.thumbnailDtos.Count)
                {
                    _thumbnails.Clear();
                    foreach (ThumbnailDto thumbnailDto in inFileMessageDto.thumbnailDtos)
                    {
                        _thumbnails.Add(new SbThumbnail(thumbnailDto, _requireAuth, chatMainContextRef));
                    }
                }
            }
        }

        protected override SbBaseMessage Clone()
        {
            return new SbFileMessage(this);
        }

        internal static SbFileMessage CreateMessage(SbFileMessageCreateParams inMessageCreateParams, SendbirdChatMainContext inChatMainContext,
                                                    SbSender inSender, SbBaseChannel inBaseChannel, bool inIsOperatorMessage, string inRequestId = null)
        {
            SbFileMessage message = new SbFileMessage(inMessageCreateParams, inChatMainContext, inSender, inBaseChannel, inRequestId)
            {
                isOperatorMessage = inIsOperatorMessage,
            };
            return message;
        }

        internal void ClearAndCopyThumbnails(IEnumerable<SbThumbnail> inThumbnails)
        {
            _thumbnails.Clear();
            if (inThumbnails != null)
                _thumbnails.AddRange(inThumbnails);
        }
    }
}