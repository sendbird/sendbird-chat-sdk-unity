// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a group channel update params.
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelUpdateParams
    {
        /// <summary>
        /// The access code for public group channel. The access code setting is only valid for PublicGroupChannels. Once the access code is set, users have to accept an invitation or join the public SbGroupChannel with the access code to be a member of the channel. Refer to SbGroupChannel.Join and SbGroupChannel.AcceptInvitation. To delete the existing access code, pass an empty string as to this and call SbGroupChannel.UpdateChannel.
        /// </summary>
        /// @since 4.0.0
        public string AccessCode { get => _accessCode; set => _accessCode = value; }

        /// <summary>
        /// The cover image of the channel. Defaults to null. If CoverUrl was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public SbFileInfo CoverImage { get => _coverImage; set => SetCoverImageAndNullUrl(value); }

        /// <summary>
        /// The cover image's url of the channel. Defaults to null. If CoverImage was set after, this will be reset to null.
        /// </summary>
        /// @since 4.0.0
        public string CoverUrl { get => _coverUrl; set => SetCoverUrlAndNullImage(value); }

        /// <summary>
        /// The custom type of the channel.
        /// </summary>
        /// @since 4.0.0
        public string CustomType { get => _customType; set => _customType = value; }

        /// <summary>
        /// The data of the channel.
        /// </summary>
        /// @since 4.0.0
        public string Data { get => _data; set => _data = value; }

        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public bool IsDiscoverable { get => _isDiscoverable; set => _isDiscoverable = value; }

        /// <summary>
        /// The distinct mode of the channel. If SbGroupChannel.IsSuper is true, then this must be set to false.
        /// </summary>
        /// @since 4.0.0
        public bool IsDistinct { get => _isDistinct; set => _isDistinct = value; }

        /// <summary>
        /// The public mode of the channel. If set to true, then IsDistinct must be false.
        /// </summary>
        /// @since 4.0.0
        public bool IsPublic { get => _isPublic; set => _isPublic = value; }

        /// <summary>
        /// The message survival seconds of the channel.
        /// </summary>
        /// @since 4.0.0
        public int MessageSurvivalSeconds { get => _messageSurvivalSeconds; set => _messageSurvivalSeconds = value; }

        /// <summary>
        /// The name of the channel.
        /// </summary>
        /// @since 4.0.0
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// The operator user ids of the channel.
        /// </summary>
        /// @since 4.0.0
        public List<string> OperatorUserIds { get => _operatorUserIds; set => _operatorUserIds = value; }
    }
}