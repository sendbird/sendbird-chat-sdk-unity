// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.IO;

namespace Sendbird.Chat
{
    public partial class SbUserUpdateParams
    {
        private string _nickName;
        private string _profileImageUrl;
        private SbFileInfo _profileImageFile;

        private void SetProfileImageUrl(string inProfileImageUrl)
        {
            _profileImageUrl = inProfileImageUrl;
            if (string.IsNullOrEmpty(_profileImageUrl) == false)
                _profileImageFile = null;
        }

        private void SetProfileImageFile(SbFileInfo inProfileImageFileInfo)
        {
            _profileImageFile = inProfileImageFileInfo;
            if (_profileImageFile != null)
                _profileImageUrl = null;
        }
    }
}