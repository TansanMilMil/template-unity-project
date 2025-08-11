using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    [DefaultExecutionOrder(-1000)]
    public class ProjectInitializer : SingletonMonoBehaviour<ProjectInitializer>
    {
        protected override void OnSingletonStart()
        {
            InitializationBootstrapper.Initialize();
            Destroy(gameObject);
        }
    }
}
