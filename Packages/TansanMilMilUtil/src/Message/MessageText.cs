using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TansanMilMil.Util
{
    public class MessageText
    {
        public string text { get; set; } = "";
        public string talkerName { get; set; } = "";
        public IList<string> choices { get; set; } = new List<string>();
        /// <summary>
        /// textの置き換えロジックを実装したコンポーネントのリスト
        /// </summary>
        private readonly IReadOnlyCollection<TextReplaceStrategy> DefaultReplaceStrategies = new ReadOnlyCollection<TextReplaceStrategy>(
            new List<TextReplaceStrategy>()
            {
                new ReplaceBackSlashNToNewLineStrategy()
            });

        public MessageText(string text, string talkerName = null, IList<string> choices = null)
        {
            IEnumerable<TextReplaceStrategy> strategies = DefaultReplaceStrategies.ToList();

            if (choices != null)
            {
                this.choices = choices;
                for (int i = 0; i < this.choices.Count; i++)
                {
                    this.choices[i] = ReplaceTexts(this.choices[i], strategies);
                }
            }

            if (text != null)
            {
                this.text = ReplaceTexts(text, strategies);
            }

            if (talkerName != null)
            {
                this.talkerName = ReplaceTexts(talkerName, strategies);
            }
        }

        private string ReplaceTexts(string text, IEnumerable<TextReplaceStrategy> strategies)
        {
            if (!strategies.Any())
            {
                return text;
            }

            var replacer = new MessageTextReplacer(strategies);
            return replacer.Replace(text);
        }
    }
}
