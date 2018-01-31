using System;

namespace Web.Helpers
{
    public static class DateTimeHelpers
    {
        private static DateTime _mEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromTimestamp(this long timestamp)
        {
            return _mEpoch.AddMilliseconds(timestamp);
        }
    }
}