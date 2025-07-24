namespace TansanMilMil.Util
{
    /// <summary>
    /// ポーズ時に呼び出される処理を定義する際にはこのインターフェースを実装してください。
    /// </summary>
    public interface IPauseEvents
    {
        void OnPause();

        void OnResume();
    }
}
