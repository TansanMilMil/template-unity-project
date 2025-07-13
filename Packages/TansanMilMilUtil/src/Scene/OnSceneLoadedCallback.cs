using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class OnSceneLoadedCallback : MonoBehaviour
    {
        private static GameObject Instance;
        private static OnSceneLoadedCallback InstanceComponent;
        private Func<UniTask> callbackAsync;
        public Subject<bool> onSceneChanged = new Subject<bool>();

        private OnSceneLoadedCallback() { }

        public static OnSceneLoadedCallback GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("OnSceneLoadedCallback.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("OnSceneLoadedCallback.InstanceComponent is null!");
            }
            return InstanceComponent;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                // すでにロードされていたら自分自身を破棄して終了
                Destroy(gameObject);
                return;
            }
            else
            {
                // ロードされていなかったら、フラグをロード済みに設定する
                Instance = gameObject;
                InstanceComponent = gameObject.GetComponent<OnSceneLoadedCallback>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            callbackAsync = null;
        }

        /// <summary>
        /// SceneがLoadされた後に実行したいcallbackを登録できる
        /// </summary>
        public void SetCallbackAfterSceneLoaded(Func<UniTask> func)
        {
            callbackAsync = func;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            onSceneChanged.OnNext(true);
            DoCallbackAsync().Forget();
        }

        private async UniTask DoCallbackAsync()
        {
            if (callbackAsync != null)
            {
                await callbackAsync();
                callbackAsync = null;
            }
        }
    }
}