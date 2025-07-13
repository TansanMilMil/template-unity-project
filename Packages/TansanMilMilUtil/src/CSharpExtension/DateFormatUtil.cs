using System;

namespace TansanMilMil.Util
{
    public class DateFormatUtil
    {
        public static string ToISOString(DateTimeOffset datetime)
        {
            return datetime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }

        public static string GetUtcNowString()
        {
            return DateTimeOffset.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }

        public static string ToPlayTimeFormat(TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}