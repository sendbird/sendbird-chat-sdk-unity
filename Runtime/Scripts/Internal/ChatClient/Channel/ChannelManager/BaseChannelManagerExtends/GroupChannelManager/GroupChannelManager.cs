// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal partial class GroupChannelManager : BaseChannelManager<SbGroupChannel, SbGroupChannelHandler>
    {
        private long _lastInvalidateTypingCheckAtMs = 0;
        private long _lastMarkAsReadAllTimestampMs = 0;

        internal GroupChannelManager(SendbirdChatMainContext inChatMainContext) : base(inChatMainContext) { }

        internal void Update()
        {
            if (_lastInvalidateTypingCheckAtMs + 1000 < TimeUtil.GetCurrentUnixTimeMilliseconds())
            {
                RemoveInvalidateTypingStatuses();
                _lastInvalidateTypingCheckAtMs = TimeUtil.GetCurrentUnixTimeMilliseconds();
            }
        }

        internal void CreateChannel(SbGroupChannelCreateParams inChannelCreateParams, SbGroupChannelCallbackHandler inCompletionHandler)
        {
            if (inChannelCreateParams == null)
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("GroupChannelCreateParams");
                    CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler.Invoke(null, error));
                }

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

                if (inResponse is CreateGroupChannelApiCommand.Response createOpenChannelResponse && createOpenChannelResponse.GroupChannelDto != null)
                {
                    SbGroupChannel groupChannel = CreateOrUpdateChannel(createOpenChannelResponse.GroupChannelDto);
                    inCompletionHandler?.Invoke(groupChannel, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            if (inChannelCreateParams.UserIds == null)
                inChannelCreateParams.UserIds = new List<string>();

            string currentUserId = chatMainContextRef.CurrentUserId;
            if (inChannelCreateParams.UserIds.Contains(currentUserId) == false)
                inChannelCreateParams.UserIds.Add(currentUserId);

            CreateGroupChannelApiCommand.Request createOpenChannelApiCommand = new CreateGroupChannelApiCommand.Request(inChannelCreateParams, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(createOpenChannelApiCommand);
        }

        internal void GetChannel(string inChannelUrl, bool inIsInternal = false, bool inIsForceRefresh = false, SbGetGroupChannelHandler inCompletionHandler = null)
        {
            if (string.IsNullOrEmpty(inChannelUrl))
            {
                if (inCompletionHandler != null)
                {
                    SbError error = SbErrorCodeExtension.CreateInvalidParameterError("ChannelUrl");
                    CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler.Invoke(null, false, error); });
                }

                return;
            }

            if (inIsForceRefresh == false && cachedChannelsByUrl.TryGetValue(inChannelUrl, out SbGroupChannel cachedChannel))
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler.Invoke(cachedChannel, true, null); });
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

                if (inResponse is GetGroupChannelApiCommand.Response groupChannelResponse && groupChannelResponse.GroupChannelDto != null)
                {
                    SbGroupChannel responseOpenChannel = CreateOrUpdateChannel(groupChannelResponse.GroupChannelDto);
                    inCompletionHandler?.Invoke(responseOpenChannel, false, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, false, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            GetGroupChannelApiCommand.Request getOpenChannelApiCommand = new GetGroupChannelApiCommand.Request(inChannelUrl, inIsInternal, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(getOpenChannelApiCommand);
        }

        protected override SbGroupChannel CreateChannelInstance(string inChannelUrl)
        {
            return new SbGroupChannel(inChannelUrl, chatMainContextRef);
        }

        private void RemoveInvalidateTypingStatuses()
        {
            foreach (SbGroupChannel groupChannel in cachedChannelsByUrl.Values)
            {
                bool removed = groupChannel.RemoveInvalidateTypingStatus();
                if (removed)
                {
                    channelHandlersById.ForEachByValue(inGroupHandler => { inGroupHandler.OnTypingStatusUpdated?.Invoke(groupChannel); });
                    chatMainContextRef.CollectionManager.OnTypingStatusUpdated(groupChannel);
                }
            }
        }

        internal void MarkAsReadWithChannelUrls(List<string> inChannelUrls, SbErrorHandler inCompletionHandler)
        {
            long now = TimeUtil.GetCurrentUnixTimeMilliseconds();
            if (now - _lastMarkAsReadAllTimestampMs < 1000)
            {
                if (inCompletionHandler != null)
                {
                    CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler.Invoke(SbErrorCodeExtension.MARK_AS_READ_RATE_LIMIT_EXCEEDED_ERROR); });
                }

                return;
            }

            _lastMarkAsReadAllTimestampMs = now;
            List<string> channelUrls = null;
            if (inChannelUrls != null && 0 < inChannelUrls.Count)
                channelUrls = new List<string>(inChannelUrls);

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inError == null)
                {
                    if (channelUrls != null)
                    {
                        foreach (string channelUrl in channelUrls)
                        {
                            SbGroupChannel channel = FindCachedChannel(channelUrl);
                            channel?.ClearAllUnreadCount();
                        }
                    }
                    else
                    {
                        foreach (SbGroupChannel channel in cachedChannelsByUrl.Values)
                        {
                            channel?.ClearAllUnreadCount();
                        }
                    }
                }

                inCompletionHandler?.Invoke(inError);
            }


            MarkAsReadChannelsApiCommand.Request getOpenChannelApiCommand = new MarkAsReadChannelsApiCommand.Request(chatMainContextRef.CurrentUserId, channelUrls, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(getOpenChannelApiCommand);
        }

        internal void MarkAsDelivered(Dictionary<string, string> inData, SbErrorHandler inCompletionHandler)
        {
            if (inData == null || inData.Count <= 0 || inData.TryGetValue("sendbird", out string sendbirdJsonString) == false)
            {
                SbError invalidParameterError = SbErrorCodeExtension.CreateInvalidParameterError("Data");
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(invalidParameterError); });
                return;
            }

            long messageId = SbBaseMessage.INVALID_MESSAGE_ID_MIN;
            string channelUrl = null;
            string sessionKey = null;
            JObject sendbirdJObject = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<JObject>(sendbirdJsonString);
            if (sendbirdJObject != null)
            {
                messageId = sendbirdJObject.ToPropertyValueIgnoreException<long>("message_id");
                JObject channelJObject = sendbirdJObject.ToPropertyValueIgnoreException<JObject>("channel");
                if (channelJObject != null)
                {
                    channelUrl = channelJObject.ToPropertyValueIgnoreException<string>("channel_url");
                }

                JObject sessionKeyJObject = sendbirdJObject.ToPropertyValueIgnoreException<JObject>("session_key");
                if (sessionKeyJObject != null)
                {
                    sessionKey = sessionKeyJObject.ToPropertyValueIgnoreException<string>("key");
                }
            }

            if (messageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN || string.IsNullOrEmpty(channelUrl))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inCompletionHandler?.Invoke(SbErrorCodeExtension.MALFORMED_DATA_ERROR); });
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                inCompletionHandler?.Invoke(inError);
            }


            MarkAsDeliveredGroupChannelApiCommand.Request getOpenChannelApiCommand = new MarkAsDeliveredGroupChannelApiCommand.Request(
                channelUrl, messageId, sessionKey, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(getOpenChannelApiCommand);
        }

        internal void GetGroupChannelCount(SbMyMemberStateFilter inMyMemberStateFilter, SbCountHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(0, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetGroupChannelCountApiCommand.Response response)
                {
                    inCompletionHandler?.Invoke(response.groupChannelCount, null);
                }
                else
                {
                    if (inError != null)
                        inError = SbErrorCodeExtension.MALFORMED_DATA_ERROR;

                    inCompletionHandler?.Invoke(0, inError);
                }
            }

            GetGroupChannelCountApiCommand.Request apiCommand = new GetGroupChannelCountApiCommand.Request(chatMainContextRef.CurrentUserId, inMyMemberStateFilter, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }

        internal void GetMyGroupChannelChangeLogs(long? inTimestamp, string inToken, SbGroupChannelChangeLogsParams inParams, SbGroupChannelChangeLogsHandler inCompletionHandler)
        {
            if (string.IsNullOrEmpty(chatMainContextRef.CurrentUserId))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, null, false, null, SbErrorCodeExtension.CONNECTION_REQUIRED_ERROR));
                return;
            }

            void OnCompletionHandler(ApiCommandAbstract.Response inResponse, SbError inError, bool inIsCanceled)
            {
                if (inResponse is GetMyGroupChannelChangeLogsApiCommand.Response response)
                {
                    List<SbGroupChannel> updatedGroupChannels = null;
                    if (response.updatedGroupChannelDtos != null && 0 < response.updatedGroupChannelDtos.Count)
                    {
                        updatedGroupChannels = new List<SbGroupChannel>(response.updatedGroupChannelDtos.Count);
                        foreach (GroupChannelDto groupChannelDto in response.updatedGroupChannelDtos)
                        {
                            SbGroupChannel groupChannel = CreateOrUpdateChannel(groupChannelDto);
                            updatedGroupChannels.Add(groupChannel);
                        }
                    }

                    inCompletionHandler?.Invoke(updatedGroupChannels, response.deletedChannelUrls, response.hasMore, response.token, null);
                }
                else
                {
                    if (inError != null)
                        inError = SbErrorCodeExtension.MALFORMED_DATA_ERROR;

                    inCompletionHandler?.Invoke(null, null, false, null, inError);
                }
            }

            const int CHANGE_LOGS_LIMIT = 100;
            GetMyGroupChannelChangeLogsApiCommand.Request apiCommand = new GetMyGroupChannelChangeLogsApiCommand.Request(
                chatMainContextRef.CurrentUserId, inToken, inTimestamp, inParams, CHANGE_LOGS_LIMIT, OnCompletionHandler);
            chatMainContextRef.CommandRouter.RequestApiCommand(apiCommand);
        }
    }
}