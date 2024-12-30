using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using R3;
using UnityEngine;
using UnityEngine.Audio;

namespace TansanMilMil.Util
{
    /// <summary>
    /// Sceneを跨いで動作するBGM管理クラス。Singleton。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(-20)]
    public class BgmManager : BgmManagerBase, IIgnoreVacuumComponent
    {
        private static GameObject Instance;
        private static BgmManager InstanceComponent;
        public static float TimeBeforeMovingScene = 0;
        private static AssetsKeeper<AudioClip> AudioKeeper = AssetsTypeSettings.NewAssetsKeeper<AudioClip>(autoRelease: true);

        private BgmManager() { }

        public static BgmManager GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("BgmManager.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("BgmManager.InstanceComponent is null!");
            }
            return InstanceComponent;
        }

        protected override void AwakeExtension()
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
                InstanceComponent = gameObject.GetComponent<BgmManager>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        protected override UniTask StartExtensionAsync()
        {
            return UniTask.CompletedTask;
        }

        public static void ResetStaticParams()
        {
            TimeBeforeMovingScene = 0;
        }

        public override async UniTask<AudioClip> LoadAssetAsync(string bgmPath)
        {
            AudioClip audioClip = await AudioKeeper.LoadAssetAsync(bgmPath);
            return audioClip;
        }

        public override void ReleaseAsset(string bgmPath)
        {
            AudioKeeper.ReleaseAsset(bgmPath);
        }

        public override void ReleaseAllAssets()
        {
            AudioKeeper.ReleaseAllAssets();
        }
    }
}
