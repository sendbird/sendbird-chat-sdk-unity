// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    internal static class QueueExtension
    {
        internal static void InsertFirst<TItemType>(this Queue<TItemType> inQueue, TItemType inTargetItem) where TItemType : class
        {
            if (0 < inQueue.Count)
            {
                Queue<TItemType> tempQueue = new Queue<TItemType>(inQueue);
                inQueue.Clear();
                inQueue.Enqueue(inTargetItem);
                while (0 < tempQueue.Count)
                {
                    inQueue.Enqueue(tempQueue.Dequeue());
                }
            }
            else
            {
                inQueue.Enqueue(inTargetItem);
            }
        }
    }
}