// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public partial class SbUser
    {
        /// <summary>
        /// User ID. This has to be unique.
        /// </summary>
        /// @since 4.0.0
        public string UserId => _userId;

        /// <summary>
        /// User nickname.
        /// </summary>
        /// @since 4.0.0
        public string Nickname => _nickname;

        /// <summary>
        /// The profile image URL. If the file encryption feature is enabled, this will have SendbirdChat.EKey combined with the PlainProfileImageUrl so the file can be accessed. For caching the file, it is recommended to use PlainProfileImageUrl as the key of the file cache.
        /// </summary>
        /// @since 4.0.0
        public string ProfileUrl => RequireAuthForProfileImage ? $"{_plainProfileImageUrl}?auth={chatMainContextRef.EKey}" : _plainProfileImageUrl;

        /// <summary>
        /// Original profile image url.
        /// </summary>
        /// @since 4.0.0
        public string OriginalProfileUrl => _originalProfileUrl;

        /// <summary>
        /// The plain profile image URL, which does not contain SendbirdChat.EKey as a parameter. If the file encryption feature is enabled, accessing this PlainProfileImageUrl will be denied.
        /// </summary>
        /// @since 4.0.0
        public string PlainProfileImageUrl => _plainProfileImageUrl;

        /// <summary>
        /// User’s preferred language. Used for translating messages.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> PreferredLanguages => _preferredLanguages;

        /// <summary>
        /// All meta data of the user.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyDictionary<string, string> MetaData => _metaData;

        /// <summary>
        /// User connection status. This is defined in SbUserConnectionStatus.
        /// </summary>
        /// @since 4.0.0
        public SbUserConnectionStatus ConnectionStatus => _connectionStatus;

        /// <summary>
        /// Represents the user is activated. This property is changed by the Platform API
        /// </summary>
        /// @since 4.0.0
        public bool IsActive => _isActive;

        /// <summary>
        /// The latest time when the user became offline.
        /// </summary>
        /// @since 4.0.0
        public long LastSeenAt => _lastSeenAt;

        /// <summary>
        /// Discovery key for friend
        /// </summary>
        /// @since 4.0.0
        public string FriendDiscoveryKey => _friendDiscoveryKey;

        /// <summary>
        /// User name for friend
        /// </summary>
        /// @since 4.0.0
        public string FriendName => _friendName;

        /// <summary>
        /// Creates the meta data for the current user.
        /// </summary>
        /// <param name="inMetaData">The meta data to be set.</param>
        /// <param name="inCompletionHandler">The handler block to execute. metaData is the meta data which are set on Sendbird server.</param>
        /// @since 4.0.0
        public void CreateMetaData(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            CreateMetaDataInternal(inMetaData, inCompletionHandler);
        }

        /// <summary>
        /// Gets meta data.
        /// </summary>
        /// <param name="inKey"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public string GetMetaData(string inKey)
        {
            return GetMetaDataInternal(inKey);
        }

        /// <summary>
        /// Updates the meta data for the current user.
        /// </summary>
        /// <param name="inMetaData"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateMetaData(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            UpdateMetaDataInternal(inMetaData, inCompletionHandler);
        }

        /// <summary>
        /// Deletes meta data with key for the current user.
        /// </summary>
        /// <param name="inKey"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteMetaData(string inKey, SbErrorHandler inCompletionHandler)
        {
            DeleteMetaDataInternal(inKey, inCompletionHandler);
        }

        /// <summary>
        /// Deletes all meta data for the current user.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void DeleteAllMetaData(SbErrorHandler inCompletionHandler)
        {
            DeleteAllMetaDataInternal(inCompletionHandler);
        }
    }
}