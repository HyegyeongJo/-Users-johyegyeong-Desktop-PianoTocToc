using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ToryUX
{
    public class ComboUIWrapperAnimationPlayer : ShowAndHideAnimationPlayer, IComboUIWrapperAnimationPlayer
    {
        public bool ShouldShowWithScoreUI
        {
            get
            {
                return false;
            }
        }

        [Space]
        public AnimationClipSpeedPair failAnimation;
        private AnimationClipPlayable failAnimationPlayable;

        public override void Awake()
        {
            base.Awake();

            failAnimationPlayable = AnimationClipPlayable.Create(playableGraph, failAnimation.animationClip);

            if (failAnimation.animationClip == null)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarningFormat("ScoreAnimationPlayer '{0}' does not have AnimationClip for failAnimation.", name);
                #endif
            }
        }

        public void PlayActivateAnimation()
        {
            base.PlayShowAnimation();
        }

        public void PlayDeactivateAnimation()
        {
            base.PlayHideAnimation();
        }

        public void PlayFailAnimationAlone()
        {
            failAnimationPlayable.SetDuration(failAnimation.animationClip.length);
            if (failAnimation.playSpeed >= 0)
            {
                failAnimationPlayable.SetTime(0);
            }
            else
            {
                failAnimationPlayable.SetTime(failAnimationPlayable.GetDuration());
            }
            failAnimationPlayable.SetSpeed(failAnimation.playSpeed);
            failAnimationPlayable.SetDone(false);
            playableOutput.SetSourcePlayable(failAnimationPlayable);
            playableGraph.Play();
        }

        public void PlayFailAnimationAndHide(bool shouldDeactivateAfterward = true)
        {
            if (playFailAnimationAndContinueToHideAnimation != null)
            {
                StopCoroutine(playFailAnimationAndContinueToHideAnimation);
            }
            playFailAnimationAndContinueToHideAnimation = StartCoroutine(PlayFailAnimationAndContinueToHideAnimation(shouldDeactivateAfterward));
        }

        Coroutine playFailAnimationAndContinueToHideAnimation;
        IEnumerator PlayFailAnimationAndContinueToHideAnimation(bool shouldDeactivateAfterward)
        {
            if (failAnimation.animationClip != null)
            {
                PlayFailAnimationAlone();

                while (!failAnimationPlayable.IsDone())
                {
                    yield return new WaitForEndOfFrame();
                    if (failAnimationPlayable.GetSpeed() < 0 && failAnimationPlayable.GetTime() <= 0)
                    {
                        failAnimationPlayable.SetDone(true);
                    }
                }
            }

            PlayHideAnimation(shouldDeactivateAfterward);
            playFailAnimationAndContinueToHideAnimation = null;
        }
    }
}