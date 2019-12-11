using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ToryUX
{
    /// <summary>
    /// Extended class from <c>IdleBaseAnimationPlayer</c>.
    /// Can have <c>tickAnimationClip</c>, <c>tockAnimationClip</c> and <c>alertAnimationClip</c> to be played.
    /// </summary>
    public class TimerAnimationPlayer : IdleBasedAnimationPlayer
    {
        public AnimationClipSpeedPair tickAnimation;
        private AnimationClipPlayable tickAnimationPlayable;
        private bool isTickAnimationAvailable = true;
        private AnimationPlayableSpeedPair tickAnimationPlayableSpeedPair;

        public AnimationClipSpeedPair tockAnimation;
        private AnimationClipPlayable tockAnimationPlayable;
        private bool isTockAnimationAvailable = true;
        private AnimationPlayableSpeedPair tockAnimationPlayableSpeedPair;

        public AnimationClipSpeedPair alertAnimation;
        private AnimationClipPlayable alertAnimationPlayable;
        private bool isAlertAnimationAvailable = true;
        private AnimationPlayableSpeedPair alertAnimationPlayableSpeedPair;

        public override void Awake()
        {
            base.Awake();

            tickAnimationPlayable = AnimationClipPlayable.Create(playableGraph, tickAnimation.animationClip);
            tickAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(tickAnimationPlayable, tickAnimation.playSpeed);

            tockAnimationPlayable = AnimationClipPlayable.Create(playableGraph, tockAnimation.animationClip);
            tockAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(tockAnimationPlayable, tockAnimation.playSpeed);

            alertAnimationPlayable = AnimationClipPlayable.Create(playableGraph, alertAnimation.animationClip);
            alertAnimationPlayableSpeedPair = new AnimationPlayableSpeedPair(alertAnimationPlayable, alertAnimation.playSpeed);

            if (tickAnimation.animationClip == null)
            {
                isTickAnimationAvailable = false;
            }
            if (tockAnimation.animationClip == null)
            {
                isTockAnimationAvailable = false;
            }
            if (alertAnimation.animationClip == null)
            {
                isAlertAnimationAvailable = false;
            }
        }

        public void PlayTickAnimation()
        {
#if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isTickAnimationAvailable = tickAnimation.animationClip != null;
            tickAnimationPlayableSpeedPair.playSpeed = tickAnimation.playSpeed;
#endif

            if (isTickAnimationAvailable)
            {
                PlayAnimation(tickAnimationPlayableSpeedPair);
            }
        }

        public void PlayTockAnimation()
        {
#if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isTockAnimationAvailable = tockAnimation.animationClip != null;
            tockAnimationPlayableSpeedPair.playSpeed = tockAnimation.playSpeed;
#endif

            if (isTockAnimationAvailable)
            {
                PlayAnimation(tockAnimationPlayableSpeedPair);
            }
        }

        public void PlayAlertAnimation()
        {
#if UNITY_EDITOR
            // Apply possible changes via inspector window.
            isAlertAnimationAvailable = alertAnimation.animationClip != null;
            alertAnimationPlayableSpeedPair.playSpeed = alertAnimation.playSpeed;
#endif

            if (isAlertAnimationAvailable)
            {
                PlayAnimation(alertAnimationPlayableSpeedPair);
            }
        }
    }
}