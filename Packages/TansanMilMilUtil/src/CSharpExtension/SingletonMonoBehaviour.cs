using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// MonoBehaviour用の汎用シングルトンベースクラス
    /// </summary>
    /// <typeparam name="T">シングルトンにするMonoBehaviourクラス</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static GameObject instance;
        private static T instanceComponent;
        private static readonly object lockObject = new object();
        private static bool applicationIsQuitting = false;

        /// <summary>
        /// シングルトンインスタンスのGameObject
        /// </summary>
        protected static GameObject Instance => instance;

        /// <summary>
        /// シングルトンインスタンスのコンポーネント
        /// </summary>
        protected static T InstanceComponent => instanceComponent;

        /// <summary>
        /// シングルトンインスタンスを取得する
        /// </summary>
        /// <returns>シングルトンインスタンス</returns>
        /// <exception cref="InvalidOperationException">インスタンスが初期化されていない場合</exception>
        public static T GetInstance()
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (lockObject)
            {
                if (instance == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name}.Instance is null! Make sure the GameObject with {typeof(T).Name} component exists in the scene.");
                }

                if (instanceComponent == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name}.InstanceComponent is null! Make sure the {typeof(T).Name} component is attached to the GameObject.");
                }

                return instanceComponent;
            }
        }

        /// <summary>
        /// シングルトンインスタンスが存在するかチェックする
        /// </summary>
        /// <returns>インスタンスが存在する場合true</returns>
        public static bool HasInstance()
        {
            return instance != null && instanceComponent != null && !applicationIsQuitting;
        }

        /// <summary>
        /// シングルトンインスタンスを安全に取得する（null許容）
        /// </summary>
        /// <returns>シングルトンインスタンス、または存在しない場合はnull</returns>
        public static T GetInstanceSafe()
        {
            return HasInstance() ? instanceComponent : null;
        }

        /// <summary>
        /// Unity Awakeメソッド - シングルトンの初期化を行う
        /// </summary>
        private void Awake()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = gameObject;
                    instanceComponent = this as T;

                    // DontDestroyOnLoadを適用するかどうかは子クラスで制御
                    if (ShouldDontDestroyOnLoad())
                    {
                        // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                        if (gameObject.transform.parent != null)
                        {
                            gameObject.transform.parent = null;
                        }
                        DontDestroyOnLoad(gameObject);
                    }

                    OnSingletonAwake();
                }
                else if (instance != gameObject)
                {
                    // 既にインスタンスが存在する場合は重複を削除
                    Debug.LogWarning($"[Singleton] Trying to instantiate a second instance of singleton class {typeof(T).Name}. Destroying duplicate.");
                    Destroy(gameObject);
                }
            }
        }

        private void Start()
        {
            OnSingletonStart();
        }

        /// <summary>
        /// Unity OnDestroy メソッド
        /// </summary>
        private void OnDestroy()
        {
            if (instance == gameObject)
            {
                OnSingletonDestroy();
                instance = null;
                instanceComponent = null;
            }
        }

        /// <summary>
        /// Unity OnApplicationQuit メソッド
        /// </summary>
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        /// <summary>
        /// DontDestroyOnLoadを適用するかどうかを決定する
        /// デフォルトでは true を返す。子クラスでオーバーライド可能。
        /// </summary>
        /// <returns>DontDestroyOnLoadを適用する場合true</returns>
        protected virtual bool ShouldDontDestroyOnLoad()
        {
            return true;
        }

        /// <summary>
        /// シングルトンが初期化された時に呼び出されるメソッド
        /// 子クラスで独自の初期化処理を実装する場合にオーバーライド
        /// </summary>
        protected virtual void OnSingletonAwake()
        {
            // デフォルトは空実装
        }

        /// <summary>
        /// シングルトンが開始された時に呼び出されるメソッド
        /// 子クラスで独自の開始処理を実装する場合にオーバーライド
        /// </summary>
        protected virtual void OnSingletonStart()
        {
            // Startメソッドのオーバーライド用
        }

        /// <summary>
        /// シングルトンが破棄される時に呼び出されるメソッド
        /// 子クラスで独自のクリーンアップ処理を実装する場合にオーバーライド
        /// </summary>
        protected virtual void OnSingletonDestroy()
        {
            // デフォルトは空実装
        }
    }
}
