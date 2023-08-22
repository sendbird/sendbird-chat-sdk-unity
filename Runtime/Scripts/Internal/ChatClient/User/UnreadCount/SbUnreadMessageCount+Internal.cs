// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbUnreadMessageCount
    {
        private int _groupChannelCount = 0;
        private int _feedChannelCount = 0;
        private int _totalCountByCustomTypes = 0;
        private readonly Dictionary<string, int> _customTypeUnreadCountMap = new Dictionary<string, int>();
        private long _timestamp = 0;

        private int GetUnreadCountInternal(string inCustomType)
        {
            if (_customTypeUnreadCountMap.TryGetValue(inCustomType, out int count))
                return count;

            return 0;
        }

        internal bool UpdateFrom(UnreadMessageCountDto inUnreadMessageCountDto)
        {
            if (inUnreadMessageCountDto == null || inUnreadMessageCountDto.timeStamp <= _timestamp)
                return false;

            _timestamp = inUnreadMessageCountDto.timeStamp;

            bool changed = false;
            if (_groupChannelCount != inUnreadMessageCountDto.groupChannelCount)
            {
                _groupChannelCount = inUnreadMessageCountDto.groupChannelCount;
                changed = true;
            }

            if (_feedChannelCount != inUnreadMessageCountDto.feedChannelCount)
            {
                _feedChannelCount = inUnreadMessageCountDto.feedChannelCount;
                changed = true;
            }

            if (inUnreadMessageCountDto.customTypes != null && 0 < inUnreadMessageCountDto.customTypes.Count)
            {
                foreach (KeyValuePair<string, int> keyValuePair in inUnreadMessageCountDto.customTypes)
                {
                    if (_customTypeUnreadCountMap.TryGetValue(keyValuePair.Key, out int oldCount) == false || oldCount != keyValuePair.Value)
                    {
                        changed = true;
                        break;
                    }
                }

                _customTypeUnreadCountMap.Clear();
                _totalCountByCustomTypes = 0;
                foreach (KeyValuePair<string, int> keyValuePair in inUnreadMessageCountDto.customTypes)
                {
                    _totalCountByCustomTypes += keyValuePair.Value;
                    _customTypeUnreadCountMap.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            else if (0 < _customTypeUnreadCountMap.Count)
            {
                _customTypeUnreadCountMap.Clear();
                _totalCountByCustomTypes = 0;
                changed = true;
            }

            return changed;
        }
    }
}