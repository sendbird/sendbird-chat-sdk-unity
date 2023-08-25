// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private void CreateMessageMetaArrayKeysInternal(SbBaseMessage inMessage, List<string> inKeys, SbBaseMessageHandler inCompletionHandler)
        {
            List<MessageMetaArrayDto> metaArrayDtos = null;
            if (inKeys != null && 0 < inKeys.Count)
            {
                metaArrayDtos = new List<MessageMetaArrayDto>(inKeys.Count);
                foreach (string key in inKeys)
                {
                    metaArrayDtos.Add(new MessageMetaArrayDto(key, null));
                }
            }

            UpdateMessageMetaArrayKeys(inMessage, metaArrayDtos, MessageMetaArrayUpdateDto.Mode.Add, inCompletionHandler);
        }

        private void DeleteMessageMetaArrayKeysInternal(SbBaseMessage inMessage, List<string> inKeys, SbBaseMessageHandler inCompletionHandler)
        {
            List<MessageMetaArrayDto> metaArrayDtos = null;
            if (inKeys != null && 0 < inKeys.Count)
            {
                metaArrayDtos = new List<MessageMetaArrayDto>(inKeys.Count);
                foreach (string key in inKeys)
                {
                    metaArrayDtos.Add(new MessageMetaArrayDto(key, null));
                }
            }

            UpdateMessageMetaArrayKeys(inMessage, metaArrayDtos, MessageMetaArrayUpdateDto.Mode.Remove, inCompletionHandler);
        }
        
        private void AddMessageMetaArrayValuesInternal(SbBaseMessage inMessage, List<SbMessageMetaArray> inMetaArrays, SbBaseMessageHandler inCompletionHandler)
        {
            List<MessageMetaArrayDto> metaArrayDtos = null;
            if (inMetaArrays != null && 0 < inMetaArrays.Count)
            {
                metaArrayDtos = new List<MessageMetaArrayDto>(inMetaArrays.Count);
                foreach (SbMessageMetaArray metaArray in inMetaArrays)
                {
                    metaArrayDtos.Add(new MessageMetaArrayDto(metaArray.Key, metaArray.ValueInternal));
                }
            }

            UpdateMessageMetaArrayKeys(inMessage, metaArrayDtos, MessageMetaArrayUpdateDto.Mode.Add, inCompletionHandler);
        }
        
        private void RemoveMessageMetaArrayValuesInternal(SbBaseMessage inMessage, List<SbMessageMetaArray> inMetaArrays, SbBaseMessageHandler inCompletionHandler)
        {
            List<MessageMetaArrayDto> metaArrayDtos = null;
            if (inMetaArrays != null && 0 < inMetaArrays.Count)
            {
                metaArrayDtos = new List<MessageMetaArrayDto>(inMetaArrays.Count);
                foreach (SbMessageMetaArray metaArray in inMetaArrays)
                {
                    metaArrayDtos.Add(new MessageMetaArrayDto(metaArray.Key, metaArray.ValueInternal));
                }
            }

            UpdateMessageMetaArrayKeys(inMessage, metaArrayDtos, MessageMetaArrayUpdateDto.Mode.Remove, inCompletionHandler);
        }

        private void UpdateMessageMetaArrayKeys(SbBaseMessage inMessage, List<MessageMetaArrayDto> inMetaArrayDtos, MessageMetaArrayUpdateDto.Mode inUpdateMode, SbBaseMessageHandler inCompletionHandler)
        {
            if (inMessage == null || inMessage.MessageId <= SbBaseMessage.INVALID_MESSAGE_ID_MIN || inMetaArrayDtos == null || inMetaArrayDtos.Count <= 0)
            {
                SbError error = SbErrorCodeExtension.CreateInvalidParameterError("Message");
                if (inMetaArrayDtos == null || inMetaArrayDtos.Count <= 0)
                {
                    error = SbErrorCodeExtension.CreateInvalidParameterError("Keys or Values");
                }

                CoroutineManager.Instance.CallOnNextFrame(() => inCompletionHandler?.Invoke(null, error));
                return;
            }


            void OnUpdateMessageWsCommandAck(WsReceiveCommandAbstract inWsReceiveCommand, SbError inError)
            {
                if (inError != null)
                {
                    inCompletionHandler?.Invoke(null, inError);
                    return;
                }

                SbBaseMessage message = null;
                if (inWsReceiveCommand is UpdateMessageWsReceiveCommandAbstract messageReceiveCommand && messageReceiveCommand.BaseMessageDto != null)
                {
                    message = messageReceiveCommand.BaseMessageDto.CreateMessageInstance(chatMainContextRef);
                }

                if (message != null)
                {
                    inCompletionHandler?.Invoke(message, null);
                }
                else
                {
                    inCompletionHandler?.Invoke(null, SbErrorCodeExtension.MALFORMED_DATA_ERROR);
                }
            }

            MessageMetaArrayUpdateDto updateDto = new MessageMetaArrayUpdateDto(inMetaArrayDtos, inUpdateMode);
            UpdateMessageWsSendCommandAbstract sendCommand = null;
            if (inMessage is SbUserMessage)
            {
                sendCommand = new UpdateUserMessageWsSendCommand(Url, inMessage.MessageId, updateDto, OnUpdateMessageWsCommandAck);
            }

            if (inMessage is SbFileMessage)
            {
                sendCommand = new UpdateFileMessageWsSendCommand(Url, inMessage.MessageId, updateDto, OnUpdateMessageWsCommandAck);
            }

            chatMainContextRef.CommandRouter.SendWsCommand(sendCommand);
        }
    }
}