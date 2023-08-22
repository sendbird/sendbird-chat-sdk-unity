// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.IO;

namespace Sendbird.Chat
{
    public partial class SbFileInfo
    {
        internal bool IsExists()
        {
            if (string.IsNullOrEmpty(FullPath))
                return false;

            return File.Exists(FullPath);
        }

        internal string GetName()
        {
            if (string.IsNullOrEmpty(FullPath))
                return string.Empty;

            return System.IO.Path.GetFileName(FullPath);
        }
    }
}