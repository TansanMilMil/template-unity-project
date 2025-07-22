using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class CameraShaker : MonoBehaviour, IIgnoreVacuumComponent
    {
        private Camera mainCamera;
        private float duration = 0;
        private bool infinity = false;
        private float currentInterval = 0;
        private float interval = 0;
        private float magnitude = 0;
        private Vector3 basePos;

        private void Start()
        {
            mainCamera = Camera.main;
            basePos = mainCamera.transform.position;
        }

        private void Update()
        {
            if (duration > 0 || infinity)
            {
                currentInterval -= Time.deltaTime;
                if (currentInterval <= 0)
                {
                    currentInterval = interval;

                    transform.position = basePos + Random.insideUnitSphere * magnitude;
                }

                duration -= Time.deltaTime;
                if (duration <= 0 && !infinity)
                {
                    mainCamera.transform.position = basePos;
                }
            }
        }

        public async UniTask ShakeAsync(float duration = 1.0f, float magnitude = 0.3f, float interval = 0.1f, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();
            
            this.duration = duration;
            this.magnitude = magnitude;
            this.interval = interval;
            basePos = mainCamera.transform.position;
            
            await UniTask.Delay(Mathf.FloorToInt(duration * 1000), cancellationToken: cToken);
        }

        public void StartShakeInfinity(float magnitude = 0.3f)
        {
            this.magnitude = magnitude;
            basePos = mainCamera.transform.position;
            infinity = true;
        }

        public void StopShakeInfinity()
        {
            infinity = false;
        }
    }
}