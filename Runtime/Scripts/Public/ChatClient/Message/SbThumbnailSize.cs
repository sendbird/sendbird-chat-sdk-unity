// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents image thumbnail size. Currently this is valid only for image files.
    /// </summary>
    /// @since 4.0.0
    public partial class SbThumbnailSize
    {
        /// <summary>
        /// The maximum width of thumbnail to be generated.
        /// </summary>
        /// @since 4.0.0
        public int MaxWidth { get => _maxWidth; set => _maxWidth = Math.Max(value, 0); }

        /// <summary>
        /// The maximum height of thumbnail to be generated.
        /// </summary>
        /// @since 4.0.0
        public int MaxHeight { get => _maxHeight; set => _maxHeight = Math.Max(value, 0); }
        
        /// @since 4.0.0
        public SbThumbnailSize(int inMaxWidth, int inMaxHeight)
        {
            MaxWidth = inMaxWidth;
            MaxHeight = inMaxHeight;
        }
    }
}