// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbUser
    {
        private readonly string _userId;
        private string _nickname;
        private string _profileUrl;
        private string _plainProfileImageUrl;
        private List<string> _preferredLanguages;
        private Dictionary<string, string> _metaData;
        private SbUserConnectionStatus _connectionStatus;
        private bool _isActive;
        private long _lastSeenAt;
        private string _friendDiscoveryKey;
        private string _friendName;
        private string _originalProfileUrl;
        internal bool RequireAuthForProfileImage { get; private set; }
        private protected readonly SendbirdChatMainContext chatMainContextRef = null;

        internal SbUser(string inUserId)
        {
            _userId = inUserId;
        }

        internal SbUser(SbUser inUser, SendbirdChatMainContext inChatMainContext)
        {
            if (inUser == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbUser::SbUser() User is null.");
                return;
            }

            chatMainContextRef = inChatMainContext;
            _userId = inUser._userId;
            _nickname = inUser._nickname;
            _plainProfileImageUrl = inUser._plainProfileImageUrl;
            _originalProfileUrl = inUser._originalProfileUrl;
            _isActive = inUser._isActive;
            _lastSeenAt = inUser._lastSeenAt;
            _friendName = inUser._friendName;
            _friendDiscoveryKey = inUser._friendDiscoveryKey;
            _preferredLanguages = new List<string>(inUser._preferredLanguages);
            _metaData = new Dictionary<string, string>(inUser._metaData);
            _userId = inUser._userId;
            _connectionStatus = inUser._connectionStatus;
            RequireAuthForProfileImage = inUser.RequireAuthForProfileImage;
        }

        internal SbUser(UserDto inUserDto, SendbirdChatMainContext inChatMainContext)
        {
            if (inUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbUser::SbUser() UserDto is null.");
                return;
            }

            chatMainContextRef = inChatMainContext;
            _userId = inUserDto.UserId;
            _nickname = !string.IsNullOrEmpty(inUserDto.nickName) ? inUserDto.nickName : inUserDto.name ?? string.Empty;
            _plainProfileImageUrl = !string.IsNullOrEmpty(inUserDto.profileUrl) ? inUserDto.profileUrl : inUserDto.imageUrl ?? string.Empty;
            _originalProfileUrl = ConvertToOriginalProfileUrl(_plainProfileImageUrl);

            _isActive = inUserDto.isActive ?? true;
            _lastSeenAt = inUserDto.lastSeenAt ?? 0;
            _friendName = inUserDto.friendName ?? string.Empty;
            _friendDiscoveryKey = inUserDto.friendDiscoveryKey ?? string.Empty;
            _preferredLanguages = inUserDto.preferredLanguages;
            _metaData = inUserDto.metaData;

            if (inUserDto.isOnline == null)
            {
                _connectionStatus = SbUserConnectionStatus.NonAvailable;
            }
            else
            {
                _connectionStatus = inUserDto.isOnline == true ? SbUserConnectionStatus.Online : SbUserConnectionStatus.Offline;
            }

            RequireAuthForProfileImage = inUserDto.requireAuthForProfileImage ?? false;
        }

        internal void UpdateFromDto(UserDto inUserDto)
        {
            if (inUserDto == null)
            {
                Logger.Warning(Logger.CategoryType.User, "SbUser::UpdateFromDto() UserDto is null.");
                return;
            }

            if (inUserDto.nickName != null || inUserDto.name != null) { _nickname = inUserDto.nickName ?? inUserDto.name; }

            if (inUserDto.profileUrl != null || inUserDto.imageUrl != null) { _plainProfileImageUrl = inUserDto.profileUrl ?? inUserDto.imageUrl; }

            _originalProfileUrl = ConvertToOriginalProfileUrl(_plainProfileImageUrl);

            _isActive = inUserDto.isActive ?? _isActive;
            _lastSeenAt = inUserDto.lastSeenAt ?? _lastSeenAt;
            _friendName = inUserDto.friendName ?? _friendName;
            _friendDiscoveryKey = inUserDto.friendDiscoveryKey ?? _friendDiscoveryKey;
            _preferredLanguages = inUserDto.preferredLanguages ?? _preferredLanguages;
            _metaData = inUserDto.metaData ?? _metaData;
            RequireAuthForProfileImage = inUserDto.requireAuthForProfileImage ?? RequireAuthForProfileImage;

            if (inUserDto.isOnline != null) { _connectionStatus = inUserDto.isOnline == true ? SbUserConnectionStatus.Online : SbUserConnectionStatus.Offline; }

            OnUpdateFromDto(inUserDto);
        }

        private string ConvertToOriginalProfileUrl(string inPlainProfileUrl)
        {
            if (string.IsNullOrEmpty(inPlainProfileUrl))
                return string.Empty;

            try
            {
                Uri uri = new Uri(inPlainProfileUrl);
                if (string.IsNullOrEmpty(uri.Scheme) || string.IsNullOrEmpty(uri.Host) || string.IsNullOrEmpty(uri.LocalPath))
                    return string.Empty;

                const StringComparison STRING_COMPARISON = StringComparison.OrdinalIgnoreCase;
                if (uri.Scheme.Equals("https", STRING_COMPARISON) == false && uri.Scheme.Equals("http", STRING_COMPARISON) == false)
                    return string.Empty;

                if (uri.Host.Equals("sendbird.com", STRING_COMPARISON) == false && uri.Host.Equals("intoz.com", STRING_COMPARISON) == false &&
                    uri.Host.Equals("sendbirdtest.com", STRING_COMPARISON) == false)
                    return string.Empty;

                const string PROFILE_IMAGES_PATH = "/profile_images/";
                if (uri.LocalPath.Contains(PROFILE_IMAGES_PATH))
                {
                    return inPlainProfileUrl.Replace(PROFILE_IMAGES_PATH, "/");
                }
            }
            catch (Exception exception)
            {
                Logger.Verbose(Logger.CategoryType.User, $"ConvertToOriginalProfileUrl:{inPlainProfileUrl} exception:{exception.Message}");
            }

            return string.Empty;
        }

        private protected virtual void OnUpdateFromDto(UserDto inUserDto) { }
    }
}