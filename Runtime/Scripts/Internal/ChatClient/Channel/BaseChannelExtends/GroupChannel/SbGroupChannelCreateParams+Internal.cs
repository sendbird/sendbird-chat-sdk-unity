// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.IO;

namespace Sendbird.Chat
{
    public partial class SbGroupChannelCreateParams
    {
        private string _accessCode;
        private string _channelUrl;
        private SbFileInfo _coverImage;
        private string _coverUrl;
        private string _customType;
        private string _data;
        private bool _isBroadcast = false;
        private bool _isDiscoverable = true;
        private bool _isDistinct = false;
        private bool _isEphemeral = false;
        private bool _isExclusive = false;
        private bool _isPublic = false;
        private bool _isStrict = false;
        private bool _isSuper = false;
        private int _messageSurvivalSeconds = -1;
        private string _name;
        private List<string> _operatorUserIds;
        private List<string> _userIds;

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