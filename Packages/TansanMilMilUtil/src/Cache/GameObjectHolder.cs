using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// GameObject.Find系の結果をキャッシュするためのクラス。負荷軽減のために使おうね。
    /// </summary>
    [DefaultExecutionOrder(-40)]
    public class GameObjectHolder : SingletonMonoBehaviour<GameObjectHolder>
    {
        /// <summary>
        /// Key: TagName, Value: GameObject
        /// </summary>
        private List<IGameObjectCache> objectCaches = new List<IGameObjectCache>();
        private List<IComponentCache> componentCaches = new List<IComponentCache>();
        private readonly GameObjectHolderVacuumer vacuumer = new GameObjectHolderVacuumer();

        private void Update()
        {
            // 10秒ごとにキャッシュのカウントをログ出力する
            // if (Time.frameCount % 600 == 0)
            // {
            //     DebugExtention.LogTemporary($"GameObjectHolder: objectCaches.Count={objectCaches.Count}, componentCaches.Count={componentCaches.Count}");
            // }
        }

        /// <summary>
        /// 保持しているキャッシュを探して返す。なければGameObject.FindWithTagして結果を返す。
        /// </summary>
        public GameObject FindObjectBy(string tag, bool vacuumable = true)
        {
            VacuumCaches();

            List<IGameObjectCache> caches = objectCaches.FindAll(x => x.tag == tag);
            if (caches.Count >= 2)
            {
                Debug.LogWarning("There are multiple objects with the same tag.");
                return null;
            }

            if (caches.Count == 1)
            {
                if (caches[0] == null)
                {
                    Debug.LogWarning("Cache is null. Maybe the object has been destroyed...??");
                    return null;
                }
                return caches[0].gameObject;
            }
            else
            {
                GameObject obj = GameObject.FindWithTag(tag);
                if (obj == null)
                {
                    return null;
                }

                objectCaches.Add(new GameObjectCache(tag, obj, vacuumable));
                return obj;
            }
        }

        /// <summary>
        /// 保持しているキャッシュを探して返す。なければGameObject.FindWithTagして結果を返す。
        /// </summary>
        public T FindComponentBy<T>(string tag)
        {
            VacuumCaches();

            List<IComponentCache> caches = componentCaches.FindAll(x => x.tag == tag && x.component is T);
            if (caches.Count >= 2)
            {
                Debug.LogWarning("There are multiple components with the same tag.");
                return default;
            }

            if (caches.Count == 1)
            {
                if (caches[0] == null)
                {
                    Debug.LogWarning("Cache is null. Maybe the component has been destroyed...??");
                    return default;
                }
                return (T)caches[0].component;
            }
            else
            {
                GameObject obj = GameObject.FindWithTag(tag);
                if (obj == null)
                {
                    return default;
                }

                T component = obj.GetComponent<T>();
                if (component == null)
                {
                    return default;
                }

                componentCaches.Add(new ComponentCache(tag, obj.GetInstanceID(), (object)component, vacuumer.IsVacuumable(component)));
                return component;
            }
        }

        /// <summary>
        /// 保持しているキャッシュを探して返す。なければtransform.gameObject.GetComponent<T>()して結果を返す。
        /// </summary>
        public T FindComponentBy<T>(Transform transform)
        {
            VacuumCaches();

            if (transform == null)
            {
                return default;
            }

            return FindComponentBy<T>(transform.gameObject);
        }

        /// <summary>
        /// 保持しているキャッシュを探して返す。なければobj.gameObject.GetComponent<T>()して結果を返す。
        /// </summary>
        public T FindComponentBy<T>(GameObject obj)
        {
            VacuumCaches();

            if (obj == null)
            {
                return default;
            }

            List<IComponentCache> caches = componentCaches.FindAll(x => x.gameObjectInstanceID == obj.GetInstanceID() && x.component is T);
            if (caches.Count >= 2)
            {
                throw new Exception("There are multiple components with the same component.");
            }

            if (caches.Count == 1)
            {
                if (caches[0] == null)
                {
                    Debug.LogWarning("Cache is null. Maybe the component has been destroyed...??");
                    return default;
                }
                return (T)caches[0].component;
            }
            else
            {
                T component = obj.GetComponent<T>();
                if (component == null)
                {
                    return default;
                }

                componentCaches.Add(new ComponentCache(obj.tag, obj.GetInstanceID(), component, vacuumer.IsVacuumable(component)));
                return component;
            }
        }

        private void VacuumCaches()
        {
            objectCaches = vacuumer.VacuumObjectCaches(objectCaches);
            componentCaches = vacuumer.VacuumComponentCaches(componentCaches);
        }
    }
}
