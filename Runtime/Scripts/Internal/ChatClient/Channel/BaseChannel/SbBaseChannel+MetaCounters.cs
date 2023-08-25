// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void CreateMetaCountersInternal(Dictionary<string, int> inMetaCounters, SbMetaCountersHandler inCompletionHandler)
        {
            if (inMetaCounters == null || inMetaCounters.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MetaCounters");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is CreateMetaCountersApiCommand.Response createMetaCountersResponse)
                {
                    inCompletionHandler?.Invoke(createMetaCountersResponse.MetaCounters, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            CreateMetaCountersApiCommand.Request apiCommand = new CreateMetaCountersApiCommand.Request(Url, ChannelType, inMetaCounters, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void GetMetaCountersInternal(List<string> inKeys, SbMetaCountersHandler inCompletionHandler)
        {
            if (inKeys == null || inKeys.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Keys");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetMetaCountersApiCommand.Response getMetaCountersResponse)
                {
                    inCompletionHandler?.Invoke(getMetaCountersResponse.MetaCounters, null);
                }
                else
                {
                    if (inError != null)
                        inError = SbErrorCodeExtension.MALFORMED_DATA_ERROR;

                    inCompletionHandler?.Invoke(null, inError);
                }
            }

            GetMetaCountersApiCommand.Request apiCommand = new GetMetaCountersApiCommand.Request(Url, ChannelType, inKeys, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void GetAllMetaCountersInternal(SbMetaCountersHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is GetAllMetaCountersApiCommand.Response getMetaCountersResponse)
                {
                    inCompletionHandler?.Invoke(getMetaCountersResponse.MetaCounters, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetAllMetaCountersApiCommand.Request apiCommand = new GetAllMetaCountersApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UpdateMetaCountersInternal(Dictionary<string, int> inMetaCounters, UpdateMetaCountersApiCommand.UpdateModeType inUpdateModeType,
                                                bool inUpsert, SbMetaCountersHandler inCompletionHandler)
        {
            if (inMetaCounters == null || inMetaCounters.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MetaCounters");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is UpdateMetaCountersApiCommand.Response updateMetaCountersResponse)
                {
                    inCompletionHandler?.Invoke(updateMetaCountersResponse.MetaCounters, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateMetaCountersApiCommand.Request apiCommand = new UpdateMetaCountersApiCommand.Request(Url, ChannelType, inMetaCounters, inUpdateModeType, inUpsert, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeleteMetaCountersInternal(string inKey, SbErrorHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inKey))
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Key");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(error));
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            DeleteMetaCountersApiCommand.Request apiCommand = new DeleteMetaCountersApiCommand.Request(Url, ChannelType, inKey, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeleteAllMetaCountersInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            DeleteAllMetaCountersApiCommand.Request apiCommand = new DeleteAllMetaCountersApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}