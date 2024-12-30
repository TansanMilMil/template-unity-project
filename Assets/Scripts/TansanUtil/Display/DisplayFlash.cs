using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class DisplayFlash : MonoBehaviour, IIgnoreVacuumComponent
    {
        private static GameObject Instance;
        private static DisplayFlash InstanceComponent;
        public GameObject flashObj;
        public Image flash;
        private readonly Color DefaultColor = new Color(1, 1, 1, 1);
        private CancellationToken ctDestroy;

        private DisplayFlash() { }

        public static DisplayFlash GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("DisplayFlash.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("DisplayFlash.InstanceComponent is null!");
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
                InstanceComponent = gameObject.GetComponent<DisplayFlash>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            ctDestroy = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask FlashAsync(float duration = 0.8f, Color? flashColor = null)
        {
            flash.color = flashColor == null ? DefaultColor : (Color)flashColor;
            Color targetColor = new Color(flash.color.r, flash.color.g, flash.color.b, 0);
            flashObj.SetActive(true);
            await flash.DOColor(targetColor, duration);
            await UniTask.Yield(ctDestroy);
            flashObj.SetActive(false);
        }

        public async UniTask WhiteOutAsync(float duration = 1.5f, Color? flashColor = null)
        {
            Color targetColor = flashColor == null ? DefaultColor : (Color)flashColor;
            flash.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0);
            flashObj.SetActive(true);
            await flash.DOColor(targetColor, duration);
            await UniTask.Yield(ctDestroy);
        }

        public async UniTask RemoveWhiteOutAsync(float duration = 1.5f, Color? flashColor = null)
        {
            Color targetColor = flashColor == null ? DefaultColor : (Color)flashColor;
            flash.color = new Color(targetColor.r, targetColor.g, targetColor.b, 1);
            flashObj.SetActive(true);
            await flash.DOColor(new Color(targetColor.r, targetColor.g, targetColor.b, 0), duration);
            await UniTask.Yield(ctDestroy);
            flashObj.SetActive(false);
        }

        public void RemoveWhiteOutImmediately()
        {
            flashObj.SetActive(false);
        }
    }
}