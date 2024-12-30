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
        public Subject<bool> onEndDrag = new Subject<bool>();
        public Subject<bool> onBeginDrag = new Subject<bool>();

        public void OnEndDrag(PointerEventData data)
        {
            onEndDrag.OnNext(true);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            onBeginDrag.OnNext(true);
        }
    }
}