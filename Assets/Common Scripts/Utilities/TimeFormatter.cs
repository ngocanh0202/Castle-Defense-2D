using System;

namespace Utilities
{
    public class TimeFormatter
    {
        public static string FormatCountdown(double seconds, CountdownOptions option)
        {
            return option switch
            {
                CountdownOptions.Seconds => FormatAsSeconds(seconds),
                CountdownOptions.Minus => FormatAsMinus(seconds),
                CountdownOptions.Decimal => FormatAsDecimal(seconds),
                _ => throw new ArgumentException($"Invalid CountdownOptions: {option}")
            };
        }

        private static string FormatAsSeconds(double seconds)
        {
            return Math.Max(0, Math.Ceiling(seconds)).ToString();
        }

        private static string FormatAsMinus(double seconds)
        {
            var clampedSeconds = Math.Max(0, seconds);
            var timeSpan = TimeSpan.FromSeconds(clampedSeconds);

            var minutes = (int)timeSpan.TotalMinutes;
            var remainingSeconds = timeSpan.Seconds;

            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        private static string FormatAsDecimal(double seconds)
        {
            return Math.Max(0, seconds).ToString("F2");
        }
    }
}

