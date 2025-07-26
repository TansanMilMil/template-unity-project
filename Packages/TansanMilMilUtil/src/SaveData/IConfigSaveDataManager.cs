using R3;

namespace TansanMilMil.Util
{
    public interface IConfigSaveDataManager<Tkey, TValue>
    {
        Observable<bool> LoadCompleted { get; }
        Observable<bool> SaveCompleted { get; }
        bool LoadedInit { get; }
        void Save();
        void LoadAsInit();
    }
}
