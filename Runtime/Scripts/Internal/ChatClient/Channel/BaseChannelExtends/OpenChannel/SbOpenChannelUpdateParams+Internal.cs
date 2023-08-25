// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbOpenChannelUpdateParams
    {
        private string _name;
        private string _coverUrl;
        private SbFileInfo _coverImage;
        private string _data;
        private string _customType;
        private List<string> _operatorUserIds;

        private void SetCoverImageAndNullUrl(SbFileInfo inCoverFileInfo)
        {
            _coverImage = inCoverFileInfo;
            if (_coverImage != null)
                _coverUrl = null;
        }

        private void SetCoverUrlAndNullImage(string inCoverUrl)
        {
            _coverUrl = inCoverUrl;
            if (string.IsNullOrEmpty(_coverUrl) == false)
                _coverImage = null;
        }
    }
}