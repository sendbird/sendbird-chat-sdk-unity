// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    [Serializable]
    internal class RestrictedUserDto : UserDto
    {
#pragma warning disable CS0649
        [JsonProperty("description")] private readonly string _description;
        [JsonProperty("muted_description")] private readonly string _mutedDescription;
        [JsonProperty("end_at")] private readonly long? _endAt = null;
        //[JsonProperty("muted_start_at")] private readonly long? _mutedStartAt = null;
        [JsonProperty("muted_end_at")] private readonly long? _mutedEndAt = null;
        [JsonProperty("remaining_duration")] private readonly long? _remainingDuration;
        [JsonProperty("restriction_type")] private readonly string _restrictionType;
#pragma warning restore CS0649

        internal string Description { get; private set; }
        internal long EndAt { get; private set; }
        internal long RemainingDuration { get; private set; }
        internal SbRestrictionType RestrictionType { get; set; } = SbRestrictionType.Muted;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext inStreamingContext)
        {
            if (string.IsNullOrEmpty(_restrictionType) == false)
                RestrictionType = SbRestrictionTypeExtension.JsonNameToType(_restrictionType);

            if (string.IsNullOrEmpty(_description) == false)
            {
                Description = _description;
            }
            else
            {
                Description = _mutedDescription;
            }

            if (_endAt != null)
            {
                EndAt = _endAt.Value;
            }
            else if (_mutedEndAt != null)
            {
                EndAt = _mutedEndAt.Value;
            }

            if (_remainingDuration != null)
            {
                RemainingDuration = _remainingDuration.Value;
            }
            else
            {
                RemainingDuration = -1;
            }
        }
    }
}