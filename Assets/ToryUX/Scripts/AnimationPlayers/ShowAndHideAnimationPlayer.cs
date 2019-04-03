using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ToryUX
{
    /// <summary>
    /// Extendable behaviour that plays show animation on enable and deactivates after playing hide animation.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ShowAndHideAnimationPlayer : MonoBehaviour
    {
        public AnimationClipSpeedPair showAnimation;
        protected AnimationClipPlayable showAnimationPlayable;

        [Space]
        public AnimationClipSpeedPair hideAnimation;
        protected AnimationClipPlayable hideAnimationPlayable;

        protected Animator animator;
        protected PlayableGraph playableGraph;
        protected AnimationPlayableOutput playableOutput;

        bool shouldDeactivateAfterHideAnimation;

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();

            playableGraph = PlayableGraph.Create();
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            showAnimationPlayable = AnimationClipPlayable.Create(playableGraph, showAnimation.animationClip);
            hideAnimationPlayable = AnimationClipPlayable.Create(playableGraph, hideAnimation.animationClip);

            playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
        }

        public virtual void OnEnable()
        {
            PlayShowAnimation();
        }

        public void PlayShowAnimation()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            else if (showAnimation.animationClip != null)
            {
                if (playHideAnimationCoroutine != null)
                {
                    StopCoroutine(playHideAnimationCoroutine);
                    playHideAnimationCoroutine = null;
                }
                showAnimationPlayable.SetDuration(showAnimation.animationClip.length);
                showAnimationPlayable.SetTime(0);
                showAnimationPlayable.SetSpeed(showAnimation.playSpeed);
                showAnimationPlayable.SetDone(false);
                playableOutput.SetSourcePlayable(showAnimationPlayable);
                playableGraph.Play();
            }
        }

        public void PlayHideAnimation(bool shouldDeactivateAfterward = true)
        {
            if (gameObject.activeInHierarchy)
            {
                shouldDeactivateAfterHideAnimation = shouldDeactivateAfterward;
                if (playHideAnimationCoroutine == null)
                {
                    playHideAnimationCoroutine = StartCoroutine(PlayHideAnimationCoroutine());
                }
            }
        }

        Coroutine playHideAnimationCoroutine;
        IEnumerator PlayHideAnimationCoroutine()
        {
            if (hideAnimation.animationClip != null)
            {
                hideAnimationPlayable.SetDuration(hideAnimation.animationClip.length);
                if (hideAnimation.playSpeed >= 0)
                {
                    hideAnimationPlayable.SetTime(0);
                }
                else
                {
                    hideAnimationPlayable.SetTime(hideAnimationPlayable.GetDuration());
                }
                hideAnimationPlayable.SetSpeed(hideAnimation.playSpeed);
                hideAnimationPlayable.SetDone(false);
                playableOutput.SetSourcePlayable(hideAnimationPlayable);
                playableGraph.Play();

                while (!hideAnimationPlayable.IsDone())
                {
                    yield return new WaitForEndOfFrame();
                    if (hideAnimationPlayable.GetSpeed() < 0 && hideAnimationPlayable.GetTime() <= 0)
                    {
                        hideAnimationPlayable.SetDone(true);
                    }
                }
            }

            if (shouldDeactivateAfterHideAnimation)
            {
                gameObject.SetActive(false);
            }

            playHideAnimationCoroutine = null;
        }

        void OnDestroy()
        {
            playableGraph.Destroy();
        }
    }
}