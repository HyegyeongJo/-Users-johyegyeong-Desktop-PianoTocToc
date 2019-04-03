using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace ToryUX
{
    public class ComboPointAnimationPlayer : IdleBasedAnimationPlayer
    {
        public AnimationClipSpeedPair gainAnimation;
        private AnimationClipPlayable gainAnimationPlayable;
        private bool isGainAnimationAvailable = true;
        private AnimationPlayableSpeedPair gainAnimationPlayableSpeedPair;

        public AnimationClipSpeedPair failAnimation;
        private AnimationClipPlayable failAnimationPlayable;
        private bool isFailAnimationAvailable = true;
        private AnimationPlayableSpeedPair failAnimationPlayableSpeedPair;

        public override void Awake()
        {
            base.Awake();

            gainAnimationPlayable = AnimationClipPlayable.Create(playableGraph, gainAnimation.animationClip);
            gainAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(gainAnimationPlayable, gainAnimation.playSpeed);

            failAnimationPlayable = AnimationClipPlayable.Create(playableGraph, failAnimation.animationClip);
            failAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(failAnimationPlayable, failAnimation.playSpeed);

            if (gainAnimation.animationClip == null)
            {
                isGainAnimationAvailable = false;
            }
            if (failAnimation.animationClip == null)
            {
                isFailAnimationAvailable = false;
            }
        }

        public void PlayGainAnimation()
        {
            #if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isGainAnimationAvailable = gainAnimation.animationClip != null;
            gainAnimationPlayableSpeedPair.playSpeed = gainAnimation.playSpeed;
            #endif

            if (isGainAnimationAvailable)
            {
                PlayAnimation(gainAnimationPlayableSpeedPair);
            }
        }

        public void PlayFailAnimation()
        {
            #if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isFailAnimationAvailable = failAnimation.animationClip != null;
            failAnimationPlayableSpeedPair.playSpeed = failAnimation.playSpeed;
            #endif

            if (isFailAnimationAvailable)
            {
                PlayAnimation(failAnimationPlayableSpeedPair);
            }
        }
    }
}