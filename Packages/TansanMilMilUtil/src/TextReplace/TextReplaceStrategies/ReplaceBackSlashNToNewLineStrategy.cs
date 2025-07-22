using System;

namespace TansanMilMil.Util
{
    internal class ReplaceBackSlashNToNewLineStrategy : TextReplaceStrategy
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
