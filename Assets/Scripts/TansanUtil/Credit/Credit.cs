namespace TansanMilMil.Util
{
    public class Credit
    {
        public readonly AssetType assetType;
        public readonly string name;
        public readonly LicenseType licenseType;
        public readonly string memo;

        public Credit(AssetType assetType, string name, LicenseType licenseType, string memo = null)
        {
            this.assetType = assetType;
            this.name = name;
            this.licenseType = licenseType;
            this.memo = memo;
        }
    }

    public enum AssetType
    {
        Scenario,
        Graphics,
        Musics,
        Sounds,
        Scripts,
        Fonts,
        Tools,
        SpecialThanks,
    }

    public enum LicenseType
    {
        StandardUnityAssetStoreEULA,
        MIT,
        Original,
        None,
    }
}