using UnityEngine;

namespace TansanMilMil.Util
{
    public interface IGameObjectCache
    {
        string tag { get; }
        GameObject gameObject { get; }
        bool vacuumable { get; }
        bool reserveVacuum { get; }
        void SetReserveVacuum(bool reserveVacuum);
    }
}