namespace TansanMilMil.Util
{
    public class Bgm
    {
        public BgmType bgmType = BgmType.NotChangeBgm;
        public string filePath = "";
        public bool isLoop = true;
        public const float NoTimeSetting = -1;
        public float initStartTime = 0;
        /// <summary>loopStartTime, loopEndTimeの両方がNoTimeSettingなら曲終わりでループする</summary>
        public float loopStartTime = NoTimeSetting;
        /// <summary>loopStartTime, loopEndTimeの両方がNoTimeSettingなら曲終わりでループする</summary>
        public float loopEndTime = NoTimeSetting;
        public float maxVolume = 1;

        public Bgm(
            BgmType bgmType,
            string filePath,
            bool isLoop = true,
            float initStartTime = 0,
            float loopStartTime = NoTimeSetting,
            float loopEndTime = NoTimeSetting,
            float maxVolume = 1)
        {
            this.bgmType = bgmType;
            this.filePath = filePath;
            this.isLoop = isLoop;
            this.initStartTime = initStartTime;
            this.loopStartTime = loopStartTime;
            this.loopEndTime = loopEndTime;
            this.maxVolume = maxVolume;
        }
    }
}