// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    public partial class SbAppInfo
    {
        private string _emojiHash;
        private long _uploadSizeLimit;
        private bool _useReaction;
        private List<string> _premiumFeatureList;
        private List<string> _attributesInUse;
        internal bool DisableSuperGroupMack { get; private set; }

        internal SbAppInfo() { }

        internal void ResetFromDto(AppInfoDto inAppInfoDto)
        {
            if (inAppInfoDto != null)
            {
                _emojiHash = inAppInfoDto.emojiHash;
                _uploadSizeLimit = inAppInfoDto.fileUploadSizeLimit;
                _useReaction = inAppInfoDto.useReaction;
                _premiumFeatureList = inAppInfoDto.premiumFeatures;
                _attributesInUse = inAppInfoDto.applicationAttributes;
                DisableSuperGroupMack = inAppInfoDto.disableSuperGroupMack;
            }
        }
    }
}