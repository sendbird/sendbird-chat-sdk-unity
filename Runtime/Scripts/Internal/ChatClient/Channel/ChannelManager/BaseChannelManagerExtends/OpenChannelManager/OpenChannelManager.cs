// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal partial class OpenChannelManager : BaseChannelManager<SbOpenChannel, SbOpenChannelHandler>
    {
        private readonly Dictionary<string, SbOpenChannel> _enteredChannelsByUrl = new Dictionary<string, SbOpenChannel>();

        internal OpenChannelManager(SendbirdChatMainContext inChatMainContext) : base(inChatMainContext) { }

        internal override void Terminate()
        {
            _enteredChannelsByUrl.Clear();
            base.Terminate();
        }

        internal void CreateChannel(SbOpenChannelCreateParams inChannelCreateParams, SbOpenChannelCallbackHandler inCompletionHandler)
        {
            if (inChannelCreateParams == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("OpenChannelCreateParams");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }
            
            if (chatMainContextRef == null || chatMainContextRef.CurrentUserRef == null)
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
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

                if (inResponse is CreateOpenChannelApiCommand.Response createOpenChannelResponse && createOpenChannelResponse.OpenChannelDto != null)
                {
                    SbOpenChannel openChannel = CreateOrUpdateChannel(createOpenChannelResponse.OpenChannelDto);
                    inCompletionHandler?.Invoke(openChannel, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            CreateOpenChannelApiCommand.Request createOpenChannelApiCommand = new CreateOpenChannelApiCommand.Request(inChannelCreateParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(createOpenChannelApiCommand);
        }

        internal void GetChannel(string inChannelUrl, bool inIsInternal = false, bool inIsForceRefresh = false, SbGetOpenChannelHandler inCompletionHandler = null)
        {
            if (string.IsNullOrEmpty(inChannelUrl))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("ChannelUrl");
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(null, false, error); });
                return;
            }

            if (inIsForceRefresh == false && cachedChannelsByUrl.TryGetValue(inChannelUrl, out SbOpenChannel cachedOpenChannel))
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler.Invoke(cachedOpenChannel, true, null); });
                }

                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, false, inError);
                    return;
                }

                if (inResponse is GetOpenChannelApiCommand.Response openChannelResponse && openChannelResponse.OpenChannelDto != null)
                {
                    SbOpenChannel responseOpenChannel = CreateOrUpdateChannel(openChannelResponse.OpenChannelDto);
                    inCompletionHandler?.Invoke(responseOpenChannel, false, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, false, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetOpenChannelApiCommand.Request getOpenChannelApiCommand = new GetOpenChannelApiCommand.Request(inChannelUrl, inIsInternal, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(getOpenChannelApiCommand);
        }

        internal void AddEnteredChannelIfNotContains(SbOpenChannel inOpenChannel)
        {
            _enteredChannelsByUrl.AddIfNotContains(inOpenChannel?.Url, inOpenChannel);
        }

        internal void RemoveEnteredChannelIfContains(string inChannelUrl)
        {
            _enteredChannelsByUrl.RemoveIfContains(inChannelUrl);
        }

        protected override SbOpenChannel CreateChannelInstance(string inChannelUrl)
        {
            return new SbOpenChannel(inChannelUrl, chatMainContextRef);
        }

        internal void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType)
        {
            if (inChangedStateType == ConnectionStateInternalType.Connected)
            {
                foreach (KeyValuePair<string, SbOpenChannel> keyValuePair in _enteredChannelsByUrl)
                {
                    keyValuePair.Value.Enter(null);
                }
            }
        }
    }
}