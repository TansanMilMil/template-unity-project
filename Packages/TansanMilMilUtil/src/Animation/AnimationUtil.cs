using UnityEngine;

namespace TansanMilMil.Util
{
    public static class AnimationUtil
    {
        public static string GetCurrentAnimationName(Animator animator)
        {
            if (animator == null)
                return null;

            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfos.Length > 0)
            {
                return clipInfos[0].clip.name;
            }

            return null;
        }

        public static string GetCurrentAnimationName(Animation animation)
        {
            if (animation == null)
                return null;

            foreach (AnimationState state in animation)
            {
                if (animation.IsPlaying(state.name))
                {
                    return state.name;
                }
            }
            return null;
        }

        public static bool IsAnimationPlaying(Animator animator, string animationName)
        {
            if (animator == null)
                return false;

            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            foreach (var clipInfo in clipInfos)
            {
                if (clipInfo.clip.name == animationName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAnimationPlaying(Animation animation, string animationName)
        {
            if (animation == null)
                return false;

            return animation.IsPlaying(animationName);
        }

        public static void PlayAnimation(Animator animator, string animationName)
        {
            if (animator != null)
            {
                animator.Play(animationName);
            }
        }

        public static void PlayAnimation(Animation animation, string animationName)
        {
            if (animation != null)
            {
                animation.Play(animationName);
            }
        }

        public static void StopAnimation(Animator animator)
        {
            if (animator != null)
            {
                animator.enabled = false;
            }
        }

        public static void StopAnimation(Animation animation)
        {
            if (animation != null)
            {
                animation.Stop();
            }
        }
    }
}
