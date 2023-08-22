// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Object representing a file.
    /// </summary>
    /// @since 4.0.0
    public partial class SbFileMessage : SbBaseMessage
    {
        /// <summary>
        /// The file URL. If the file encryption feature is enabled, this will have SendbirdChat.EKey combined with the plainUrl so the file can be accessed. For caching the file, it is recommended to use plainUrl as the key of the file cache.
        /// </summary>
        /// @since 4.0.0
        public string Url => _requireAuth ? $"{_filePlainUrl}?auth={chatMainContextRef.EKey}" : _filePlainUrl;

        /// <summary>
        /// The plain file URL, which does not contain SendbirdChat.EKey as a parameter. If the file encryption feature is enabled, accessing this plainUrl will be denied.
        /// </summary>
        /// @since 4.0.0
        public string PlainUrl => _filePlainUrl;

        /// <summary>
        /// Represents the name of the file.
        /// </summary>
        /// @since 4.0.0
        public string Name => _fileName;

        /// <summary>
        /// Represents the size of the file.
        /// </summary>
        /// @since 4.0.0
        public int Size => _fileSize;

        /// <summary>
        /// Represents the type of the file. MIME preferred.
        /// </summary>
        /// @since 4.0.0
        public string Type => _mimeType;

        /// <summary>
        /// Represents the thumbnail information of image file. To make thumbnail of image when you send it.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbThumbnail> Thumbnails => _thumbnails;
    }
}