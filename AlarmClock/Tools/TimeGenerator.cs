using System.Collections.Generic;

namespace AlarmClock.Tools
{
    internal static class TimeGenerator
    {
        public static string GetFormattedTime(int time)
        {
            return time < 10 ? $"0{time}" : $"{time}"; ;
        }

        public static List<string> GenerateHours()
        {
            List<string> hours = new List<string>();
            for (int i = 0; i < 24; ++i)
            {
                hours.Add(GetFormattedTime(i));
            }

            return hours;
        }

        public static List<string> GenerateMinutes()
        {
            List<string> minutes = new List<string>();
            for (int i = 0; i < 60; ++i)
            {
                minutes.Add(GetFormattedTime(i));
            }

            return minutes;
        }

    }
}
