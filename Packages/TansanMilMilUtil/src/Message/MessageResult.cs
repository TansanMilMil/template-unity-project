namespace TansanMilMil.Util
{
    public class MessageResult
    {
        public int selectedChoiceIndex { get; set; } = -1;

        public MessageResult(int selectedChoiceIndex)
        {
            this.selectedChoiceIndex = selectedChoiceIndex;
        }
    }
}