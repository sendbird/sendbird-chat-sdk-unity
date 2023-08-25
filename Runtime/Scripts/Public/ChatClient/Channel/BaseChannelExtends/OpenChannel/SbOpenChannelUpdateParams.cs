// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a open channel params.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOpenChannelUpdateParams
    {
        /// <summary>
        /// The name of the channel.
        /// </summary>
        /// @since 4.0.0
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// The cover image's url of the channel. Defaults to null. If CoverImage was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public string CoverUrl { get => _coverUrl; set => SetCoverUrlAndNullImage(value); }

        /// <summary>
        /// The cover image of the channel. Defaults to null. If CoverUrl was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public SbFileInfo CoverImage { get => _coverImage; set => SetCoverImageAndNullUrl(value); }

        /// <summary>
        /// The data of the channel.
        /// </summary>
        /// @since 4.0.0
        public string Data { get => _data; set => _data = value; }

        /// <summary>
        /// The custom type of the channel.
        /// </summary>
        /// @since 4.0.0
        public string CustomType { get => _customType; set => _customType = value; }

        /// <summary>
        /// The operator user ids of the channel. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public List<string> OperatorUserIds { get => _operatorUserIds; set => _operatorUserIds = value; }
    }
}