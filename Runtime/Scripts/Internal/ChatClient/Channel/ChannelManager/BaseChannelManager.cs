// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal abstract partial class BaseChannelManager<TChannelType, TChannelHandlerType> where TChannelType : SbBaseChannel where TChannelHandlerType : SbBaseChannelHandler
    {
        protected readonly SendbirdChatMainContext chatMainContextRef = null;
        protected readonly Dictionary<string, TChannelType> cachedChannelsByUrl = new Dictionary<string, TChannelType>();
        protected readonly Dictionary<string, TChannelHandlerType> channelHandlersById = new Dictionary<string, TChannelHandlerType>();

        internal BaseChannelManager(SendbirdChatMainContext inChatMainContext)
        {
            chatMainContextRef = inChatMainContext;
        }

        internal virtual void Terminate()
        {
            cachedChannelsByUrl.Clear();
            channelHandlersById.Clear();
        }

        internal void RemoveCachedChannelIfContains(string inChannelUrl)
        {
            cachedChannelsByUrl.RemoveIfContains(inChannelUrl);
        }

        internal TChannelType FindCachedChannel(string inChannelUrl)
        {
            if (string.IsNullOrEmpty(inChannelUrl))
                return null;

            if (cachedChannelsByUrl.TryGetValue(inChannelUrl, out TChannelType channel))
                return channel;

            return null;
        }

        private TChannelType FindOrCreateChannel(string inChannelUrl)
        {
            TChannelType channel = FindCachedChannel(inChannelUrl);
            if (channel == null)
            {
                channel = CreateChannelInstance(inChannelUrl);
                cachedChannelsByUrl.Add(inChannelUrl, channel);
            }

            return channel;
        }

        protected abstract TChannelType CreateChannelInstance(string inChannelUrl);

        internal TChannelType CreateOrUpdateChannel(BaseChannelDto inChannelDto)
        {
            if (inChannelDto == null || string.IsNullOrEmpty(inChannelDto.channelUrl))
                return null;

            TChannelType channel = FindOrCreateChannel(inChannelDto.channelUrl);
            channel.ResetFromChannelDto(inChannelDto);
            return channel;
        }

        internal void AddChannelHandler(string inIdentifier, TChannelHandlerType inChannelHandler)
        {
            if (string.IsNullOrEmpty(inIdentifier) || inChannelHandler == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "AddChannelHandler() : Invalid parameter.");
                return;
            }

            if (channelHandlersById.ContainsKey(inIdentifier) == false)
            {
                channelHandlersById.Add(inIdentifier, inChannelHandler);
            }
            else
            {
                channelHandlersById[inIdentifier] = inChannelHandler;
            }
        }

        internal void RemoveChannelHandlerIfContains(string inIdentifier)
        {
            channelHandlersById.RemoveIfContains(inIdentifier);
        }

        internal void RemoveAllChannelHandlers()
        {
            channelHandlersById.Clear();
        }
    }
}