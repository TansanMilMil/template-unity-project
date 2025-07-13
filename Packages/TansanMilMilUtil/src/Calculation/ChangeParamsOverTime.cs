using UnityEditor;
using UnityEngine;
using R3;
using System;

namespace TansanMilMil.Util
{
    /// <summary>
    /// 徐々に変化する値を管理するクラス。
    /// WARNING: このクラスのインスタンスが不要になったらDispose()を呼ぶこと。
    /// </summary>
    public class ChangeParamsOverTime
    {
        public Subject<float> valueChanged = new();
        private float targetValue = 0;
        private float currentValue = 0;
        private float changeSpeed = 3;
        private IDisposable disposable;

        public ChangeParamsOverTime(float targetValue, float changeSpeed = 0)
        {
            this.currentValue = targetValue;
            SetTargetValue(targetValue);
            if (changeSpeed != 0) this.changeSpeed = changeSpeed;

            // targetValueとcurrentValueが同じになるまで毎フレーム変更する
            disposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    ChangeParams();
                });
        }

        ~ChangeParamsOverTime()
        {
            // Dispose忘れ防止としてデストラクタでDisposeする
            Dispose();
        }

        private void ChangeParams()
        {
            if (currentValue == targetValue) return;

            if (currentValue < targetValue)
            {
                currentValue += Mathf.Clamp(changeSpeed, 0, targetValue - currentValue);
            }
            if (currentValue > targetValue)
            {
                currentValue -= Mathf.Clamp(changeSpeed, 0, currentValue - targetValue);
            }
            valueChanged.OnNext(currentValue);
        }

        public float CurrentValue()
        {
            return currentValue;
        }

        public void Dispose()
        {
            disposable.Dispose();
            valueChanged.OnCompleted();
        }

        public void SetTargetValue(float targetValue)
        {
            this.targetValue = targetValue;
        }
    }
}