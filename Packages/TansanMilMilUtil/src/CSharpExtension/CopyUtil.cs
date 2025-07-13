using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TansanMilMil.Util
{
    public static class CopyUtil
    {
        public static T DeepCopy<T>(this T src)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter
                  = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, src); // シリアライズ
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream); // デシリアライズ
            }
        }
    }
}