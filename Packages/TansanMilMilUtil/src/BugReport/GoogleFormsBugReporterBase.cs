using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public abstract class GoogleFormsBugReporterBase : BugReporter
    {
        public override BugReportInfo GenerateInfo()
        {
            string osAndPlatform = SystemInfo.operatingSystem + " - " + Application.platform.ToString();
            string gameVersion = Application.version;
            return new BugReportInfo(osAndPlatform, gameVersion);
        }

        public override void ReportBug(BugReportInfo info)
        {
            string url = GenerateUrl(info);
            Application.OpenURL(url);
        }

        private string GenerateUrl(BugReportInfo info)
        {
            List<string> queries = GetUrlQueries();

            return $"{GetGoogleFormsUrl()}&" + string.Join("&", queries);
        }

        protected abstract List<string> GetUrlQueries();

        protected abstract string GetGoogleFormsUrl();
    }
}
