// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class UserEventWsReceiveCommand : WsReceiveCommandAbstract
    {
        internal enum CategoryType
        {
            Unblock = 20000,
            Block = 20001,
            RoleChange = 20100,
            FriendDiscoveryReady = 20900
        }

        [JsonProperty("cat")] private readonly int _category;
        internal UserEventDataAbstract EventData { get; private set; }
        internal UserEventWsReceiveCommand() : base(WsCommandType.UserEvent) { }

        private static UserEventDataAbstract DeserializeEventData(CategoryType inCategoryType, string inJsonString)
        {
            if (inJsonString == null)
                return null;

            switch (inCategoryType)
            {
                case CategoryType.Unblock:              return UnblockUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.Block:                return BlockUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.RoleChange:           return RoleChangeUserEventData.DeserializeFromJson(inJsonString);
                case CategoryType.FriendDiscoveryReady: return FriendDiscoveryReadyUserEventData.DeserializeFromJson(inJsonString);
                default:
                {
                    Logger.Warning(Logger.CategoryType.Command, $"UserEventWsEventCommand::DeserializeEventData Invalid event category type:{inCategoryType}");
                    return null;
                }
            }
        }

        internal static UserEventWsReceiveCommand DeserializeFromJson(string inJsonString)
        {
            UserEventWsReceiveCommand eventCommand = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<UserEventWsReceiveCommand>(inJsonString);
            if (eventCommand != null)
            {
                eventCommand.EventData = DeserializeEventData((CategoryType)eventCommand._category, inJsonString);
            }

            return eventCommand;
        }
    }
}