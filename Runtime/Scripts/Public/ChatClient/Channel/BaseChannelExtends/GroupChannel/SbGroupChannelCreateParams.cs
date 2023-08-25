// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// An object contains set of options to create a group channel
    /// </summary>
    /// @since 4.0.0
    public partial class SbGroupChannelCreateParams
    {
        /// <summary>
        /// A string that allows access to the public group channel. ONLY use for public group channel.
        /// </summary>
        /// @since 4.0.0
        public string AccessCode { get => _accessCode; set => _accessCode = value; }

        /// <summary>
        /// Represents the channel is to be updated.
        /// </summary>
        /// @since 4.0.0
        public string ChannelUrl { get => _channelUrl; set => _channelUrl = value; }

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
        /// The broadcast mode of the channel. If set to true, then IsSuper will be true.
        /// </summary>
        /// @since 4.0.0
        public bool IsBroadcast { get => _isBroadcast; set => _isBroadcast = value; }

        /// <summary>
        /// Whether the channel is a discoverable channel for public group channel. It is valid only when IsPublic is set to true. If set to false, this channel will not appear in the result of SbPublicGroupChannelListQuery.
        /// </summary>
        /// @since 4.0.0
        public bool IsDiscoverable { get => _isDiscoverable; set => _isDiscoverable = value; }

        /// <summary>
        /// The distinct mode of the channel. If IsSuper is true, then this must be set to false.
        /// </summary>
        /// @since 4.0.0
        public bool IsDistinct { get => _isDistinct; set => _isDistinct = value; }

        /// <summary>
        /// The ephemeral mode of the channel.
        /// </summary>
        /// @since 4.0.0
        public bool IsEphemeral { get => _isEphemeral; set => _isEphemeral = value; }

        /// <summary>
        /// The exclusive mode of the channel. If set to true, then IsSuper and isBroadcast will both be true.
        /// </summary>
        /// @since 4.0.0
        public bool IsExclusive { get => _isExclusive; set => _isExclusive = value; }

        /// <summary>
        /// The public mode of the channel. If set to true, then IsDistinct must be false.
        /// </summary>
        /// @since 4.0.0
        public bool IsPublic { get => _isPublic; set => _isPublic = value; }

        /// <summary>
        /// The strict mode of the channel. When true, the channel creation will fail if any of the users do not exist. When false, the channel creation will succeed even if all the users do not exist. The default value is false.
        /// </summary>
        /// @since 4.0.0
        public bool IsStrict { get => _isStrict; set => _isStrict = value; }

        /// <summary>
        /// The super mode of the channel. If set to true, then IsDistinct must be false.
        /// </summary>
        /// @since 4.0.0
        public bool IsSuper { get => _isSuper; set => _isSuper = value; }

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
        /// The operator user ids of the channel. Defaults to null.
        /// </summary>
        /// @since 4.0.0
        public List<string> OperatorUserIds { get => _operatorUserIds; set => _operatorUserIds = value; }

        /// <summary>
        /// The user ids of the users of the channel. Defaults to an empty list.
        /// </summary>
        /// @since 4.0.0
        public List<string> UserIds { get => _userIds; set => _userIds = value; }
    }
}