namespace TansanMilMil.Util
{
    public class Credit
    {
        public readonly CreditAssetType assetType;
        public readonly string name;
        public readonly LicenseType licenseType;
        public readonly string memo;

        public Credit(CreditAssetType assetType, string name, LicenseType licenseType, string memo = null)
        {
            this.assetType = assetType;
            this.name = name;
            this.licenseType = licenseType;
            this.memo = memo;
        }
    }
}
