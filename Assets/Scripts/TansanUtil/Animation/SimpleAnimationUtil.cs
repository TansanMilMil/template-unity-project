using UnityEngine;

namespace TansanMilMil.Util
{
    public static class SimpleAnimationUtil
    {
        public static string GetCurrentAnimationName(SimpleAnimation simpleAnimation)
        {
            foreach (var state in simpleAnimation.GetStates())
            {
                if (state.enabled)
                {
                    return state.name;
                }
            }
            return null;
        }
    }
}