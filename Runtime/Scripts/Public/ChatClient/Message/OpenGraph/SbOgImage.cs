// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a OGImage of OgMetaData. For Specifications, see https://ogp.me/.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOgImage
    {
        /// <summary>
        /// A description of what is in the image (not a caption).
        /// </summary>
        /// @since 4.0.0
        public string Alt => _alt;

        /// <summary>
        /// The number of pixels high.
        /// </summary>
        /// @since 4.0.0
        public int Height => _height;

        /// <summary>
        /// The number of pixels wide.
        /// </summary>
        /// @since 4.0.0
        public int Width => _width;

        /// <summary>
        /// An alternate url to use if the webpage requires HTTPS.
        /// </summary>
        /// @since 4.0.0
        public string SecureUrl => _secureUrl;

        /// <summary>
        /// A MIME type for this image.
        /// </summary>
        /// @since 4.0.0
        public string Type => _type;

        /// <summary>
        /// An image URL which represents the object within the Open Graph.
        /// </summary>
        /// @since 4.0.0
        public string Url => _url;
    }
}