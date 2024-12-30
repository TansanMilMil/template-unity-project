using R3;

namespace TansanMilMil.Util
{
    public abstract class SaveDataManagerWithIsLoaded
    {
        public Subject<bool> loadCompleted = new Subject<bool>();
        public Subject<bool> saveCompleted = new Subject<bool>();
        private bool loadedInit = false;

        public void Save()
        {
            SaveInner();
            saveCompleted.OnNext(true);
        }

        public abstract void SaveInner();

        public void LoadIfRequired()
        {
            if (!loadedInit)
            {
                Load();
            }
        }

        public void Load()
        {
            LoadInner();
            loadedInit = true;
            loadCompleted.OnNext(true);
        }

        protected abstract void LoadInner();
    }
}