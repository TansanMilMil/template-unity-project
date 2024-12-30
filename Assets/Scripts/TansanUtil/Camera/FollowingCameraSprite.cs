using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class FollowingCameraSprite : MonoBehaviour
    {
        private Camera mainCamera;
        public bool freezeRotateX = false;
        public bool freezeRotateY = false;
        public bool freezeRotateZ = false;

        private void Start()
        {
            mainCamera = GameObjectHolder.GetInstance().FindComponentBy<Camera>(GameObjectHolder.GetInstance().FindObjectBy("MainCamera"));
        }

        private void Update()
        {
            if (mainCamera == null) return;

            Vector3 vector = mainCamera.transform.position - transform.position;
            if (freezeRotateX) vector.x = 0;
            if (freezeRotateY) vector.y = 0;
            if (freezeRotateZ) vector.z = 0;
            if (vector != Vector3.zero)
            {
                transform.localRotation = Quaternion.LookRotation(vector);
            }
        }
    }
}