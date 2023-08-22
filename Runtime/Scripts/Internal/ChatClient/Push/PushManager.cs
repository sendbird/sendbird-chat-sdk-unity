// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal class PushManager
    {
        private readonly SendbirdChatMainContext _chatMainContextRef = null;

        internal PushManager(SendbirdChatMainContext inChatMainContext)
        {
            _chatMainContextRef = inChatMainContext;
        }

        internal void RegisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, bool inUnique, SbPushTokenRegistrationStatusHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inPushToken))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("PushToken");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(SbPushTokenRegistrationStatus.Error, error));
                return;
            }
            else if (string.IsNullOrEmpty(_chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(SbPushTokenRegistrationStatus.Error, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                SbPushTokenRegistrationStatus status = inError == null ? SbPushTokenRegistrationStatus.Success : SbPushTokenRegistrationStatus.Error;
                inCompletionHandler?.Invoke(status, inError);
            }

            RegisterPushTokenApiCommand.Request apiCommand = new RegisterPushTokenApiCommand.Request(
                _chatMainContextRef.CurrentUserId, inPushTokenType, inPushToken, inUnique, inAlwaysPush: false, inInternal: false, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void UnregisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, SbErrorHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inPushToken))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("PushToken");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }
            else if (string.IsNullOrEmpty(_chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            UnregisterPushTokenApiCommand.Request apiCommand = new UnregisterPushTokenApiCommand.Request(
                _chatMainContextRef.CurrentUserId, inPushTokenType, inPushToken, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void UnregisterAllPushToken(SbErrorHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(_chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            UnregisterAllPushTokenApiCommand.Request apiCommand = new UnregisterAllPushTokenApiCommand.Request(_chatMainContextRef.CurrentUserId, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetMyPushTokensByToken(string inToken, SbPushTokenType inPushTokenType, SbPushTokensHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(_chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(
                                                              inPushTokens: null, inPushTokenType, false, inToken: null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetPushTokensApiCommand.Response tokensResponse)
                {
                    inCompletionHandler?.Invoke(tokensResponse.deviceTokens, tokensResponse.PushTokenType, tokensResponse.hasMore, tokensResponse.token, inError);
                }
                else
                {
                    inCompletionHandler?.Invoke(inPushTokens: null, inPushTokenType, false, inToken: null, inError);
                }
            }

            GetPushTokensApiCommand.Request apiCommand = new GetPushTokensApiCommand.Request(
                _chatMainContextRef.CurrentUserId, inPushTokenType, inToken, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void SetPushTemplate(string inTemplateName, SbErrorHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(inTemplateName))
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("TemplateName");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            SetPushTemplateApiCommand.Request apiCommand = new SetPushTemplateApiCommand.Request(_chatMainContextRef.CurrentUserId, inTemplateName, OnCompletionHandler);
            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetPushTemplate(SbPushTemplateHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetPushTemplateApiCommand.Response pushTemplateResponse)
                {
                    inCompletionHandler.Invoke(pushTemplateResponse.name, inError);
                    return;
                }

                inCompletionHandler.Invoke(null, inError);
            }

            GetPushTemplateApiCommand.Request apiCommand = new GetPushTemplateApiCommand.Request(_chatMainContextRef.CurrentUserId, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void SetSnoozePeriod(bool inEnabled, long inStartTimestamp, long inEndTimestamp, SbErrorHandler inCompletionHandler)
        {
            if (inEndTimestamp <= inStartTimestamp)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("EndTimestamp");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            SetSnoozePeriodApiCommand.Request apiCommand = new SetSnoozePeriodApiCommand.Request(
                _chatMainContextRef.CurrentUserId, inEnabled, inStartTimestamp, inEndTimestamp, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetSnoozePeriod(SbSnoozePeriodHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetSnoozePeriodApiCommand.Response doNotDisturbResponse)
                {
                    inCompletionHandler.Invoke(doNotDisturbResponse.snoozeEnabled, doNotDisturbResponse.snoozeStartTimestamp, doNotDisturbResponse.snoozeEndTimestamp, inError);
                    return;
                }

                inCompletionHandler.Invoke(inEnabled: false, inStartTimestamp: 0, inEndTimestamp: 0, inError);
            }

            GetSnoozePeriodApiCommand.Request apiCommand = new GetSnoozePeriodApiCommand.Request(_chatMainContextRef.CurrentUserId, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void SetDoNotDisturb(bool inEnabled, int inStartHour, int inStartMin, int inEndHour, int inEndMin, string inTimezone, SbErrorHandler inCompletionHandler)
        {
            if (inStartHour < 0 || 23 < inStartHour || inEndHour < 0 || 23 < inEndHour || inStartMin < 0 || 59 < inStartMin || inEndMin < 0 || 59 < inEndMin)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(SbErrorCodeExtension.INVALID_PARAMETER_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            SetDoNotDisturbApiCommand.Request apiCommand = new SetDoNotDisturbApiCommand.Request(
                _chatMainContextRef.CurrentUserId, inEnabled, inStartHour, inStartMin, inEndHour, inEndMin, inTimezone, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetDoNotDisturb(SbDoNotDisturbHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetDoNotDisturbApiCommand.Response doNotDisturbResponse)
                {
                    inCompletionHandler.Invoke(doNotDisturbResponse.isDoNotDisturbEnable, doNotDisturbResponse.startHour, doNotDisturbResponse.startMin,
                                               doNotDisturbResponse.endHour, doNotDisturbResponse.endMin, doNotDisturbResponse.timezone, inError);
                    return;
                }

                inCompletionHandler.Invoke(inEnabled: false, inStartHour: 0, inStartMin: 0, inEndHour: 0, inEndMin: 0, inTimezone: null, inError);
            }

            GetDoNotDisturbApiCommand.Request apiCommand = new GetDoNotDisturbApiCommand.Request(_chatMainContextRef.CurrentUserId, OnCompletionHandler);

            _chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}