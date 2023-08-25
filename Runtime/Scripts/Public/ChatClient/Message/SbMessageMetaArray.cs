// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents meta array of SbBaseMessage.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageMetaArray
    {
        /// <summary>
        /// The meta array key
        /// </summary>
        /// @since 4.0.0
        public string Key { get; }

        /// <summary>
        /// The meta array values
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<string> Value => _value;
        
        /// @since 4.0.0
        public SbMessageMetaArray(string inKey, List<string> inValue = null)
        {
            Key = inKey;
            if (inValue != null && 0 < inValue.Count)
            {
                _value.AddRange(inValue);
            }
        }

        /// <summary>
        /// Add new value to meta array value. If meta array value contains given data, it will be ignored.
        /// </summary>
        /// <param name="inValue"></param>
        /// @since 4.0.0
        public void AddValue(string inValue)
        {
            AddValueInternal(inValue);
        }

        /// <summary>
        /// Add new value list to meta array value. If meta array value contains given data, it will be ignored.
        /// </summary>
        /// <param name="inValue"></param>
        /// @since 4.0.0
        public void AddValue(IList<string> inValue)
        {
            AddValueInternal(inValue);
        }

        /// <summary>
        /// Remove value from meta array.
        /// </summary>
        /// <param name="inValue"></param>
        /// @since 4.0.0
        public void RemoveValue(string inValue)
        {
            RemoveValueInternal(inValue);
        }
    }
}