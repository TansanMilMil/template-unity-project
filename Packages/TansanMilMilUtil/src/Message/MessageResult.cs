namespace TansanMilMil.Util
{
    public class MessageResult
    {
        public MessageFrameBase frame { get; set; }
        public int selectedChoiceIndex { get; set; } = -1;

        public MessageResult(MessageFrameBase frame, int selectedChoiceIndex)
        {
            this.frame = frame;
            this.selectedChoiceIndex = selectedChoiceIndex;
        }
    }
}
