// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbUser
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

                if (inResponse is CreateUserMetaDataApiCommand.Response createMetaCountersResponse)
                {
                    InsertAllMetaData(createMetaCountersResponse.MetaData);
                    inCompletionHandler?.Invoke(createMetaCountersResponse.MetaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            CreateUserMetaDataApiCommand.Request apiCommand = new CreateUserMetaDataApiCommand.Request(_userId, inMetaData, OnCompletionHandler);
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

                if (inResponse is UpdateUserMetaDataApiCommand.Response createMetaCountersResponse)
                {
                    InsertAllMetaData(createMetaCountersResponse.MetaData);
                    inCompletionHandler?.Invoke(createMetaCountersResponse.MetaData, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            UpdateUserMetaDataApiCommand.Request apiCommand = new UpdateUserMetaDataApiCommand.Request(_userId, inUpsert: true, inMetaData, OnCompletionHandler);
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
                    _metaData.RemoveIfContains(inKey);
                }

                inCompletionHandler?.Invoke(inError);
            }

            DeleteUserMetaDataApiCommand.Request apiCommand = new DeleteUserMetaDataApiCommand.Request(_userId, inKey, OnCompletionHandler);
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

            DeleteAllUserMetaDataApiCommand.Request apiCommand = new DeleteAllUserMetaDataApiCommand.Request(_userId, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private string GetMetaDataInternal(string inKey)
        {
            if (string.IsNullOrEmpty(inKey))
                return string.Empty;

            if (_metaData.TryGetValue(inKey, out string metaDataValue))
                return metaDataValue;

            return null;
        }

        private void InsertAllMetaData(Dictionary<string, string> inMetaData)
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
            if (string.IsNullOrEmpty(inKey))
            {
                Logger.Warning(Logger.CategoryType.User, "InsertMetaData() => Invalid parameter: Key");
                return;
            }

            if (_metaData.ContainsKey(inKey))
            {
                _metaData[inKey] = inValue;
            }
            else
            {
                _metaData.Add(inKey, inValue);
            }
        }

        private void ClearMetaData()
        {
            _metaData.Clear();
        }
    }
}