namespace TansanMilMil.Util
{
    /// <summary>
    /// 頻繁に利用するComponentはキャッシュを常に保持していた方がパフォーマンスが良いのでこのinterfaceを実装してね
    /// このinterfaceを実装すると、GameObjectHolderのVacuum対象から除外されるわよ
    /// なんでもかんでも実装し過ぎるとメモリ圧迫するので気をつけろォ！
    /// </summary>
    /// <see cref="GameObjectHolder"/>
    public interface IIgnoreVacuumComponent
    {
    }
}