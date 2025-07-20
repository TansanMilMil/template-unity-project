using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TansanMilMil.Util
{
    public class PrevSceneHolder : SingletonMonoBehaviour<PrevSceneHolder>
    {
        private List<string> _prevScenes = new List<string>();
        /// <summary>
        /// 現在のSceneがloadされる前に表示していたScene名たち。
        /// </summary>
        public ReadOnlyCollection<string> prevScenes
        {
            get { return _prevScenes.AsReadOnly(); }
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
