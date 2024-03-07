// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void AddOperatorsInternal(List<string> inUserIds, SbErrorHandler inCompletionHandler)
        {
            if (inUserIds == null || inUserIds.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserIds");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            AddOperatorsApiCommand.Request apiCommand = new AddOperatorsApiCommand.Request(Url, ChannelType, inUserIds, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void RemoveOperatorsInternal(List<string> inUserIds, SbErrorHandler inCompletionHandler)
        {
            if (inUserIds == null || inUserIds.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("UserIds");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            RemoveOperatorsApiCommand.Request apiCommand = new RemoveOperatorsApiCommand.Request(Url, ChannelType, inUserIds, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void RemoveAllOperatorsInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            RemoveAllOperatorsApiCommand.Request apiCommand = new RemoveAllOperatorsApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private SbOperatorListQuery CreateOperatorListQueryInternal(SbOperatorListQueryParams inParams = null)
        {
            if (inParams == null)
                inParams = new SbOperatorListQueryParams();

            return new SbOperatorListQuery(ChannelType, _url, inParams, chatMainContextRef);
        }
    }
}