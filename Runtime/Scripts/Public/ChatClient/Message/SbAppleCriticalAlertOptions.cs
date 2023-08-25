// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// A class that can be used to send apple's critical alert.
    /// </summary>
    /// @since 4.0.0
    public partial class SbAppleCriticalAlertOptions
    {
        /// <summary>
        /// The name of a sound file in the iOS app. The default value is default.
        /// </summary>
        /// @since 4.0.0
        public string Name => _name;

        /// <summary>
        /// The volume for the critical alert’s sound. Set this to a value between 0.0 (silent) and 1.0 (full volume). The default value is 1.0.
        /// </summary>
        /// @since 4.0.0
        public double Volume => _volume;
        
        /// @since 4.0.0
        public SbAppleCriticalAlertOptions(string inName = DEFAULT_NAME, double inVolume = DEFAULT_VOLUME)
        {
            _name = inName;
            _volume = inVolume;
        }
        
        /// @since 4.0.0
        public SbAppleCriticalAlertOptions(double inVolume = DEFAULT_VOLUME)
        {
            _name = DEFAULT_NAME;
            _volume = inVolume;
        }
    }
}