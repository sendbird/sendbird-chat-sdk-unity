// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbAppInfo
    {
        /// <summary>
        /// This is the latest hash value for Emoji. Default value is empty string.
        /// </summary>
        /// @since 4.0.0
        public string EmojiHash => _emojiHash;

        /// <summary>
        /// This is the upload able file size limit. (The unit is bytes.)
        /// </summary>
        /// @since 4.0.0
        public long UploadSizeLimit => _uploadSizeLimit;

        /// <summary>
        /// This is the state of using the reaction feature.
        /// </summary>
        /// @since 4.0.0
        public bool UseReaction => _useReaction;

        /// <summary>
        /// List of all premium features that application is using.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> PremiumFeatureList => _premiumFeatureList;

        /// <summary>
        /// List of all attributes that the application is using.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> AttributesInUse => _attributesInUse;
    }
}