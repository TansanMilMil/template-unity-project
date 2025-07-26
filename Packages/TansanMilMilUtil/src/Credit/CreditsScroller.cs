using System.Collections.Generic;
using System.Linq;
using TMPro;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class CreditsScroller : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;
        public float scrollSpeed = 100f;
        private bool startScrolling = false;
        private ICreditProviderRegistry creditProviderRegistry => CreditProviderRegistry.GetInstance();

        private void Start()
        {
            SetCreditsText();
            startScrolling = true;
        }

        private void Update()
        {
            if (startScrolling)
            {
                creditsText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            }
        }

        private void SetCreditsText()
        {
            List<Credit> credits = GetCredits();
            if (credits == null || credits.Count == 0)
            {
                creditsText.text = "No credits available";
                return;
            }

            string text = "";
            credits.GroupBy(credit => credit.assetType).ToList().ForEach(group =>
            {
                text += $"<b>- {group.Key} -</b>\n\n";

                group.ToList().ForEach(credit =>
                {
                    text += $"{credit.name}\n";
                });

                text += "\n\n\n";
            });

            creditsText.text = text;
        }

        /// <summary>
        /// クレジットリストを取得
        /// プロバイダーが登録されていればそれを使用し、なければデフォルトを使用
        /// </summary>
        private List<Credit> GetCredits()
        {
            if (!creditProviderRegistry.IsProviderRegistered())
            {
                throw new System.Exception("CreditProvider is not registered. Please register a provider before using CreditsScroller.");
            }

            ICreditProvider provider = creditProviderRegistry.GetProvider();
            return provider?.GetCredits();
        }

        public void Hide()
        {
            startScrolling = false;
            Destroy(gameObject);
        }
    }
}
