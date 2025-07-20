using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class OnSceneLoadedCallback : SingletonMonoBehaviour<OnSceneLoadedCallback>
    {
        private Func<UniTask> callbackAsync;
        private Subject<bool> _onSceneChanged = new Subject<bool>();
        public Observable<bool> OnSceneChanged => _onSceneChanged;

        protected override void OnSingletonStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnSingletonDestroy()
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
            _onSceneChanged.OnNext(true);
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
