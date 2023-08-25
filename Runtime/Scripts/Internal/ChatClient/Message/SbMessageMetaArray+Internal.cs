// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMessageMetaArray
    {
        internal List<string> ValueInternal => _value;
        private readonly List<string> _value = new List<string>();
        internal SbMessageMetaArray(MessageMetaArrayDto inMetaArrayDto)
        {
            if (inMetaArrayDto != null)
            {
                Key = inMetaArrayDto.key;
                if (inMetaArrayDto.value != null && 0 < inMetaArrayDto.value.Count)
                    _value = new List<string>(inMetaArrayDto.value);
            }
        }

        private void AddValueInternal(string inValue)
        {
            _value.Add(inValue);
        }

        private void AddValueInternal(IList<string> inValue)
        {
            if (inValue != null && 0 < inValue.Count)
            {
                _value.AddRange(inValue);
            }
        }

        private void RemoveValueInternal(string inValue)
        {
            _value.RemoveIfContains(inValue);
        }
    }
}