// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using UnityEngine.PlayerLoop;

namespace Sendbird.Chat
{
    internal abstract partial class BaseChannelManager<TChannelType, TChannelHandlerType> where TChannelType : SbBaseChannel where TChannelHandlerType : SbBaseChannelHandler
    {
        internal void ProcessBaseChannelWsEventCommand(WsReceiveCommandAbstract inWsReceiveCommand)
        {
            if (inWsReceiveCommand == null)
                return;

            switch (inWsReceiveCommand.CommandType)
            {
                case WsCommandType.DeleteMessage:
                    OnReceiveDeleteMessageWsEvent(inWsReceiveCommand as DeleteMessageWsReceiveCommand);
                    break;
                case WsCommandType.Reaction:
                    OnReceiveMessageReactionWsEvent(inWsReceiveCommand as ReactionWsReceiveCommand);
                    break;
                default: break;
            }
        }

        private void OnReceiveDeleteMessageWsEvent(DeleteMessageWsReceiveCommand inReceiveCommand)
        {
            if (inReceiveCommand == null || string.IsNullOrEmpty(inReceiveCommand.channelUrl))
            {
                Logger.Warning(Logger.CategoryType.Channel, "BaseChannelManager::OnReceiveDeleteMessageWsEvent invalid params");
                return;
            }

            void OnGetChannelCompletionHandler(SbBaseChannel inChannel, bool inIsFromCache, SbError inError)
            {
                if (inChannel == null)
                    return;

                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMessageDeleted?.Invoke(inChannel, inReceiveCommand.msgId); });
                chatMainContextRef.CollectionManager.OnMessageDeleted(inChannel, inReceiveCommand.msgId);
            }

