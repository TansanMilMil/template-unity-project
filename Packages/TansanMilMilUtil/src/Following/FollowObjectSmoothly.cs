using UnityEngine;

namespace TansanMilMil.Util
{
    public class FollowObjectSmoothly : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private float speed = 1.0f;
        [SerializeField]
        private Vector3 posCorrection = Vector3.zero;
        public enum FollowType
        {
            Normal,
            FuwaFuwa,
            BuruBuru,
        }
        [SerializeField]
        private FollowType followType = FollowType.Normal;
        private const float DefaultFuwafuwaInterval = 1.0f;
        private float fuwafuwaInterval = 0;
        private Vector3 fuwafuwaCorrection = Vector3.zero;
        public bool following { get; private set; }

        void Start()
        {
            if (target != null)
                StartFollowing();
        }

        void Update()
        {
            if (target == null || !following)
                return;

            switch (followType)
            {
                case FollowType.Normal:
                    transform.position = CalcNormalPos(transform.position);
                    break;
                case FollowType.FuwaFuwa:
                    transform.position = CalcFuwaFuwaPos(transform.position);
                    break;
                case FollowType.BuruBuru:
                    transform.position = CalcBuruBuruPos(transform.position);
                    break;
            }
            transform.position = Vector3.Lerp(transform.position, target.transform.position + posCorrection, Time.deltaTime * speed);
        }

        public void StartFollowing(GameObject target = null, float speed = -1, Vector3? posCorrection = null, FollowType followType = FollowType.Normal)
        {
            ChangeParams(target, speed, posCorrection, followType);
            following = true;
        }

        public void StopFollowing()
        {
            following = false;
        }

        public void ChangeParams(GameObject target = null, float speed = -1, Vector3? posCorrection = null, FollowType followType = FollowType.Normal)
        {
            if (target != null)
                this.target = target;
            if (speed != -1)
                this.speed = speed;
            if (posCorrection != null)
                this.posCorrection = (Vector3)posCorrection;
            if (followType != FollowType.Normal)
                this.followType = followType;
        }

        private Vector3 CalcNormalPos(Vector3 position)
        {
            return Vector3.Lerp(position, target.transform.position + posCorrection, Time.deltaTime * speed);
            ;
        }

        private Vector3 CalcFuwaFuwaPos(Vector3 position)
        {
            fuwafuwaInterval -= Time.deltaTime;
            if (fuwafuwaInterval <= 0)
            {
                fuwafuwaInterval = DefaultFuwafuwaInterval;
                fuwafuwaCorrection = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            }
            position = Vector3.Lerp(position, target.transform.position + posCorrection, Time.deltaTime * speed);
            position = Vector3.Lerp(position, position + fuwafuwaCorrection, Time.deltaTime * 0.1f);
            return position;

        }

        private Vector3 CalcBuruBuruPos(Vector3 position)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + posCorrection, Time.deltaTime * speed);
            return position + new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
        }
    }
}
