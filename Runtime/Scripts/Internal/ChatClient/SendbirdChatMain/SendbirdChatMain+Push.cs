// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal partial class SendbirdChatMain
    {
        internal void RegisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, bool inUnique, SbPushTokenRegistrationStatusHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.RegisterPushToken(inPushTokenType, inPushToken, inUnique, inCompletionHandler);
        }

        internal void UnregisterPushToken(SbPushTokenType inPushTokenType, string inPushToken, SbErrorHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.UnregisterPushToken(inPushTokenType, inPushToken, inCompletionHandler);
        }

        internal void UnregisterAllPushToken(SbErrorHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.UnregisterAllPushToken(inCompletionHandler);
        }

        internal void GetMyPushTokensByToken(string inToken, SbPushTokenType inPushTokenType, SbPushTokensHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.GetMyPushTokensByToken(inToken, inPushTokenType, inCompletionHandler);
        }

        internal void SetPushTemplate(string inTemplateName, SbErrorHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.SetPushTemplate(inTemplateName, inCompletionHandler);
        }

        internal void GetPushTemplate(SbPushTemplateHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.GetPushTemplate(inCompletionHandler);
        }

        internal void SetSnoozePeriod(bool inEnabled, long inStartTimestamp, long inEndTimestamp, SbErrorHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.SetSnoozePeriod(inEnabled, inStartTimestamp, inEndTimestamp, inCompletionHandler);
        }

        internal void GetSnoozePeriod(SbSnoozePeriodHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.GetSnoozePeriod(inCompletionHandler);
        }

        internal void SetDoNotDisturb(bool inEnabled, int inStartHour, int inStartMin, int inEndHour, int inEndMin, string inTimezone, SbErrorHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.SetDoNotDisturb(inEnabled, inStartHour, inStartMin, inEndHour, inEndMin, inTimezone, inCompletionHandler);
        }

        internal void GetDoNotDisturb(SbDoNotDisturbHandler inCompletionHandler)
        {
            ChatMainContext.PushManager.GetDoNotDisturb(inCompletionHandler);
        }

        internal void GetPushTriggerOption(SbPushTriggerOptionHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetPushTriggerOptionApiCommand.Response response)
                {
                    inCompletionHandler.Invoke(response.GroupChannelPushTriggerOptionNullable, inError);
                    return;
                }

                inCompletionHandler.Invoke(inPushTriggerOption: null, inError);
            }

            GetPushTriggerOptionApiCommand.Request apiCommand = new GetPushTriggerOptionApiCommand.Request(
                ChatMainContext.CurrentUserId, OnCompletionHandler);

            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void SetPushTriggerOption(SbPushTriggerOption inPushTriggerOption, SbErrorHandler inCompletionHandler)
        {
            if (inCompletionHandler == null)
                return;

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler.Invoke(inError);
            }

            SetPushTriggerOptionApiCommand.Request apiCommand = new SetPushTriggerOptionApiCommand.Request(
                ChatMainContext.CurrentUserId, inPushTriggerOption, OnCompletionHandler);

            ChatMainContext.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}