            GetChannel(inReceiveCommand.ChannelType, inReceiveCommand.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void OnReceiveMessageReactionWsEvent(ReactionWsReceiveCommand inReceiveCommand)
        {
            if (inReceiveCommand == null || string.IsNullOrEmpty(inReceiveCommand.channelUrl) || inReceiveCommand.ReactionEventDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "BaseChannelManager::OnReceiveMessageReactionWsEvent invalid params");
                return;
            }

            void OnGetChannelCompletionHandler(SbBaseChannel inChannel, bool inIsFromCache, SbError inError)
            {
                if (inChannel == null)
                    return;

                SbReactionEvent reactionEvent = new SbReactionEvent(inReceiveCommand.ReactionEventDto);
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnReactionUpdated?.Invoke(inChannel, reactionEvent); });
                chatMainContextRef.CollectionManager.OnReactionUpdated(inChannel, reactionEvent);
            }

            GetChannel(inReceiveCommand.ChannelType, inReceiveCommand.channelUrl, inIsInternal: false, inIsForceRefresh: false, OnGetChannelCompletionHandler);
        }

        private void GetChannel(SbChannelType inChannelType, string inChannelUrl, bool inIsInternal, bool inIsForceRefresh, Action<SbBaseChannel, bool, SbError> inCompletionHandler)
        {
            void OnGetGroupChannelCompletionHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError)
            {
                inCompletionHandler?.Invoke(inGroupChannel, inIsFromCache, inError);
            }

            void OnGetOpenChannelCompletionHandler(SbOpenChannel inOpenChannel, bool inIsFromCache, SbError inError)
            {
                inCompletionHandler?.Invoke(inOpenChannel, inIsFromCache, inError);
            }

            if (inChannelType == SbChannelType.Open && this is OpenChannelManager openChannelManager)
            {
                openChannelManager.GetChannel(inChannelUrl, inIsInternal, inIsForceRefresh, OnGetOpenChannelCompletionHandler);
            }
            else if (inChannelType == SbChannelType.Group && this is GroupChannelManager groupChannelManager)
            {
                groupChannelManager.GetChannel(inChannelUrl, inIsInternal, inIsForceRefresh, OnGetGroupChannelCompletionHandler);
            }
            else
            {
                inCompletionHandler?.Invoke(null, false, new SbError(SbErrorCode.UnknownError));
            }
        }

        protected void ProcessBaseChannelEvent(SbBaseChannel inBaseChannel, ChannelEventDataAbstract inChannelEventData)
        {
            switch (inChannelEventData.CategoryType)
            {
                case ChannelReceiveWsReceiveCommand.CategoryType.Frozen:
                    OnReceiveFrozenChannelEvent(inBaseChannel);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.Unfrozen:
                    OnReceiveUnfrozenChannelEvent(inBaseChannel);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.MetaDataChanged:
                    OnReceiveMetaDataChangedChannelEvent(inBaseChannel, inChannelEventData as MetaDataChangedChannelEventData);
                    return;

                case ChannelReceiveWsReceiveCommand.CategoryType.MetaCounterChanged:
                    OnReceiveMetaCounterChangedChannelEvent(inBaseChannel, inChannelEventData as MetaCounterChangedChannelEventData);
                    return;
                default: return;
            }
        }

        private void OnReceiveFrozenChannelEvent(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OnReceiveFrozenChannelEvent invalid params");
                return;
            }

            inBaseChannel.IsFrozen = true;
            channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnChannelFrozen?.Invoke(inBaseChannel); });
            chatMainContextRef.CollectionManager.OnChannelFrozen(inBaseChannel);
        }

        private void OnReceiveUnfrozenChannelEvent(SbBaseChannel inBaseChannel)
        {
            if (inBaseChannel == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OnReceiveUnfrozenChannelEvent invalid params");
                return;
            }

            inBaseChannel.IsFrozen = false;
            channelHandlersById.ForEachByValue(inHandler => { inHandler.OnChannelUnfrozen?.Invoke(inBaseChannel); });
            chatMainContextRef.CollectionManager.OnChannelUnfrozen(inBaseChannel);
        }

        private void OnReceiveMetaDataChangedChannelEvent(SbBaseChannel inBaseChannel, MetaDataChangedChannelEventData inMetaDataChangedEventData)
        {
            if (inBaseChannel == null || inMetaDataChangedEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OnReceiveMetaDataChangedChannelEvent invalid params");
                return;
            }

            inBaseChannel.InsertAllMetaData(inMetaDataChangedEventData.Created);
            inBaseChannel.InsertAllMetaData(inMetaDataChangedEventData.Updated);
            inBaseChannel.RemoveAllMetaDataIfContains(inMetaDataChangedEventData.Deleted);

            if (0 < inMetaDataChangedEventData.Created?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaDataCreated?.Invoke(inBaseChannel, inMetaDataChangedEventData.Created); });
                chatMainContextRef.CollectionManager.OnMetaDataCreated(inBaseChannel);
            }
            
            if (0 < inMetaDataChangedEventData.Updated?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaDataUpdated?.Invoke(inBaseChannel, inMetaDataChangedEventData.Updated); });
                chatMainContextRef.CollectionManager.OnMetaDataUpdated(inBaseChannel);
            }

            if (0 < inMetaDataChangedEventData.Deleted?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaDataDeleted?.Invoke(inBaseChannel, inMetaDataChangedEventData.Deleted); });
                chatMainContextRef.CollectionManager.OnMetaDataDeleted(inBaseChannel);
            }
        }

        private void OnReceiveMetaCounterChangedChannelEvent(SbBaseChannel inBaseChannel, MetaCounterChangedChannelEventData inMetaCounterChangedEventData)
        {
            if (inBaseChannel == null || inMetaCounterChangedEventData == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "OnReceiveMetaCounterChangedChannelEvent invalid params");
                return;
            }

            if (0 < inMetaCounterChangedEventData.Created?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaCountersCreated?.Invoke(inBaseChannel, inMetaCounterChangedEventData.Created); });
                chatMainContextRef.CollectionManager.OnMetaCountersCreated(inBaseChannel);
            }

            if (0 < inMetaCounterChangedEventData.Updated?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaCountersUpdated?.Invoke(inBaseChannel, inMetaCounterChangedEventData.Updated); });
                chatMainContextRef.CollectionManager.OnMetaCountersUpdated(inBaseChannel);
            }

            if (0 < inMetaCounterChangedEventData.Deleted?.Count)
            {
                channelHandlersById.ForEachByValue(inHandler => { inHandler.OnMetaCountersDeleted?.Invoke(inBaseChannel, inMetaCounterChangedEventData.Deleted); });
                chatMainContextRef.CollectionManager.OnMetaCountersDeleted(inBaseChannel);
            }
        }
    }
}