namespace TansanMilMil.Util
{
    public interface IComponentCache
    {
        string tag { get; }
        int gameObjectInstanceID { get; }
        object component { get; }
        bool vacuumable { get; }
        bool reserveVacuum { get; }
        void SetReserveVacuum(bool reserveVacuum);
    }
}