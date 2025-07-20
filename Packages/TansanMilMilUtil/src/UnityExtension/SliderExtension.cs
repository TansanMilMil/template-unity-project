using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    /// <summary>
    /// Sliderはドラッグ開始と終了を検知できないため、その機能を追加する
    /// Sliderと一緒にGameObjectにアタッチする
    /// </summary>
    public class SliderExtension : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        public Slider slider;
        private Subject<bool> _onStopDrag = new Subject<bool>();
        public Observable<bool> OnStopDrag => _onStopDrag;
        private Subject<bool> _onStartDrag = new Subject<bool>();
        public Observable<bool> OnStartDrag => _onStartDrag;

        public void OnEndDrag(PointerEventData data)
        {
            _onStopDrag.OnNext(true);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            _onStartDrag.OnNext(true);
        }
    }
}
