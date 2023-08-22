// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbFileMessageCreateParams : SbBaseMessageCreateParams
    {
        private SbFileInfo _file = null;
        private string _fileUrl = null;
        private string _fileName = null;
        private int _fileSize = 0;
        private string _mimeType = null;
        private List<SbThumbnailSize> _thumbnailSizes;

        private SbFileMessageCreateParams(SbFileMessageCreateParams inOtherParams) : base(inOtherParams)
        {
            if (inOtherParams != null)
            {
                _file = inOtherParams._file;
                _fileUrl = inOtherParams._fileUrl;
                _fileName = inOtherParams._fileName;
                _fileSize = inOtherParams._fileSize;
                _mimeType = inOtherParams._mimeType;
                if (inOtherParams._thumbnailSizes != null && 0 < inOtherParams._thumbnailSizes.Count)
                {
                    _thumbnailSizes = new List<SbThumbnailSize>(inOtherParams._thumbnailSizes);
                }
            }
        }

        internal SbFileMessageCreateParams(SbFileMessage inFileMessage) : base(inFileMessage)
        {
            if (inFileMessage != null)
            {
                _fileUrl = inFileMessage.PlainUrl;
                _fileName = inFileMessage.Name;
                _fileSize = inFileMessage.Size;
                _mimeType = inFileMessage.Type;
                if (inFileMessage.Thumbnails != null && 0 < inFileMessage.Thumbnails.Count)
                    _thumbnailSizes = inFileMessage.Thumbnails.Select(inThumbnail => new SbThumbnailSize(inThumbnail.MaxWidth, inThumbnail.MaxHeight)).ToList();
            }
        }

        internal override SbBaseMessageCreateParams Clone()
        {
            return new SbFileMessageCreateParams(this);
        }

        private void SetFileAndNullUrl(SbFileInfo inFileInfo)
        {
            _file = inFileInfo;
            if (_file != null)
                _fileUrl = null;
        }

        internal void SetFileUrlAndNullFile(string inFileUrl)
        {
            _fileUrl = inFileUrl;
            if (string.IsNullOrEmpty(_fileUrl) == false)
                _file = null;
        }

        private string GetFileName()
        {
            if (string.IsNullOrEmpty(_fileName) == false)
                return _fileName;

            if (_file != null)
                return _file.GetName();

            return "image.jpg";
        }
    }
}