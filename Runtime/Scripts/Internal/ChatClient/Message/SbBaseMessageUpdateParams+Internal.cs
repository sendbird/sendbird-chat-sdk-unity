// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public abstract partial class SbBaseMessageUpdateParams
    {
        private string _customType;
        private string _data;
        private SbMentionType _mentionType = SbMentionType.Users;
        private List<string> _mentionedUserIds;

        internal SbBaseMessageUpdateParams() { }
        internal virtual SbBaseMessageUpdateParams Clone()
        {
            return null;
        }

        private protected SbBaseMessageUpdateParams(SbBaseMessageUpdateParams inOtherParams)
        {
            if (inOtherParams != null)
            {
                _customType = inOtherParams._customType;
                _data = inOtherParams._data;
                _mentionType = inOtherParams._mentionType;

                if (inOtherParams._mentionedUserIds != null && 0 < inOtherParams._mentionedUserIds.Count)
                {
                    _mentionedUserIds = new List<string>(inOtherParams._mentionedUserIds);
                }
            }
        }
    }
}