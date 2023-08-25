// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Text;

namespace Sendbird.Chat.Sample
{
    public static class StringUtil
    {
        private static string ApplyEllipsis(string inString, int inMaxLength)
        {
            const string ELLIPSIS = "...";
            if (inMaxLength <= inString.Length)
            {
                inString = inString.Substring(0, inMaxLength - ELLIPSIS.Length) + ELLIPSIS;
            }

            return inString;
        }

        public static string GetGroupChannelName(SbGroupChannel inGroupChannel, int inMaxLength)
        {
            if (inGroupChannel == null)
                return string.Empty;

            string channelName = string.Empty;
            if (string.IsNullOrEmpty(inGroupChannel.Name) == false)
            {
                channelName = inGroupChannel.Name;
            }
            else if (inGroupChannel.Members != null && 0 < inGroupChannel.Members.Count)
            {
                StringBuilder nameStringBuilder = new StringBuilder();
                foreach (SbMember groupChannelMember in inGroupChannel.Members)
                {
                    if (0 < nameStringBuilder.Length)
                        nameStringBuilder.Append(", ");

                    nameStringBuilder.Append(groupChannelMember.UserId);
                }

                channelName = nameStringBuilder.ToString();
            }

            return StringUtil.ApplyEllipsis(channelName, inMaxLength);
        }
        
        public static string GetOpenChannelName(SbOpenChannel inOpenChannel, int inMaxLength)
        {
            if (inOpenChannel == null)
                return string.Empty;

            string channelName = string.Empty;
            if (string.IsNullOrEmpty(inOpenChannel.Name) == false)
            {
                channelName = inOpenChannel.Name;
            }
            else if (string.IsNullOrEmpty(inOpenChannel.Url) == false)
            {
                channelName = inOpenChannel.Url;
            }

            return StringUtil.ApplyEllipsis(channelName, inMaxLength);
        }
    }
}