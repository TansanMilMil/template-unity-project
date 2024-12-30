using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace TansanMilMil.Util
{
    public class Message : MonoBehaviour
    {
        public Transform dammyTextTransform;
        public TextMeshProUGUI dammyTextMesh;
        public TextMeshProUGUI textMesh;
        public Color talkerTextColor;
        public Transform frameImageTransform;
        public Image frameImage;
        public AudioSource audioSource;
        public AudioClip messageSe;
        public AudioClip readMessageSe;
        public const int DefaultTextSoundInterval = 200;
        private int textSoundInterval = DefaultTextSoundInterval;
        public GameObject choiceObj;
        [NonSerialized]
        /// <summary>メッセージ表示処理が完了したか</summary>
        public BehaviorSubject<bool> isCompleted = new BehaviorSubject<bool>(false);
        [NonSerialized]
        /// <summary>文字送り中であるか</summary>
        public BehaviorSubject<bool> isTalking = new BehaviorSubject<bool>(false);
        public enum MessageDesignType
        {
            Default,
            Reminiscence,
        }
        public MessageDesignType designType = MessageDesignType.Default;
        public List<MessageDesign> messageDesigns;
        private SkipEvent skipEvent;
        private CancellationTokenSource cts;
        private CancellationToken ctDestroy;

        private void Start()
        {
            ctDestroy = this.GetCancellationTokenOnDestroy();
            skipEvent = GameObjectHolder.GetInstance().FindComponentBy<SkipEvent>("SkipEvent");

            SetInitSubscriber();
        }

        private void SetInitSubscriber()
        {
            skipEvent?.onSkipEvent
                .Subscribe(_ =>
                {
                    cts?.Cancel();
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
        }

        private void OnDestroy()
        {
            isCompleted.OnNext(true);
            isTalking.OnNext(false);

            cts?.Dispose();
            cts = null;
        }

        private CancellationTokenSource GetCTS()
        {
            if (cts == null || cts.IsCancellationRequested)
            {
                cts = new CancellationTokenSource();
            }
            return cts;
        }

        public void ChangeMessageDesignType(MessageDesignType type)
        {
            designType = type;

            ChangeColors();
        }

        private void ChangeColors()
        {
            MessageDesign messageDesign = messageDesigns.Find(d => d.designType == designType);
            if (messageDesign == null) return;

            frameImage.color = messageDesign.frameColor;
            textMesh.color = messageDesign.textColor;
            talkerTextColor = messageDesign.talkerTextColor;
        }

        public void DestroyMessage()
        {
            Destroy(this.gameObject);
        }

        public async UniTask<int> StartRenderMessagesAsync(IEnumerable<string> texts, string talkerName, Vector3? frameCanvasPos, IEnumerable<string> choices, int writeTextInterval, int autoMessageWaitMSec)
        {
            int selectedChoice = -1;
            ChangeFramePos(frameCanvasPos);
            ExpandFrame(texts.First(), talkerName);
            await DisplayMessageFrameAsync();
            await UniTask.Yield(ctDestroy);

            foreach (string text in texts)
            {
                ExpandFrame(text, talkerName);
                await WriteMessageAsync(text, talkerName, choices, writeTextInterval, autoMessageWaitMSec);
                await UniTask.Yield(ctDestroy);
            }
            if (choices != null && choices.Count() >= 1)
            {
                selectedChoice = await DisplayChoicesAsync(choices);
                await UniTask.Yield(ctDestroy);
            }
            await HideMessageFrameAsync();
            await UniTask.Yield(ctDestroy);

            isCompleted.OnNext(true);
            return selectedChoice;
        }

        private void ChangeFramePos(Vector3? frameCanvasPos)
        {
            if (frameCanvasPos == null)
            {
                dammyTextTransform.localPosition = Vector3.zero;
            }
            else
            {
                dammyTextTransform.localPosition = (Vector3)frameCanvasPos;
            }

            // frameの位置が画面からはみ出ないように調整
            RectTransform rect = dammyTextTransform.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(
                Mathf.Clamp(rect.anchoredPosition.x, -69.4f, 69.4f),
                Mathf.Clamp(rect.anchoredPosition.y, -69.4f, 69.4f)
            );
        }

        private void ExpandFrame(string text, string talkerName)
        {
            dammyTextMesh.text = GetTalkerNameString(talkerName);
            dammyTextMesh.text += text;
        }

        private async UniTask DisplayMessageFrameAsync()
        {
            frameImageTransform.localScale = Vector3.zero;
            frameImage.enabled = true;
            await frameImageTransform.DOScale(Vector3.one, 0.16f).ToUniTask(cancellationToken: GetCTS().Token);
            await UniTask.Yield(ctDestroy);
        }

        private async UniTask HideMessageFrameAsync()
        {
            textMesh.text = "";
            await frameImageTransform.DOScale(Vector3.zero, 0.16f).ToUniTask(cancellationToken: GetCTS().Token);
            await UniTask.Yield(ctDestroy);
            frameImage.enabled = false;
        }

        private string GetTalkerNameString(string talkerName)
        {
            if (string.IsNullOrWhiteSpace(talkerName)) return "";

            float fontSize = dammyTextMesh.fontSize - 1.5f;
            string hexColor = ColorUtility.ToHtmlStringRGB(talkerTextColor);
            return $"<color=#{hexColor}><size={fontSize}>{talkerName}</size></color>\n";
        }

        private async UniTask WriteMessageAsync(string text, string talkerName, IEnumerable<string> choices, int writeTextInterval, int autoMessageWaitMSec)
        {
            // TODO
            // int writeTextInterval = PlayerConfigManager.GetInstance().GetConfig().messageWriteTextInterval;

            int textInterval = writeTextInterval;
            textMesh.text = GetTalkerNameString(talkerName);

            int charIndex = 0;
            bool completed = false;
            isTalking.OnNext(true);

            if (writeTextInterval < 0)
            {
                textMesh.text = $@"{GetTalkerNameString(talkerName) + text}";
                isTalking.OnNext(false);
            }
            else
            {
                // textを1文字ずつ表示する
                IDisposable writeText = Observable.Interval(TimeSpan.FromMilliseconds(textInterval))
                    .TakeWhile(_ => charIndex < text.Length)
                    .Subscribe(
                        _ =>
                        {
                            textMesh.text = $@"{textMesh.text + text[charIndex]}";
                            charIndex++;
                        },
                        onCompleted =>
                        {
                            isTalking.OnNext(false);
                            if ((choices != null && choices.Count() >= 1) || autoMessageWaitMSec > 0) completed = true;
                        }
                    )
                    .AddTo(this.GetCancellationTokenOnDestroy());

                // 文字送りSEを再生する
                audioSource.PlayOneShot(messageSe);
                IDisposable textSound = Observable.Interval(TimeSpan.FromMilliseconds(textSoundInterval))
                    .TakeWhile(_ => charIndex < text.Length)
                    .Subscribe(_ =>
                    {
                        audioSource.PlayOneShot(messageSe);
                    })
                    .AddTo(this.GetCancellationTokenOnDestroy());

                // キー入力があった場合は文字送りを強制的に完了させる
                IDisposable waitKey = Observable.EveryUpdate()
                    .Where(_ => InputKeys.GetInstance().AnyInputGetKeyDown(KeyRole.Decide))
                    .TakeWhile(_ => !completed)
                    .Subscribe(_ =>
                    {
                        audioSource.PlayOneShot(readMessageSe);
                        charIndex = text.Length;
                        textMesh.text = $@"{GetTalkerNameString(talkerName) + text}";
                        completed = true;
                        isTalking.OnNext(false);
                    })
                    .AddTo(this.GetCancellationTokenOnDestroy());

                // イベントスキップされた時も文字送りを強制的に完了させる
                skipEvent.onSkipEvent
                    .Subscribe(_ =>
                    {
                        charIndex = text.Length;
                        textMesh.text = $@"{GetTalkerNameString(talkerName) + text}";
                        completed = true;
                        isTalking.OnNext(false);
                        Destroy(gameObject);
                    })
                    .AddTo(this.GetCancellationTokenOnDestroy());

                await UniTask.WaitUntil(() => completed, cancellationToken: GetCTS().Token);
                await UniTask.Yield(ctDestroy);

                if (autoMessageWaitMSec > 0)
                {
                    await UniTask.Delay(autoMessageWaitMSec, cancellationToken: GetCTS().Token);
                    await UniTask.Yield(ctDestroy);
                }

                // 後始末
                writeText.Dispose();
                textSound.Dispose();
                waitKey.Dispose();
            }
        }

        private async UniTask<int> DisplayChoicesAsync(IEnumerable<string> choices)
        {
            int selectedChoice = -1;
            GameObject[] objects = new GameObject[choices.Count()];

            for (int i = 0; i < choices.Count(); i++)
            {
                // nullの場合は非表示の選択肢として扱う
                if (choices.ElementAt(i) == null) continue;

                // 選択肢の配置
                GameObject obj = Instantiate(choiceObj, dammyTextMesh.transform);
                objects[i] = obj;
                float offsetY = -17.5f;
                float basePosY = -10.4f;
                RectTransform rectTran = obj.GetComponent<RectTransform>();
                rectTran.anchoredPosition = new Vector2(rectTran.anchoredPosition.x, basePosY + offsetY * i);
                obj.transform.Find("text").GetComponent<TextMeshProUGUI>().text = choices.ElementAt(i);
                obj.transform.localScale = new Vector3(0, 1, 1);
                await obj.transform.DOScale(Vector3.one, 0.1f).ToUniTask(cancellationToken: GetCTS().Token);
                await UniTask.Yield(ctDestroy);

                // 選択肢オブジェクトにクリックトリガーを適用
                EventTrigger evTrigger = GameObjectHolder.GetInstance().FindComponentBy<EventTrigger>(obj);
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                int callbackChoice = i;
                entry.callback.AddListener((eventData) =>
                {
                    if (selectedChoice >= 0) return;
                    selectedChoice = callbackChoice;
                });
                evTrigger.triggers.Add(entry);
            }

            // 選択肢がクリックされるまでウェイト
            await UniTask.WaitUntil(() => selectedChoice >= 0, cancellationToken: GetCTS().Token);
            await UniTask.Yield(ctDestroy);

            audioSource.PlayOneShot(readMessageSe);

            // 選択肢消す
            foreach (GameObject obj in objects)
            {
                obj.transform.DOScale(new Vector3(1, 0, 1), 0.1f)
                    .OnComplete(() => Destroy(obj))
                    .ToUniTask(cancellationToken: GetCTS().Token).Forget();
            }
            return selectedChoice;
        }
    }
}