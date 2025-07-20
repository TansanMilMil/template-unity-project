using System;
using System.Collections.Generic;
using System.Linq;

namespace TansanMilMil.Util {
    [Serializable]
    public class ParsedKeyValue<TKey, TValue> {
        public TKey key;
        public TValue value;

        public ParsedKeyValue(TKey key, TValue value) {
            this.key = key;
            this.value = value;
        }
    }

    public static class ListParser<TKey, TValue> {
        public static IList<ParsedKeyValue<TKey, TValue>> Parse(Dictionary<TKey, TValue> dic) {
            var parsedList = new List<ParsedKeyValue<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> kvp in dic) {
                parsedList.Add(new ParsedKeyValue<TKey, TValue>(kvp.Key, kvp.Value));
            }
            return parsedList;
        }

        public static Dictionary<TKey, TValue> RetrieveDictionary(IEnumerable<ParsedKeyValue<TKey, TValue>> list) {
            var dic = new Dictionary<TKey, TValue>();
            foreach (ParsedKeyValue<TKey, TValue> kv in list) {
                dic.Add(kv.key, kv.value);
            }
            return dic;
        }
    }
}