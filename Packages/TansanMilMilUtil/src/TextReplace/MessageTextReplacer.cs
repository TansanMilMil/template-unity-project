using System;
using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public class MessageTextReplacer
    {
        public event Func<string, string> replaceText;

        public MessageTextReplacer(IEnumerable<TextReplaceStrategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                replaceText += strategy.Replace;
            }
        }

        public string Replace(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || replaceText == null)
                return "";

            text = replaceText.Invoke(text);

            return text;
        }
    }
}
