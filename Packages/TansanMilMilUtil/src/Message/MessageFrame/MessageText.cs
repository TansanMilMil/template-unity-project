using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public class MessageText
    {
        public string text { get; set; }
        public string talkerName { get; set; }
        public List<string> choices { get; set; } = new List<string>();
    }
}