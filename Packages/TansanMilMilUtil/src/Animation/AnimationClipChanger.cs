using UnityEngine;

namespace TansanMilMil.Util
{
    public class AnimationClipChanger : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private string targetStateName = "New State";
        private AnimatorOverrideController overrideController;

        void Start()
        {
            // 元のAnimator ControllerをもとにAnimator Override Controllerを生成
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
        }

        public void ChangeAnimationClip(AnimationClip newClip)
        {
            // "New State"は元Animator Controllerのステートに登録してあるAnimation Clip名
            overrideController[targetStateName] = newClip;

            animator.Play(targetStateName, 0, 0f);
        }
    }
}
