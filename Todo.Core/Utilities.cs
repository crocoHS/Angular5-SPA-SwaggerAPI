using System;
using System.Net;

namespace Todo.Core
{
    public class Utilities
    {
        public static string GetPageSource(string pageURL)
        {
            string strSource = null;

            using (var webClient = new WebClient())
            {
                strSource = webClient.DownloadString(pageURL);
            }

            return strSource;
        }

        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var rnd = new Random();
            var range = to - from;
            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;

        }
    }
}
