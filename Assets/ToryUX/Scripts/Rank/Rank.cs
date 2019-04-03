using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    /// <summary>
    /// A static class to handle game's ranking.
    /// It will work together with one <c>RankUI</c> component in the scene.
    /// </summary>
    public static class Rank
    {
        public static RankUI rankObject;

        /// <summary>
        /// How much time should be passed on each animation interval?
        /// Default value is 1 second.
        /// </summary>
        public static int AnimationStep
        {
            get
            {
                return animationStep;
            }
            set
            {
                animationStep = value;
            }
        }
        private static int animationStep = 1;

        /// <summary>
        /// How many seconds should the animation take for each rank rise/fall animation?
        /// Default value is 1s
        /// </summary>
        /// <value>The animation interval.</value>
        public static float AnimationInterval
        {
            get
            {
                return animationInterval;
            }
            set
            {
                animationInterval = value;
            }
        }
        private static float animationInterval = 1f;

        public static int Ranking
        {
            get
            {
                return ranking;
            }
            set
            {
                ranking = (value > 0) ? value : 1;

                if (rankObject != null && IsUIShown)
                {
                    rankObject.AnimateRank();
                }
            }
        }
        private static int ranking;

        public static void Reset()
        {
            ranking = 0;
        }

        /// <summary>
        /// Makes rank UI appear.
        /// </summary>
        public static void ShowUI()
        {
            if (rankObject != null && !IsUIShown)
            {
                rankObject.gameObject.SetActive(true);
                rankObject.Show();
            }
        }

        /// <summary>
        /// Makes rank UI disappear.
        /// </summary>
        public static void HideUI()
        {
            if (rankObject != null && IsUIShown)
            {
                rankObject.Hide();
            }
        }

        /// <summary>
        /// Returns true if rank UI game object is active.
        /// </summary>
        public static bool IsUIShown
        {
            get
            {
                if (rankObject != null)
                {
                    return rankObject.IsShown;
                }
                else
                {
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning("No rank UI is registered to Rank static class!");
                    #endif
                    return false;
                }
            }
        }
    }
}