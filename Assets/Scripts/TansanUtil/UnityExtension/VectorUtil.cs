using UnityEngine;

namespace TansanMilMil.Util
{
    public static class VectorUtil
    {
        public static Vector3 GetDirection(Vector3 pos1, Vector3 pos2)
        {
            Vector3 heading = pos1 - pos2;
            float distance = heading.magnitude;
            return heading / distance;
        }

        public static Vector3 GetDirectionOnCamera(float x, float y, Camera camera)
        {
            Vector3 cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            return (cameraForward * y) + (camera.transform.right * x);
        }

        public static Vector3 DeepCopy(Vector3 vec)
        {
            return new Vector3(vec.x, vec.y, vec.z);
        }

        public static float[] ToFloatArray(Vector3 vec)
        {
            return new float[3] {
                vec.x,
                vec.y,
                vec.z
            };
        }

        public static Vector3 ParseVector(float[] f)
        {
            return new Vector3(f[0], f[1], f[2]);
        }
    }
}