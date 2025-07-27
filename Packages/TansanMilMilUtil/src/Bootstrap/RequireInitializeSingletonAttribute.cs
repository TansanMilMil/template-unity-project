using System;

namespace TansanMilMil.Util
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequireInitializeSingletonAttribute : Attribute
    {
        public string Message { get; }

        public RequireInitializeSingletonAttribute(string message = "This class requires initialization before use.")
        {
            Message = message;
        }
    }
}
