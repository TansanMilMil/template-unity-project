using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TansanMilMil.Util
{
    public class LocaleString
    {
        public TableReferenceType tableReference { get; private set; }
        public string key { get; private set; }

        public LocaleString(string key, TableReferenceType tableReference)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            this.tableReference = tableReference;
            this.key = key;
        }
    }
}
