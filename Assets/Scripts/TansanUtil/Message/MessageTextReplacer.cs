using System;

namespace TansanMilMil.Util
{
    public class MessageTextReplacer
    {
        public event Func<string, string> replaceText;

        public MessageTextReplacer(IMessageTextReplacerLogic replacerLogic)
        {
            replaceText += replacerLogic.ReplaceText;
        }

        public string Replace(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            text = ReplaceBackSlashNToNewLine(text);
            text = replaceText.Invoke(text);

            return text;
        }

        private string ReplaceBackSlashNToNewLine(string text)
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