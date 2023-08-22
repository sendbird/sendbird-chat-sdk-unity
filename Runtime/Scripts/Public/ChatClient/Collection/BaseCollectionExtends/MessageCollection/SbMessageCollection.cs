// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Collection that handles message lists, also supporting local caching.
    /// </summary>
    /// @since 4.0.0
    public partial class SbMessageCollection : SbBaseCollection
    {
        /// <summary>
        /// The SbGroupChannel tracked by this SbMessageCollection.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannel GroupChannel => _groupChannel;

        /// <summary>
        /// The list of succeeded message list in this collection.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbBaseMessage> SucceededMessages => _succeededMessages;

        /// <summary>
        /// The failed message lists.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbBaseMessage> FailedMessages => _failedMessages;

        /// <summary>
        /// The pending message lists.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbBaseMessage> PendingMessages => _pendingMessages;

        /// <summary>
        /// The starting point of the collection.
        /// </summary>
        /// @since 4.0.0
        public long StartingPoint => _startingPoint;

        /// <summary>
        /// Whether there's more data to load in next (latest) direction.
        /// </summary>
        /// @since 4.0.0
        public bool HasNext => _hasNext;

        /// <summary>
        /// Whether there's more data to load in previous (oldest) direction.
        /// </summary>
        /// @since 4.0.0
        public bool HasPrevious => _hasPrevious;
        
        /// @since 4.0.0
        public SbMessageCollectionHandler MessageCollectionHandler { get => _messageCollectionHandler; set => _messageCollectionHandler = value; }

        /// <summary>
        /// Initializes this collection from startingPoint.
        /// </summary>
        /// <param name="inMessageCollectionInitHandler"></param>
        /// @since 4.0.0
        public void Initialize(SbMessageCollectionInitHandler inMessageCollectionInitHandler)
        {
            InitializeInternal(inMessageCollectionInitHandler);
        }

        /// <summary>
        /// Disposes current MessageCollection and stops all events from being received.
        /// </summary>
        /// @since 4.0.0
        public void Dispose()
        {
            DisposeInternal();
        }

        /// <summary>
        /// Loads next (latest direction) message lists.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadNext(SbMessageListHandler inCompletionHandler)
        {
            LoadNextInternal(inCompletionHandler);
        }

        /// <summary>
        /// Loads previous (oldest direction) message lists.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void LoadPrevious(SbMessageListHandler inCompletionHandler)
        {
            LoadPreviousInternal(inCompletionHandler);
        }

        /// <summary>
        /// Remove all failed messages of this BaseMessageCollection
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void RemoveAllFailedMessages(SbErrorHandler inCompletionHandler)
        {
            RemoveAllFailedMessagesInternal(inCompletionHandler);
        }

        /// <summary>
        /// Remove specific failed messages of this BaseMessageCollection
        /// </summary>
        /// <param name="inFailedMessages"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void RemoveFailedMessages(List<SbBaseMessage> inFailedMessages, SbRemoveFailedMessagesHandler inCompletionHandler)
        {
            RemoveFailedMessagesInternal(inFailedMessages, inCompletionHandler);
        }
    }
}