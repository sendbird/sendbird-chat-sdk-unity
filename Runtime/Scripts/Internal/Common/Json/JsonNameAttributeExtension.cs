// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Reflection;

namespace Sendbird.Chat
{
    internal static class JsonNameAttributeExtension
    {
        internal static string ToJsonName(this Enum inEnumValue)
        {
            Type enumType = inEnumValue.GetType();
            string enumName = Enum.GetName(enumType, inEnumValue);
            FieldInfo fieldInfo = enumType.GetField(enumName);
            if (fieldInfo != null)
            {
                JsonNameAttribute attribute = fieldInfo.GetCustomAttribute<JsonNameAttribute>();
                if (attribute != null && attribute.JsonName != null)
                    return attribute.JsonName;
            }

            return enumName;
        }
    }
}