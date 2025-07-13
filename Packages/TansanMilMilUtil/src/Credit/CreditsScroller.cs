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
            string text = "";
            Credits.List.GroupBy(credit => credit.assetType).ToList().ForEach(group =>
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

        public void Hide()
        {
            startScrolling = false;
            Destroy(gameObject);
        }
    }
}