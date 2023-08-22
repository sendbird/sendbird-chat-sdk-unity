// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    internal static class ListExtension
    {
        internal static readonly IReadOnlyList<string> EMPTY_STRING_LIST = new List<string>();
        internal static bool AddIfNotContains<TItemType>(this List<TItemType> inList, TItemType inAddTargetItem)
        {
            if (inAddTargetItem == null)
                return false;

            if (inList.Contains(inAddTargetItem) == false)
            {
                inList.Add(inAddTargetItem);
                return true;
            }

            return false;
        }

        internal static bool RemoveIfContains<TItemType>(this List<TItemType> inList, TItemType inRemoveTargetItem)
        {
            if (inRemoveTargetItem == null)
                return false;

            for (int index = 0; index < inList.Count; index++)
            {
                if (inList[index].Equals(inRemoveTargetItem))
                {
                    inList.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }
    }
}