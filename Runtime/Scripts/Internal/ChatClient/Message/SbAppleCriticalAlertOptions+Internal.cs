// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbAppleCriticalAlertOptions
    {
        private const string DEFAULT_NAME = "default";
        private const double DEFAULT_VOLUME = 1.0;

        private readonly string _name;
        private readonly double _volume;

        private SbAppleCriticalAlertOptions(SbAppleCriticalAlertOptions inAppleCriticalAlertOptions)
        {
            if (inAppleCriticalAlertOptions != null)
            {
                _name = inAppleCriticalAlertOptions._name;
                _volume = inAppleCriticalAlertOptions._volume;
            }
        }

        internal SbAppleCriticalAlertOptions(AppleCriticalAlertOptionsDto inAppleCriticalAlertOptionsDto)
        {
            if (inAppleCriticalAlertOptionsDto != null)
            {
                _name = inAppleCriticalAlertOptionsDto.name;
                _volume = inAppleCriticalAlertOptionsDto.volume;
            }
        }

        internal SbAppleCriticalAlertOptions Clone()
        {
            return new SbAppleCriticalAlertOptions(this);
        }
    }
}