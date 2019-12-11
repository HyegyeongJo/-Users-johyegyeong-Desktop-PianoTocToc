using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace ToryUX
{
    /// <summary>
    /// Extended class from <c>IdleBaseAnimationPlayer</c>.
    /// Can have <c>gainAnimationClip</c> and <c>loseAnimationClip</c> to be played.
    /// </summary>
    public class ScorePointAnimationPlayer : IdleBasedAnimationPlayer
    {
        public AnimationClipSpeedPair gainAnimation;
        private AnimationClipPlayable gainAnimationPlayable;
        private bool isGainAnimationAvailable = true;
        private AnimationPlayableSpeedPair gainAnimationPlayableSpeedPair;

        public AnimationClipSpeedPair loseAnimation;
        private AnimationClipPlayable loseAnimationPlayable;
        private bool isLoseAnimationAvailable = true;
        private AnimationPlayableSpeedPair loseAnimationPlayableSpeedPair;

        public override void Awake()
        {
            base.Awake();

            gainAnimationPlayable = AnimationClipPlayable.Create(playableGraph, gainAnimation.animationClip);
            gainAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(gainAnimationPlayable, gainAnimation.playSpeed);

            loseAnimationPlayable = AnimationClipPlayable.Create(playableGraph, loseAnimation.animationClip);
            loseAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(loseAnimationPlayable, loseAnimation.playSpeed);

            if (gainAnimation.animationClip == null)
            {
                isGainAnimationAvailable = false;
            }
            if (loseAnimation.animationClip == null)
            {
                isLoseAnimationAvailable = false;
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

        public void PlayLoseAnimation()
        {
            #if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isLoseAnimationAvailable = loseAnimation.animationClip != null;
            loseAnimationPlayableSpeedPair.playSpeed = loseAnimation.playSpeed;
            #endif

            if (isLoseAnimationAvailable)
            {
                PlayAnimation(loseAnimationPlayableSpeedPair);
            }
        }
    }
}