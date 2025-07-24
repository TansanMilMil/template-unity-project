using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class OnSceneLoadedCallback : SingletonMonoBehaviour<OnSceneLoadedCallback>
    {
        private readonly List<Func<UniTask>> callbacksAsync = new List<Func<UniTask>>();
        private Subject<bool> _onSceneChanged = new Subject<bool>();
        public Observable<bool> OnSceneChanged => _onSceneChanged;

        protected override void OnSingletonStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnSingletonDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            callbacksAsync.Clear();
        }

        /// <summary>
        /// SceneがLoadされた後に実行したいcallbackを登録できる
        /// </summary>
        public void SetCallbackAfterSceneLoaded(Func<UniTask> func)
        {
            callbacksAsync.Add(func);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _onSceneChanged.OnNext(true);
            DoCallbackAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask DoCallbackAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            if (callbacksAsync != null)
            {
                foreach (var callback in callbacksAsync)
                {
                    cToken.ThrowIfCancellationRequested();

                    Debug.Log($"OnSceneLoadedCallback: Executing callback for scene load: {callback.Method.Name}");
                    await callback();
                }
                callbacksAsync.Clear();
            }
        }
    }
}
