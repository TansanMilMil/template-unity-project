using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TansanMilMil.Util
{
    public class LoadingMainAssets : MonoBehaviour
    {
        public AssetLabelReference addressableLabelInitLoad;
        public static string NextSceneName = null;
        [SerializeField]
        public event Action funcAfterLoading;
        public GameObject loadingText;

        void Start()
        {
            InitAsync().Forget();
        }

#pragma warning disable CS1998
        private async UniTask InitAsync()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log("skip => Addressables.DownloadDependenciesAsync");
#else
            try
            {
                loadingText.SetActive(true);
                Debug.Log("Addressables.DownloadDependenciesAsync...");
                await AddressablesWrapper.GetInstance().DownloadDependenciesAsync(addressableLabelInitLoad.labelString, true);
                Debug.Log("Addressables.DownloadDependenciesAsync was completed.");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                loadingText.SetActive(false);
            }
#endif

            if (NextSceneName == null)
            {
                throw new System.Exception("LoadingMainAssets.NextSceneName is null.");
            }
            else
            {
                funcAfterLoading.Invoke();
            }

        }
    }
}