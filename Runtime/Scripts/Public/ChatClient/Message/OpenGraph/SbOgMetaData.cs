// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a SbOgMetaData of a url. For Specifications, see https://ogp.me/. Currently we only support images.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOgMetaData
    {
        /// <summary>
        /// A one to two sentence description of the object. ex: "Sean Connery found fame and fortune as the suave, sophisticated British agent, James Bond."
        /// </summary>
        /// @since 4.0.0
        public string Description => _description;

        /// <summary>
        /// An ogImage object that contains information about the image that this Open Graph points to.
        /// </summary>
        /// @since 4.0.0
        public SbOgImage DefaultImage => _defaultImage;

        /// <summary>
        /// The title of the object as it should appear within the graph. ex: "The Rock".
        /// </summary>
        /// @since 4.0.0
        public string Title => _title;

        /// <summary>
        /// The canonical URL of the object that can be used as its permanent ID in the graph. ex: "http://www.imdb.com/title/tt0117500/"
        /// </summary>
        /// @since 4.0.0
        public string Url => _url;
    }
}