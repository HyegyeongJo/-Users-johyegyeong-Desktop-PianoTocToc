using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace ToryUX
{
    [System.Serializable]
    public struct AnimationClipSpeedPair
    {
        public AnimationClip animationClip;
        public float playSpeed;

        public AnimationClipSpeedPair(AnimationClip clip, float speed)
        {
            animationClip = clip;
            playSpeed = speed;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", animationClip.ToString(), playSpeed.ToString());
        }
    }
}