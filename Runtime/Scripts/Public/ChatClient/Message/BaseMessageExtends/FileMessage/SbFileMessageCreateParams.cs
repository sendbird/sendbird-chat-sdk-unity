// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a file message params.
    /// </summary>
    /// @since 4.0.0
    public partial class SbFileMessageCreateParams : SbBaseMessageCreateParams
    {
        /// <summary>
        /// The SbFileInfo object of the message. Defaults to null. If FileUrl was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public SbFileInfo File { get => _file; set => SetFileAndNullUrl(value); }

        /// <summary>
        /// The file's url of the message. Defaults to null. If File was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public string FileUrl { get => _fileUrl; set => SetFileUrlAndNullFile(value); }

        /// <summary>
        /// The file's name of the messages. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public string FileName { get => GetFileName(); set => _fileName = value; }

        /// <summary>
        /// The file's size of the messages.
        /// </summary>
        /// @since 4.0.0
        public int FileSize { get => _fileSize; set => _fileSize = value; }

        /// <summary>
        /// The file's mime type of the messages. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public string MimeType { get => _mimeType; set => _mimeType = value; }

        /// <summary>
        /// The file's thumbnail sizes of the messages. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public List<SbThumbnailSize> ThumbnailSizes { get => _thumbnailSizes; set => _thumbnailSizes = value; }
        
        /// @since 4.0.0
        public SbFileMessageCreateParams(string inFileUrl)
        {
            FileUrl = inFileUrl;
        }

        /// @since 4.0.0
        public SbFileMessageCreateParams(SbFileInfo inFile)
        {
            File = inFile;
        }
    }
}