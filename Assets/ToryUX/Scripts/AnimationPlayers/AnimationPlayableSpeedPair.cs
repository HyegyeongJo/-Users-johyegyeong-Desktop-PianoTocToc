using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace ToryUX
{
    [System.Serializable]
    public struct AnimationPlayableSpeedPair
    {
        public AnimationClipPlayable animationPlayable;
        public float playSpeed;

        public AnimationPlayableSpeedPair(AnimationClipPlayable playable, float speed)
        {
            animationPlayable = playable;
            playSpeed = speed;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", animationPlayable.GetAnimationClip().ToString(), playSpeed.ToString());
        }
    }
}