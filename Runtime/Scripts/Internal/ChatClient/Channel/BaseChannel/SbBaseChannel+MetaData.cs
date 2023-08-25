// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void CreateMetaDataInternal(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            if (inMetaData == null || inMetaData.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MetaData");
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

                if (inResponse is CreateMetaDataApiCommand.Response createMetaDataResponse)
                {
                    InsertAllMetaData(createMetaDataResponse.metaData);
                    inCompletionHandler?.Invoke(createMetaDataResponse.metaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            CreateMetaDataApiCommand.Request apiCommand = new CreateMetaDataApiCommand.Request(Url, ChannelType, inMetaData, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void GetMetaDataInternal(List<string> inKeys, SbMetaDataHandler inCompletionHandler)
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
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is GetMetaDataApiCommand.Response getMetaDataResponse)
                {
                    InsertAllMetaData(getMetaDataResponse.metaData);
                    inCompletionHandler?.Invoke(getMetaDataResponse.metaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetMetaDataApiCommand.Request apiCommand = new GetMetaDataApiCommand.Request(Url, ChannelType, inKeys, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void GetAllMetaDataInternal(SbMetaDataHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                if (inResponse is GetAllMetaDataApiCommand.Response getMetaDataResponse)
                {
                    ClearMetaData();
                    InsertAllMetaData(getMetaDataResponse.metaData);
                    inCompletionHandler?.Invoke(getMetaDataResponse.metaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetAllMetaDataApiCommand.Request apiCommand = new GetAllMetaDataApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void UpdateMetaDataInternal(Dictionary<string, string> inMetaData, SbMetaDataHandler inCompletionHandler)
        {
            if (inMetaData == null || inMetaData.Count <= 0)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("MetaData");
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

                if (inResponse is UpdateMetaDataApiCommand.Response updateMetaDataResponse)
                {
                    InsertAllMetaData(updateMetaDataResponse.metaData);
                    inCompletionHandler?.Invoke(updateMetaDataResponse.metaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateMetaDataApiCommand.Request apiCommand = new UpdateMetaDataApiCommand.Request(Url, ChannelType, inMetaData, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeleteMetaDataInternal(string inKey, SbErrorHandler inCompletionHandler)
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
                if (inError == null)
                {
                    _cachedMetaDataByKey.RemoveIfContains(inKey);
                }

                inCompletionHandler?.Invoke(inError);
            }

            DeleteMetaDataApiCommand.Request apiCommand = new DeleteMetaDataApiCommand.Request(Url, ChannelType, inKey, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void DeleteAllMetaDataInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError == null)
                {
                    ClearMetaData();
                }

                inCompletionHandler?.Invoke(inError);
            }

            DeleteAllMetaDataApiCommand.Request apiCommand = new DeleteAllMetaDataApiCommand.Request(Url, ChannelType, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
        
        internal void InsertAllMetaData(Dictionary<string, string> inMetaData)
        {
            if (inMetaData == null || inMetaData.Count <= 0)
                return;

            foreach (KeyValuePair<string, string> keyValuePair in inMetaData)
            {
                InsertMetaData(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void InsertMetaData(string inKey, string inValue)
        {
            if (_cachedMetaDataByKey.ContainsKey(inKey))
            {
                _cachedMetaDataByKey[inKey] = inValue;
            }
            else
            {
                _cachedMetaDataByKey.Add(inKey, inValue);
            }
        }

        internal void RemoveAllMetaDataIfContains(IReadOnlyList<string> inMetaDataKeys)
        {
            if (inMetaDataKeys == null || inMetaDataKeys.Count <= 0)
                return;

            foreach (string metaDataKey in inMetaDataKeys)
            {
                _cachedMetaDataByKey.RemoveIfContains(metaDataKey);
            }
        }

        private void ClearMetaData()
        {
            _cachedMetaDataByKey.Clear();
        }
    }
}