using System;

namespace TansanMilMil.Util
{
    public class ReplaceBackSlashNToNewLineStrategy : TextReplaceStrategy
    {
        public override string Replace(string text)
        {
            string backSlashNPattern = @"\n";
            if (text.Contains(backSlashNPattern))
            {
                text = text.Replace(backSlashNPattern, Environment.NewLine);
            }

            return text;
        }
    }
}
