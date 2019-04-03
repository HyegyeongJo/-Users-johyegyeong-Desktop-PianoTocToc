using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ToryUX
{
    /// <summary>
    /// Extendable behaviour that can play animations and get back to idle animation.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class IdleBasedAnimationPlayer : MonoBehaviour
    {
        public AnimationClipSpeedPair idleAnimation;
        protected AnimationClipPlayable idleAnimationPlayable;

        protected Animator animator;
        protected PlayableGraph playableGraph;
        protected AnimationPlayableOutput playableOutput;

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();

            playableGraph = PlayableGraph.Create();
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            idleAnimationPlayable = AnimationClipPlayable.Create(playableGraph, idleAnimation.animationClip);

            playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
            playableOutput.SetSourcePlayable(idleAnimationPlayable);
        }

        public virtual void OnEnable()
        {
            PlayIdleAnimation();
        }

        #if UNITY_EDITOR
        public virtual void Update()
        {
            idleAnimationPlayable.SetSpeed(idleAnimation.playSpeed);
        }
        #endif

        public void PlayIdleAnimation()
        {
            idleAnimationPlayable.SetTime(0f);
            idleAnimationPlayable.SetSpeed(idleAnimation.playSpeed);
            playableOutput.SetSourcePlayable(idleAnimationPlayable);
            playableGraph.Play();
        }

        public void PlayAnimation(AnimationPlayableSpeedPair playableSpeedPair)
        {
            if (playAnimationAndGetBackToIdleAnimation != null)
            {
                StopCoroutine(playAnimationAndGetBackToIdleAnimation);
                playAnimationAndGetBackToIdleAnimation = null;
            }

            if (gameObject.activeInHierarchy)
            {
                playAnimationAndGetBackToIdleAnimation = StartCoroutine(PlayAnimationAndGetBackToIdleAnimation(playableSpeedPair));
            }
        }

        Coroutine playAnimationAndGetBackToIdleAnimation;
        IEnumerator PlayAnimationAndGetBackToIdleAnimation(AnimationPlayableSpeedPair playableSpeedPair)
        {
            playableSpeedPair.animationPlayable.SetDuration(playableSpeedPair.animationPlayable.GetAnimationClip().length);
            if (playableSpeedPair.playSpeed >= 0)
            {
                playableSpeedPair.animationPlayable.SetTime(0);
            }
            else
            {
                playableSpeedPair.animationPlayable.SetTime(playableSpeedPair.animationPlayable.GetDuration());
            }
            playableSpeedPair.animationPlayable.SetSpeed(playableSpeedPair.playSpeed);
            playableSpeedPair.animationPlayable.SetDone(false);
            playableOutput.SetSourcePlayable(playableSpeedPair.animationPlayable);
            playableGraph.Play();

            while (!playableSpeedPair.animationPlayable.IsDone())
            {
                yield return new WaitForEndOfFrame();
                if (playableSpeedPair.animationPlayable.GetSpeed() < 0 && playableSpeedPair.animationPlayable.GetTime() <= 0)
                {
                    playableSpeedPair.animationPlayable.SetDone(true);
                }
            }

            PlayIdleAnimation();
            playAnimationAndGetBackToIdleAnimation = null;
        }

        void OnDestroy()
        {
            playableGraph.Destroy();
        }
    }
}