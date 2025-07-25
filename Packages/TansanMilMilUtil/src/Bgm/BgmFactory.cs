using System.Collections.Generic;
using System.IO;

namespace TansanMilMil.Util
{
    public class BgmFactory : Singleton<BgmFactory>, IBgmFactory
    {
        /// <summary>各BGMのループ範囲を定義する</summary>
        private readonly List<Bgm> musics = new List<Bgm>()
        {
            new Bgm(BgmType.NotChangeBgm, null),
            new Bgm(BgmType.StopBgm, ""),
        };

        /// <summary>
        /// 各ゲームプロジェクト専用のBGMを定義するときに呼ぶ
        /// </summary>
        public void Initialize(List<Bgm> musics)
        {
            if (musics.Exists(m => m.bgmType == BgmType.NotChangeBgm ||
                m.bgmType == BgmType.StopBgm))
            {
                throw new System.ArgumentException("BgmType.NotChangeBgm and BgmType.StopBgm are reserved types and cannot be used in custom BGM definitions.");
            }

            this.musics.AddRange(musics);
        }

        public Bgm Create(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return new Bgm(BgmType.NotChangeBgm, null);
            }

            Bgm music = musics.Find(x => !string.IsNullOrEmpty(x.filePath) &&
                                        Path.GetFileNameWithoutExtension(x.filePath) == fileName);
            return music ?? new Bgm(BgmType.NotChangeBgm, null);
        }
    }
}
