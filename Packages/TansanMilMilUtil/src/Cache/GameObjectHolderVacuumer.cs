using System.Collections.Generic;
using System.Linq;
using System;
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
        public List<IGameObjectCache> VacuumObjectCaches(IList<IGameObjectCache> objectCaches)
        {
            // Vacuum対象だけをListに抽出
            List<IGameObjectCache> vacuumObjectCaches = objectCaches.Where(x => x.vacuumable).ToList();

            // キャッシュ量が増えすぎたら、古いものからフラグ(reserveVacuum)をtrueにして削除する
            if (vacuumObjectCaches.Count > MaxObjectCacheCount)
            {
                for (int i = 0; i < vacuumObjectCaches.Count - MaxObjectCacheCount; i++)
                {
                    vacuumObjectCaches[i].SetReserveVacuum(true);
                }
            }
            vacuumedObjectCount += RemoveAllFromList(objectCaches, x => x.reserveVacuum);

            // DestroyされたGameObjectやComponentのキャッシュも削除する
            vacuumedObjectCount += RemoveAllFromList(objectCaches, x => x.vacuumable && x.gameObject == null);

            WarnTooManyVacuumCount();

            return objectCaches as List<IGameObjectCache> ?? objectCaches.ToList();
        }

        /// <summary>
        /// キャッシュ量が閾値を超えた場合、古いキャッシュ順に削除する
        /// メモリリークを防ぐために使ってね
        /// </summary>
        public List<IComponentCache> VacuumComponentCaches(IList<IComponentCache> componentCaches)
        {
            // Vacuum対象だけをListに抽出
            List<IComponentCache> vacuumComponentCaches = componentCaches.Where(x => x.vacuumable).ToList();

            // キャッシュ量が増えすぎたら、古いものからフラグ(reserveVacuum)をtrueにして削除する
            if (vacuumComponentCaches.Count > MaxComponentCacheCount)
            {
                for (int i = 0; i < vacuumComponentCaches.Count - MaxComponentCacheCount; i++)
                {
                    vacuumComponentCaches[i].SetReserveVacuum(true);
                }
            }
            vacuumedComponentCount += RemoveAllFromList(componentCaches, x => x.reserveVacuum);

            // DestroyされたGameObjectやComponentのキャッシュも削除する
            vacuumedComponentCount += RemoveAllFromList(componentCaches, x => x.vacuumable && x.component == null);

            WarnTooManyVacuumCount();

            return componentCaches as List<IComponentCache> ?? componentCaches.ToList();
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

        private int RemoveAllFromList<T>(IList<T> list, Func<T, bool> match)
        {
            int removedCount = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    removedCount++;
                }
            }
            return removedCount;
        }
    }
}
