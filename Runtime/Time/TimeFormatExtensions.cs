using System;

namespace Playrika.GameFoundation.Time
{
    public static class TimeFormatExtensions
    {
        public static string ConvertToStringFormat(this TimeFormat timeFormat)
        {
            switch (timeFormat)
            {
                case TimeFormat.HoursMinutesSeconds:
                    return @"hh\:mm\:ss";
                case TimeFormat.MinutesSeconds:
                    return @"mm\:ss";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}