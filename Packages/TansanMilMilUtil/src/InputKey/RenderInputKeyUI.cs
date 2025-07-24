using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    public class RenderInputKeyUI : MonoBehaviour
    {
        [SerializeField] private KeyRole inputKeyType;
        [SerializeField] private TextMeshProUGUI inputKeyText;
        [Header("ImageとTextMeshProUGUIのどちらか一方だけを表示する場合はtrueにする。falseの場合は両方表示される。")]
        [SerializeField] private bool renderOnlyOne = false;
        [SerializeField] private Sprite interactionIcon;
        [SerializeField] private Image inputKeyBackgroundImage;
        [SerializeField] private Image inputKeyImage;
        [SerializeField] private InputKeyBindConditions bindConditions = InputKeyBindConditions.All;
        /// <summary>
        /// 基本的にゲーム中ずっとUIのSpriteは必要になるはずなので SpriteKeeper.ReleaseAllAssets() はしてない。メモリの問題が出てきたら要検討。
        /// </summary>
        private static AssetsKeeper<Sprite> SpriteKeeper;

        private void Start()
        {
            RenderKeyNameAndSpriteAsync(this.GetCancellationTokenOnDestroy()).Forget();

            SpriteKeeper = new AssetsKeeperFactory(AssetsTypeSettingRegistry.GetAssetsTypeSetting()).Create<Sprite>();
        }

        private async UniTask RenderKeyNameAndSpriteAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            if (inputKeyImage != null)
            {
                inputKeyImage.enabled = false;
            }
            if (inputKeyText != null)
            {
                inputKeyText.text = "";
            }

            // 文字で表現できないKeyCodeがあるのでImageを優先的に表示させる
            bool imageRendered = await RenderKeyImageAsync(cToken);

            cToken.ThrowIfCancellationRequested();
            if (imageRendered && renderOnlyOne)
            {
                return;
            }

            RenderKeyText();
            RenderSubstituteIconIfNeed();
        }

        private async UniTask<bool> RenderKeyImageAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            if (inputKeyImage != null)
            {
                string spritePath = InputKeys.GetInstance().GetSingleKeySpritePathForUI(inputKeyType, bindConditions);
                if (spritePath != null)
                {
                    inputKeyImage.enabled = false;
                    inputKeyImage.sprite = await SpriteKeeper.LoadAssetAsync(spritePath, cToken);

                    cToken.ThrowIfCancellationRequested();
                    inputKeyImage.enabled = true;

                    return true;
                }
            }
            return false;
        }

        private bool RenderKeyText()
        {
            if (inputKeyText != null)
            {
                inputKeyText.text = InputKeys.GetInstance().GetSingleKeyNameForUI(inputKeyType, bindConditions);
                return true;
            }
            return false;
        }

        private void RenderSubstituteIconIfNeed()
        {
            if (inputKeyImage == null || inputKeyText == null)
            {
                return;
            }

            // 何もレンダリングするものがなかった場合は代替アイコンを表示する
            if (!inputKeyImage.enabled && string.IsNullOrWhiteSpace(inputKeyText.text))
            {
                if (interactionIcon != null)
                {
                    inputKeyImage.sprite = interactionIcon;
                    inputKeyImage.enabled = true;
                }
            }
        }
    }
}
