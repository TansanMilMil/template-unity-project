using R3;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public class GamePauseUI : MonoBehaviour
    {
        public GamePause gamePause;
        public GameObject pauseObjects;

        void Start()
        {
            SetInitSubscriber();
        }

        private void SetInitSubscriber()
        {
            gamePause.onPaused
                .Subscribe(isPaused =>
                {
                    pauseObjects.SetActive(isPaused);
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
        }
    }
}