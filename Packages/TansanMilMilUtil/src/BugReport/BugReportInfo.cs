namespace TansanMilMil.Util
{
    public class BugReportInfo
    {
        public string osAndPlatform { get; private set; }
        public string gameVersion { get; private set; }

        public BugReportInfo(string osAndPlatform, string gameVersion)
        {
            this.osAndPlatform = osAndPlatform;
            this.gameVersion = gameVersion;
        }
    }
}
