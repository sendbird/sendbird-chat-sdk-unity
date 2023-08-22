// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Object representing a file path.
    /// </summary>
    /// @since 4.0.0
    public partial class SbFileInfo
    {
        /// <summary>
        /// Represents the fully qualified absolute path of the file.
        /// </summary>
        /// @since 4.0.0
        public string FullPath { get; set; }

        /// <summary>
        /// Create error SbFileInfo with absolute path.
        /// </summary>
        /// <param name="inFullPath"></param>
        /// @since 4.0.0
        public SbFileInfo(string inFullPath)
        {
            FullPath = inFullPath;
        }
    }
}