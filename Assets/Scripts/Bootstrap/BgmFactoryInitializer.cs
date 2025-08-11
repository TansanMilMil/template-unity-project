using System.Collections.Generic;
using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class BgmFactoryInitializer
    {
        public static void Initialize()
        {
            var bgmList = new List<Bgm>
            {
                // new Bgm(BgmType.NormalBgm, "BGM/MainTheme", isLoop: true, maxVolume: 0.8f),
                // new Bgm(BgmType.NormalBgm, "BGM/MenuMusic", isLoop: true, maxVolume: 0.6f),
                // new Bgm(BgmType.NormalBgm, "BGM/GameplayMusic", isLoop: true, maxVolume: 0.7f),
                // new Bgm(BgmType.NormalBgm, "BGM/BossMusic", isLoop: true, maxVolume: 0.9f),
                // new Bgm(BgmType.NormalBgm, "BGM/VictoryMusic", isLoop: false, maxVolume: 1.0f)
            };

            BgmFactory.GetInstance().Initialize(bgmList);

            Debug.Log("BgmFactory initialized with project-specific BGM list");
        }
    }
}
