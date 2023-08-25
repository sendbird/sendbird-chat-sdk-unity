// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        internal SbUnreadMessageCount GetUnreadMessageCount()
        {
            return ChatMainContext.UnreadMessageCount;
        }

        internal int GetSubscribedTotalUnreadMessageCount()
        {
            return ChatMainContext.UnreadMessageCount.GroupChannelCount;
        }

        internal int GetSubscribedCustomTypeTotalUnreadMessageCount()
        {
            return ChatMainContext.UnreadMessageCount.TotalCountByCustomTypes;
        }

        internal int GetSubscribedCustomTypeUnreadMessageCount(string inCustomType)
        {
            return ChatMainContext.UnreadMessageCount.GetUnreadCount(inCustomType);
        }

        internal void GetTotalUnreadChannelCount(SbGroupChannelTotalUnreadMessageCountParams inParams, SbCountHandler inCompletionHandler)
        {
            if (inParams == null || CurrentUser == null)
            {
                if (inCompletionHandler == null)
                    return;

                SbError error = null;
                if (inParams == null)
                {
                    error = SbErrorCodeExtension.CreateInvalidParameterError("GroupChannelTotalUnreadMessageCountParams");
                }
                else if (CurrentUser == null)
                {
                    error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                }

                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(0, error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(0, inError);
                    return;
                }

                if (inResponse is GetTotalUnreadChannelCountApiCommand.Response response)
                {
                    inCompletionHandler?.Invoke(response.unreadCount, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(0, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetTotalUnreadChannelCountApiCommand.Request apiCommand = new GetTotalUnreadChannelCountApiCommand.Request(
                ChatMainContext.CurrentUserId, inParams.ChannelCustomTypesFilter, inParams.SuperChannelFilter, OnCompletionHandler);

            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetTotalUnreadMessageCount(SbGroupChannelTotalUnreadMessageCountParams inParams, SbUnreadMessageCountHandler inCompletionHandler)
        {
            if (inParams == null || CurrentUser == null)
            {
                if (inCompletionHandler == null)
                    return;

                SbError error = null;
                if (inParams == null)
                {
                    error = SbErrorCodeExtension.CreateInvalidParameterError("GroupChannelTotalUnreadMessageCountParams");
                }
                else if (CurrentUser == null)
                {
                    error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                }

                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(0, 0, error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(0, 0, inError);
                    return;
                }

                if (inResponse is GetTotalUnreadMessageCountApiCommand.Response response)
                {
                    inCompletionHandler?.Invoke(response.groupChannelUnreadCount, response.feedChannelUnreadCount, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(0, 0, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetTotalUnreadMessageCountApiCommand.Request apiCommand = new GetTotalUnreadMessageCountApiCommand.Request(
                ChatMainContext.CurrentUserId, inParams.ChannelCustomTypesFilter, inParams.SuperChannelFilter, OnCompletionHandler);

            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetUnreadItemCount(List<SbUnreadItemKey> inKeys, SbUnreadItemCountHandler inCompletionHandler)
        {
            if (inKeys == null || inKeys.Count <= 0 || CurrentUser == null)
            {
                if (inCompletionHandler == null)
                    return;

                SbError error = null;
                if (inKeys == null || inKeys.Count <= 0)
                {
                    error = SbErrorCodeExtension.CreateInvalidParameterError("Keys");
                }
                else if (CurrentUser == null)
                {
                    error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                }

                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is GetUnreadItemCountApiCommand.Response response)
                {
                    inCompletionHandler?.Invoke(response.CountByUnreadItemKey, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetUnreadItemCountApiCommand.Request apiCommand = new GetUnreadItemCountApiCommand.Request(
                ChatMainContext.CurrentUserId, inKeys, OnCompletionHandler);

            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}