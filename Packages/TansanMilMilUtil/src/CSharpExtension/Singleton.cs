using System;

namespace TansanMilMil.Util
{
    /// <summary>
    /// 通常クラス用の汎用シングルトンベースクラス
    /// </summary>
    /// <typeparam name="T">シングルトンにするクラス</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        private static readonly object lockObject = new object();
        private static T instance;

        /// <summary>
        /// シングルトンインスタンスを取得する
        /// </summary>
        /// <returns>シングルトンインスタンス</returns>
        public static T GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new T();
                        if (instance is Singleton<T> singletonInstance)
                        {
                            singletonInstance.OnSingletonCreated();
                        }
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// シングルトンインスタンスが存在するかチェックする
        /// </summary>
        /// <returns>インスタンスが存在する場合true</returns>
        public static bool HasInstance()
        {
            return instance != null;
        }

        /// <summary>
        /// シングルトンインスタンスを安全に取得する（null許容）
        /// </summary>
        /// <returns>シングルトンインスタンス、または存在しない場合はnull</returns>
        public static T GetInstanceSafe()
        {
            return instance;
        }

        /// <summary>
        /// シングルトンインスタンスを破棄する（主にテスト用）
        /// </summary>
        public static void DestroyInstance()
        {
            lock (lockObject)
            {
                if (instance != null)
                {
                    if (instance is Singleton<T> singletonInstance)
                    {
                        singletonInstance.OnSingletonDestroyed();
                    }
                    if (instance is IDisposable disposableInstance)
                    {
                        disposableInstance.Dispose();
                    }
                    instance = null;
                }
            }
        }

        /// <summary>
        /// シングルトンが作成された時に呼び出されるメソッド
        /// 子クラスで独自の初期化処理を実装する場合にオーバーライド
        /// </summary>
        protected virtual void OnSingletonCreated()
        {
            // デフォルトは空実装
        }

        /// <summary>
        /// シングルトンが破棄される時に呼び出されるメソッド
        /// 子クラスで独自のクリーンアップ処理を実装する場合にオーバーライド
        /// </summary>
        protected virtual void OnSingletonDestroyed()
        {
            // デフォルトは空実装
        }
    }
}
