using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

using R3;
using UnityEngine;
using static TansanMilMil.Util.Message;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class MessageManager : MonoBehaviour, IIgnoreVacuumComponent
    {
        public GameObject messageObj;
        public static bool RenderingMessage = false;
        public RectTransform canvasRect;
        public Camera mainCamera;
        private MessageDesignType designType = MessageDesignType.Default;
        public BehaviorSubject<int> messageWriteTextInterval = new(0);

        private void OnDestroy()
        {
            RenderingMessage = false;
        }

        public void ChangeMessageDesignType(MessageDesignType type)
        {
            designType = type;
        }

        /// <param name="overrideWriteTextInterval">テキスト表示速度。1以上を指定でデフォルト速度を上書きできる。-1指定で瞬間表示する。</param>
        /// <param name="choices">選択肢表示したい場合のみ指定する。LocaleString = nullの要素は画面には表示されない（選択肢の数, indexを変えたくない時に使う）</param>
        public async UniTask<int> RenderLocaleMessagesAsync(
            LocaleString texts,
            LocaleString talkerName = null,
            Vector3? frameCanvasPos = null,
            IEnumerable<LocaleString> choices = null,
            int overrideWriteTextInterval = 0,
            int autoMessageWaitMSec = 0)
        {

            string localeTexts = await GameLocale.GetEntryValueReplacedAsync(texts);
            string localeTalkerName = await GameLocale.GetEntryValueReplacedAsync(talkerName);

            List<string> localeChoices = new List<string>();
            if (choices != null)
            {
                foreach (LocaleString choice in choices)
                {
                    if (choice == null || choice?.key == null)
                    {
                        localeChoices.Add(null);
                    }
                    else
                    {
                        string localeChoice = await GameLocale.GetEntryValueReplacedAsync(choice);
                        localeChoices.Add(localeChoice);
                    }
                }
            }

            return await RenderSingleMessageAsync(localeTexts, localeTalkerName, frameCanvasPos, localeChoices.ToArray(), overrideWriteTextInterval, autoMessageWaitMSec);
        }

        /// <param name="overrideWriteTextInterval">テキスト表示速度。1以上を指定でデフォルト速度を上書きできる。-1指定で瞬間表示する。</param>
        public async UniTask<int> RenderSingleMessageAsync(
            string texts,
            string talkerName = null,
            Vector3? frameCanvasPos = null,
            IEnumerable<string> choices = null,
            int overrideWriteTextInterval = 0,
            int autoMessageWaitMSec = 0)
        {

            return await RenderMultiMessagesAsync(new List<string> { texts }, talkerName, frameCanvasPos, choices, overrideWriteTextInterval, autoMessageWaitMSec);
        }

        public async UniTask<int> RenderMultiMessagesAsync(
            List<string> texts,
            string talkerName,
            Vector3? frameCanvasPos,
            IEnumerable<string> choices,
            int overrideWriteTextInterval,
            int autoMessageWaitMSec)
        {
            GameObject hukidashi = Instantiate(messageObj);
            Message message = GameObjectHolder.GetInstance().FindComponentBy<Message>(hukidashi);
            message.ChangeMessageDesignType(designType);
            message.isCompleted
                .Where(x => x == true)
                .Subscribe(_ =>
                {
                    RenderingMessage = false;
                    Destroy(message.gameObject, 0.3f);
                })
                .AddTo(this.GetCancellationTokenOnDestroy()); ;
            RenderingMessage = true;
            return await message.StartRenderMessagesAsync(
                texts,
                talkerName,
                frameCanvasPos,
                choices,
                overrideWriteTextInterval == 0 ? messageWriteTextInterval.Value : overrideWriteTextInterval,
                autoMessageWaitMSec);
        }

        public Vector2 GetMessagePos(GameObject eventObject)
        {
            // Vector2 correction = new Vector2();
            // SpriteRenderer spriteRenderer = GameObjectHolder.GetInstance().FindComponentBy<SpriteRenderer>(eventObject.transform.Find("followingCamera/rotatableWrap/sprite")?);
            // SpriteがあるならSpriteの直上の座標を取得する
            // if (spriteRenderer != null)
            // {
            //     correction.y = spriteRenderer.bounds.size.y + 60;
            // }
            // else
            // {
            //     correction.y = 50;
            // }
            // correction.y = 60;

            Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(eventObject.transform.position);
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                targetScreenPos,
                null, // オーバーレイモードの場合はnull
                out canvasPos
            );

            // float fieldOfView = BasicCameraMotion.DefaultFieldOfView - basicCameraMotion.targetFieldOfView;
            // float posY = Mathf.Clamp(15 + fieldOfView, -45, 45);
            // canvasPos = new Vector2(canvasPos.x, posY);
            // canvasPos += correction;
            return canvasPos;
        }
    }
}