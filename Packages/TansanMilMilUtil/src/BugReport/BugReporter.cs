namespace TansanMilMil.Util
{
    public abstract class BugReporter
    {
        public abstract void ReportBug(BugReportInfo info);

        public abstract BugReportInfo GenerateInfo();
    }
}
