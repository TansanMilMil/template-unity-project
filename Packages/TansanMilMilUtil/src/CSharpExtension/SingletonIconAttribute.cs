using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// Singletonコンポーネントにアイコンを表示するためのAttribute
    /// </summary>
    public class SingletonIconAttribute : PropertyAttribute
    {
        public string iconName;
        public string tooltipText;

        public SingletonIconAttribute(string iconName = "d_Settings", string tooltipText = "This is a Singleton Component")
        {
            this.iconName = iconName;
            this.tooltipText = tooltipText;
        }
    }
}