using System.Collections.Generic;
using System.IO;

namespace TansanMilMil.Util
{
    public class BgmFactory : FactoryBase<Bgm, string>
    {
        /// <summary>各BGMのループ範囲を定義する</summary>
        private static List<Bgm> AllMusics = new List<Bgm>()
        {
            new Bgm(BgmType.NotChangeBgm, null),
            new Bgm(BgmType.StopBgm, ""),
            new Bgm(BgmType.Title, "Assets/Musics/music_title_audiostock_1162727.mp3"),
            // new Music() {
            //      fileName = BgmPaths.ExtractOnlyFileNameWithoutExtension(BgmPaths.GetBgmPath(Bgm.Title)),
            //      loopStartTime = 0.316f,
            //      loopEndTime = 102.710f,
            // },

        };

        public override Bgm Create(string fileName)
        {
            Bgm music = AllMusics.Find(x => Path.GetFileNameWithoutExtension(x.filePath) == fileName);
            if (music == null)
            {
                return new Bgm(BgmType.NotChangeBgm, null);
            }
            else
            {
                return music;
            }
        }

        public static string GetBgmPath(BgmType bgmPath)
        {
            return AllMusics.Find(x => x.bgmType == bgmPath).filePath;
        }
    }
}