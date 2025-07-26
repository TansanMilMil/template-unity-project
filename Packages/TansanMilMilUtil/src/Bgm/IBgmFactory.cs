using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public interface IBgmFactory
    {
        void Initialize(List<Bgm> musics);
        Bgm Create(string fileName);
    }
}