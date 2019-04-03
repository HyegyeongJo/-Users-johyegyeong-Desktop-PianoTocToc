using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ToryUX
{
    /// <summary>
    /// Extended class from <c>IdleBaseAnimationPlayer</c>.
    /// Can have <c>riseAnimationClip</c> and <c>fallAnimationClip</c> to be played.
    /// </summary>
    public class RankAnimationPlayer : IdleBasedAnimationPlayer
    {
        public AnimationClipSpeedPair riseAnimation;
        private AnimationClipPlayable riseAnimationPlayable;
        private bool isRiseAnimationAvailable = true;
        private AnimationPlayableSpeedPair riseAnimationPlayableSpeedPair;

        public AnimationClipSpeedPair fallAnimation;
        private AnimationClipPlayable fallAnimationPlayable;
        private bool isFallAnimationAvailable = true;
        private AnimationPlayableSpeedPair fallAnimationPlayableSpeedPair;

        public override void Awake()
        {
            base.Awake();

            riseAnimationPlayable = AnimationClipPlayable.Create(playableGraph, riseAnimation.animationClip);
            riseAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(riseAnimationPlayable, riseAnimation.playSpeed);

            fallAnimationPlayable = AnimationClipPlayable.Create(playableGraph, fallAnimation.animationClip);
            fallAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(fallAnimationPlayable, fallAnimation.playSpeed);

            if (riseAnimation.animationClip == null)
            {
                isRiseAnimationAvailable = false;
            }
            if (fallAnimation.animationClip == null)
            {
                isFallAnimationAvailable = false;
            }
        }

        public void PlayRiseAnimation()
        {
#if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isRiseAnimationAvailable = riseAnimation.animationClip != null;
            riseAnimationPlayableSpeedPair.playSpeed = riseAnimation.playSpeed;
#endif

            if (isRiseAnimationAvailable)
            {
                PlayAnimation(riseAnimationPlayableSpeedPair);
            }
        }

        public void PlayFallAnimation()
        {
#if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isFallAnimationAvailable = fallAnimation.animationClip != null;
            fallAnimationPlayableSpeedPair.playSpeed = fallAnimation.playSpeed;
#endif

            if (isFallAnimationAvailable)
            {
                PlayAnimation(fallAnimationPlayableSpeedPair);
            }
        }
    }
}