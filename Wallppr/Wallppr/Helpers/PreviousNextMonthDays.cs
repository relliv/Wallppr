using System;

namespace Wallppr.Helpers
{
    public static class PreviousNextMonthDays
    {
        /// <summary>
        /// Previous month's last days countdown
        /// </summary>
        /// <param name="dayOfWeek">Day of week of first day of the current month.</param>
        /// <returns></returns>
        public static int GetPreviousCountdown(DayOfWeek dayOfWeek)
        {
            var previousCoundown = 7;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    previousCoundown = 7;
                    break;
                case DayOfWeek.Tuesday:
                    previousCoundown = 1;
                    break;
                case DayOfWeek.Wednesday:
                    previousCoundown = 2;
                    break;
                case DayOfWeek.Thursday:
                    previousCoundown = 3;
                    break;
                case DayOfWeek.Friday:
                    previousCoundown = 4;
                    break;
                case DayOfWeek.Saturday:
                    previousCoundown = 5;
                    break;
                case DayOfWeek.Sunday:
                    previousCoundown = 6;
                    break;
                default:
                    previousCoundown = 7;
                    break;
            }

            return previousCoundown;
        }

        /// <summary>
        /// Next month's first days countdown
        /// </summary>
        /// <param name="dayOfWeek">Day of week of last day of the current month.</param>
        /// <returns></returns>
        public static int GetNextCountdown(DayOfWeek dayOfWeek)
        {
            var nextCoundown = 7;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    nextCoundown = 6;
                    break;
                case DayOfWeek.Tuesday:
                    nextCoundown = 5;
                    break;
                case DayOfWeek.Wednesday:
                    nextCoundown = 4;
                    break;
                case DayOfWeek.Thursday:
                    nextCoundown = 3;
                    break;
                case DayOfWeek.Friday:
                    nextCoundown = 2;
                    break;
                case DayOfWeek.Saturday:
                    nextCoundown = 1;
                    break;
                case DayOfWeek.Sunday:
                    nextCoundown = 7;
                    break;
                default:
                    nextCoundown = 7;
                    break;
            }

            return nextCoundown;
        }
    }
}