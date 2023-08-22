// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelUpdateParams
    {
        private string _accessCode;
        private SbFileInfo _coverImage;
        private string _coverUrl;
        private string _customType;
        private string _data;
        private bool _isDiscoverable = true;
        private bool _isDistinct = false;
        private bool _isPublic = false;
        private int _messageSurvivalSeconds = -1;
        private string _name;
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