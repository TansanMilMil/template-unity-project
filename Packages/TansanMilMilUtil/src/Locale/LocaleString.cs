using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TansanMilMil.Util
{
    public class LocaleString
    {
        public TableReferenceType tableReference { get; private set; }
        public string key { get; private set; }
        public Dictionary<Regex, string> regexList { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tableReference"></param>
        /// <param name="regexList">メッセージ内に埋め込みたい文字列があれば正規表現で指定する</param>
        public LocaleString(string key, TableReferenceType tableReference, Dictionary<Regex, string> regexList = null)
        {
            this.tableReference = tableReference;
            this.key = key;
            this.regexList = regexList;
        }

        public string ReplaceTextByRegex(string text)
        {
            if (regexList == null)
                return text;
            foreach (KeyValuePair<Regex, string> regex in regexList)
            {
                text = regex.Key.Replace(text, regex.Value);
            }
            return text;
        }
    }
}
