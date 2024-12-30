using UnityEngine;

namespace TansanMilMil.Util
{
    public class GameObjectCache
    {
        public string tag { get; private set; }
        public GameObject gameObject { get; private set; }
        public bool vacuumable { get; private set; } = true;
        /// <summary>
        /// このGameObjectをVacuumするためのフラグ
        /// </summary>
        public bool reserveVacuum { get; private set; } = false;

        public GameObjectCache(string tag, GameObject gameObject, bool vacuumable)
        {
            this.tag = tag;
            this.gameObject = gameObject;
            this.vacuumable = vacuumable;
        }

        public void SetReserveVacuum(bool reserveVacuum)
        {
            this.reserveVacuum = reserveVacuum;
        }
    }
}