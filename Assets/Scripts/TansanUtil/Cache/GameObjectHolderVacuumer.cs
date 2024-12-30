using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class GameObjectHolderVacuumer
    {
        private const int MaxObjectCacheCount = 100;
        private const int MaxComponentCacheCount = 100;
        private int vacuumedObjectCount = 0;
        private int vacuumedComponentCount = 0;
        private const int WarnVacuumedObjectCount = 1000;
        private const int WarnVacuumedComponentCount = 1000;

        /// <summary>
        /// キャッシュ量が閾値を超えた場合、古いキャッシュ順に削除する
        /// メモリリークを防ぐために使ってね
        /// </summary>
        public List<GameObjectCache> VacuumObjectCaches(List<GameObjectCache> objectCaches)
        {
            // Vacuum対象だけをListに抽出
            List<GameObjectCache> vacuumObjectCaches = objectCaches.FindAll(x => x.vacuumable);

            // キャッシュ量が増えすぎたら、古いものからフラグ(reserveVacuum)をtrueにして削除する
            if (vacuumObjectCaches.Count > MaxObjectCacheCount)
            {
                for (int i = 0; i < vacuumObjectCaches.Count - MaxObjectCacheCount; i++)
                {
                    vacuumObjectCaches[i].SetReserveVacuum(true);
                }
            }
            vacuumedObjectCount += objectCaches.RemoveAll(x => x.reserveVacuum);

            // DestroyされたGameObjectやComponentのキャッシュも削除する
            vacuumedObjectCount += objectCaches.RemoveAll(x => x.vacuumable && x.gameObject == null);

            WarnTooManyVacuumCount();

            return objectCaches;
        }

        /// <summary>
        /// キャッシュ量が閾値を超えた場合、古いキャッシュ順に削除する
        /// メモリリークを防ぐために使ってね
        /// </summary>
        public List<ComponentCache> VacuumComponentCaches(List<ComponentCache> componentCaches)
        {
            // Vacuum対象だけをListに抽出
            List<ComponentCache> vacuumComponentCaches = componentCaches.FindAll(x => x.vacuumable);

            // キャッシュ量が増えすぎたら、古いものからフラグ(reserveVacuum)をtrueにして削除する
            if (vacuumComponentCaches.Count > MaxComponentCacheCount)
            {
                for (int i = 0; i < vacuumComponentCaches.Count - MaxComponentCacheCount; i++)
                {
                    vacuumComponentCaches[i].SetReserveVacuum(true);
                }
            }
            vacuumedComponentCount += componentCaches.RemoveAll(x => x.reserveVacuum);

            // DestroyされたGameObjectやComponentのキャッシュも削除する
            vacuumedComponentCount += componentCaches.RemoveAll(x => x.vacuumable && x.component == null);

            WarnTooManyVacuumCount();

            return componentCaches;
        }

        public bool IsVacuumable<T>(T component)
        {
            // キャッシュ対象外のComponentを予めここで定義する
            if (component is IIgnoreVacuumComponent)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// VacuumedObjectCountやVacuumedComponentCountが多すぎる場合に警告を出す
        /// </summary>
        private void WarnTooManyVacuumCount()
        {
            if (vacuumedObjectCount > WarnVacuumedObjectCount)
            {
                Debug.LogWarning("VacuumedObjectCount is over " + WarnVacuumedObjectCount + "!");
                vacuumedObjectCount = 0;
            }
            if (vacuumedComponentCount > WarnVacuumedComponentCount)
            {
                Debug.LogWarning("VacuumedComponentCount is over " + WarnVacuumedComponentCount + "!");
                vacuumedComponentCount = 0;
            }
        }
    }
}
