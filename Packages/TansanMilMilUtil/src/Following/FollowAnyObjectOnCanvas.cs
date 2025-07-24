using UnityEngine;

namespace TansanMilMil.Util
{
    public class FollowAnyObjectOnCanvas : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector2 correctionPos;
        [SerializeField]
        private RectTransform canvasRect;
        [SerializeField]
        private FollowType followType = FollowType.EveryUpdate;
        public enum FollowType
        {
            EveryUpdate,
            OnlyOnce,
        }

        private void Start()
        {
            if (followType == FollowType.OnlyOnce)
                FollowOnce();
        }

        private void Update()
        {
            if (followType == FollowType.EveryUpdate)
                Follow();
        }

        public void FollowOnce()
        {
            Follow();
        }

        private void Follow()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                null, // オーバーレイモードの場合はnull
                out canvasPos
            );
            transform.localPosition = canvasPos + correctionPos;
        }
    }
}
