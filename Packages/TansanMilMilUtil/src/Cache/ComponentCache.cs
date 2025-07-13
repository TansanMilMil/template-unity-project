using UnityEngine;

namespace TansanMilMil.Util
{
    public class ComponentCache
    {
        public string tag { get; private set; }
        public int gameObjectInstanceID { get; private set; }
        public object component { get; private set; }
        public bool vacuumable { get; private set; } = true;
        /// <summary>
        /// このComponentをVacuumするためのフラグ
        /// </summary>
        public bool reserveVacuum { get; private set; } = false;

        public ComponentCache(string tag, int gameObjectInstanceID, object component, bool vacuumable)
        {
            this.tag = tag;
            this.gameObjectInstanceID = gameObjectInstanceID;
            this.component = component;
            this.vacuumable = vacuumable;
        }

        public void SetReserveVacuum(bool reserveVacuum)
        {
            this.reserveVacuum = reserveVacuum;
        }
    }
}