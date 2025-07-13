using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class PrevSceneHolder : MonoBehaviour
    {
        private static GameObject Instance;
        private static PrevSceneHolder InstanceComponent;
        private List<string> _prevScenes = new List<string>();
        /// <summary>
        /// 現在のSceneがloadされる前に表示していたScene名たち。
        /// </summary>
        public ReadOnlyCollection<string> prevScenes
        {
            get { return _prevScenes.AsReadOnly(); }
        }

        private PrevSceneHolder() { }

        public static PrevSceneHolder GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("PrevSceneHolder.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("PrevSceneHolder.InstanceComponent is null!");
            }
            return InstanceComponent;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                // すでにロードされていたら自分自身を破棄して終了
                Destroy(gameObject);
                return;
            }
            else
            {
                // ロードされていなかったら、フラグをロード済みに設定する
                Instance = gameObject;
                InstanceComponent = gameObject.GetComponent<PrevSceneHolder>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AddPrevScene(scene.name);
        }

        private void AddPrevScene(string sceneName)
        {
            if (_prevScenes.Count >= 10)
            {
                _prevScenes.RemoveAt(_prevScenes.Count - 1);
            }
            _prevScenes.Insert(0, sceneName);
        }
    }
}