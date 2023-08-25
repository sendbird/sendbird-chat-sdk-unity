// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void ReportUserInternal(SbUser inOffendingUser, SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            if (inOffendingUser == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("OffendingUser");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            if (chatMainContextRef.CurrentUserRef == null)
            {
                SbError error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            string offendingUserId = inOffendingUser.UserId;
            string reporterUserId = chatMainContextRef.CurrentUserId;
            ReportUserApiCommand.Request apiCommand = new ReportUserApiCommand.Request(
                Url, ChannelType, offendingUserId, inReportCategory, reporterUserId, inReportDescription, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void ReportMessageInternal(SbBaseMessage inMessage, SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            if (inMessage == null)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            if (chatMainContextRef.CurrentUserRef == null)
            {
                SbError error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }

            string offendingUserId = inMessage.Sender?.UserId;
            string reporterUserId = chatMainContextRef.CurrentUserId;
            ReportMessageApiCommand.Request apiCommand = new ReportMessageApiCommand.Request(
                Url, ChannelType, offendingUserId, inMessage.MessageId, inReportCategory, reporterUserId, inReportDescription, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        private void ReportInternal(SbReportCategory inReportCategory, string inReportDescription, SbErrorHandler inCompletionHandler)
        {
            if (chatMainContextRef.CurrentUserRef == null)
            {
                SbError error = SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR;
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(error));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }
            
            string reporterUserId = chatMainContextRef.CurrentUserId;
            ReportChannelApiCommand.Request apiCommand = new ReportChannelApiCommand.Request(
                Url, ChannelType, inReportCategory, reporterUserId, inReportDescription, OnCompletionHandler);

            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}