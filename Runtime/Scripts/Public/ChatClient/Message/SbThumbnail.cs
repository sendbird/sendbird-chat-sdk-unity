// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents image thumbnail. Currently this is valid only for image files.
    /// </summary>
    /// @since 4.0.0
    public partial class SbThumbnail
    {
        /// <summary>
        /// Represents the maximum width of thumbnail.
        /// </summary>
        /// @since 4.0.0
        public int MaxWidth => _maxWidth;

        /// <summary>
        /// Represents the maximum height of thumbnail.
        /// </summary>
        /// @since 4.0.0
        public int MaxHeight => _maxHeight;

        /// <summary>
        /// The actual width of thumbnail.
        /// </summary>
        /// @since 4.0.0
        public int RealWidth => _realWidth;

        /// <summary>
        /// The actual height of thumbnail.
        /// </summary>
        /// @since 4.0.0
        public int RealHeight => _realHeight;

        /// <summary>
        /// The URL of the generated thumbnail, which does not contain SendbirdChat.EKey as a parameter. If the file encryption feature is enabled, accessing this plainUrl will be denied.
        /// </summary>
        /// @since 4.0.0
        public string PlainUrl => _plainUrl;

        /// <summary>
        /// The URL of the generated thumbnail. If the file encryption feature is enabled, this will have SendbirdChat.EKey combined with the plainUrl so the thumbnail can be accessed. For caching the thumbnail, it is recommended to use plainUrl as the key of the file cache.
        /// </summary>
        /// @since 4.0.0
        public string Url => _requireAuth ? $"{_plainUrl}?auth={_chatMainContextRef.EKey}" : _plainUrl;
    }
}