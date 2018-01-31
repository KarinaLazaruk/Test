using System;

namespace Web.Helpers
{
    public static class DateTimeHelpers
    {
        private static DateTime _mEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string FromTimestamp(this long timestamp)
        {
            var time = _mEpoch.AddMilliseconds(timestamp);
            return time.ToString("d");
        }
    }
}