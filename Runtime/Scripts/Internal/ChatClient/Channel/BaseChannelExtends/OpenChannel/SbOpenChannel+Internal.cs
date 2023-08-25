// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;

namespace Sendbird.Chat
{
    public partial class SbOpenChannel
    {
        private readonly List<SbUser> _operators = new List<SbUser>();
        private int _participantCount;

        internal SbOpenChannel(string inChannelUrl, SendbirdChatMainContext inChatMainContext) : base(inChannelUrl, inChatMainContext) { }

        private protected override void OnResetFromChannelDto(BaseChannelDto inBaseChannelDto)
        {
            if (!(inBaseChannelDto is OpenChannelDto openChannelDto))
            {
                Logger.Warning(Logger.CategoryType.Channel, "SbOpenChannel::OnResetFromChannelDto() Command object is null.");
                return;
            }

            _participantCount = openChannelDto.participantCount;

            _operators.Clear();
            if (openChannelDto.operatorUserDtos != null && 0 < openChannelDto.operatorUserDtos.Count)
            {
                foreach (UserDto userDto in openChannelDto.operatorUserDtos)
                {
                    if (userDto == null)
                        continue;

                    SbUser user = new SbUser(userDto, chatMainContextRef);
                    _operators.Add(user);
                }
            }
        }

        protected override SbRole GetCurrentUserRole()
        {
            if (IsOperator(chatMainContextRef.CurrentUserRef))
                return SbRole.Operator;

            return SbRole.None;
        }

        private void EnterInternal(SbErrorHandler inCompletionHandler)
        {
            void OnAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inSbError)
            {
                if (inSbError != null)
                {
                    chatMainContextRef.OpenChannelManager.RemoveEnteredChannelIfContains(Url);
                    inCompletionHandler?.Invoke(inSbError);
                    return;
                }
                
                if (inWsReceiveCommand is EnterChannelWsReceiveCommand enterChannelWsReceiveCommand)
                {
                    _participantCount = enterChannelWsReceiveCommand.participantCount;
                }

                chatMainContextRef.OpenChannelManager.AddEnteredChannelIfNotContains(this);
                inCompletionHandler?.Invoke(null);
            }

            EnterChannelWsSendCommand enterChannelWsSendCommand = new EnterChannelWsSendCommand(Url, OnAck);
            chatMainContextRef.CommandRouter.SendWsCommand(enterChannelWsSendCommand);
        }

        private void ExitInternal(SbErrorHandler inCompletionHandler)
        {
            void OnAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inSbError)
            {
                if (inSbError != null)
                {
                    inCompletionHandler?.Invoke(inSbError);
                    return;
                }
                
                if (inWsReceiveCommand is ExitChannelWsReceiveCommand exitChannelWsEventCommand)
                {
                    _participantCount = exitChannelWsEventCommand.participantCount;
                }

                inCompletionHandler?.Invoke(null);
                chatMainContextRef.OpenChannelManager.RemoveEnteredChannelIfContains(Url);
            }

            ExitChannelWsSendCommand exitChannelWsSendCommand = new ExitChannelWsSendCommand(Url, OnAck);
            chatMainContextRef.CommandRouter.SendWsCommand(exitChannelWsSendCommand);
        }

        private void RefreshInternal(SbErrorHandler inCompletionHandler)
        {
            void OnCompletionHandler(SbOpenChannel inChannel, bool inCache, SbError inError)
            {
                inCompletionHandler?.Invoke(inError);
            }

            chatMainContextRef.OpenChannelManager.GetChannel(Url, inIsInternal: false, inIsForceRefresh: true, OnCompletionHandler);
        }

        private bool IsOperatorInternal(string inUserId)
        {
            if (Operators == null || string.IsNullOrEmpty(inUserId))
                return false;

            return Operators.Any(inUser => inUser.UserId == inUserId);
        }

        internal void SetOperators(List<SbUser> inOperatorUsers)
        {
            _operators.Clear();
            if (inOperatorUsers != null)
                _operators.AddRange(inOperatorUsers);
        }

        internal SbParticipantListQuery CreateParticipantListQueryInternal(SbParticipantListQueryParams inParams)
        {
            return new SbParticipantListQuery(Url, inParams, chatMainContextRef);
        }
    }
}