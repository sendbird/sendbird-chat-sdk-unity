// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbUserUpdateParams
    {
        /// <summary>
        /// User’s nickname
        /// </summary>
        /// @since 4.0.0
        public string NickName { get => _nickName; set => _nickName = value; }

        /// <summary>
        /// User’s profile image url
        /// </summary>
        /// @since 4.0.0
        public string ProfileImageUrl { get => _profileImageUrl; set => SetProfileImageUrl(value); }

        /// <summary>
        /// User’s profile image file info
        /// </summary>
        /// @since 4.0.0
        public SbFileInfo ProfileImageFile { get => _profileImageFile; set => SetProfileImageFile(value); }
    }
}