using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class RankUI : MonoBehaviour
    {
        public bool shouldBeHiddenOnStart = true;

        public AudioClip rankingRiseSfx;
        public AudioClip rankingFallSfx;

        public ParticleSystem rankingRiseVfx;
        public ParticleSystem rankingFallVfx;

        public Text rankingText;
        public Text rankingPostfixText;
        public bool useSeparatedRankingText;

        public Color textPostfixTextColor;
        [Range(0f, 1f)]
        public float textPostfixScale = 0.5f;

        private int showingRanking;

        private RankUIWrapperAnimationPlayer[] rankUIWrapperAnimationPlayers;
        private RankAnimationPlayer[] rankingAnimations;

        public bool IsShown
        {
            get
            {
                if (rankUIWrapperAnimationPlayers.Length <= 0)
                {
                    return false;
                }
                else
                {
                    return rankUIWrapperAnimationPlayers[0].gameObject.activeInHierarchy;
                }
            }
        }

        void Awake()
        {
            // Register this object to ToryUX.Rank
            if (Rank.rankObject != null && Rank.rankObject != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("RankUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                Rank.rankObject = this;
            }

            // Collect animation players of children.
            rankUIWrapperAnimationPlayers = GetComponentsInChildren<RankUIWrapperAnimationPlayer>(true);
            rankingAnimations = GetComponentsInChildren<RankAnimationPlayer>(true);

            rankingText = GetComponentInChildren<TagClasses.RankUIRankingTextObject>(true).GetComponent<Text>();
            rankingPostfixText = GetComponentInChildren<TagClasses.RankUIRankingPostfixTextObject>(true).GetComponent<Text>();

            // Hide if needed.
            if (shouldBeHiddenOnStart)
            {
                for (int i = 0; i < rankUIWrapperAnimationPlayers.Length; i++)
                {
                    rankUIWrapperAnimationPlayers[i].gameObject.SetActive(false);
                }
            }
        }

        public void Show()
        {
            UpdateRank();

            for (int i = 0; i < rankUIWrapperAnimationPlayers.Length; i++)
            {
                rankUIWrapperAnimationPlayers[i].gameObject.SetActive(true);
                rankUIWrapperAnimationPlayers[i].PlayShowAnimation();
            }
            rankAnimationCoroutine = StartCoroutine(RankAnimationCoroutine());
        }

        public void Hide()
        {
            for (int i = 0; i < rankUIWrapperAnimationPlayers.Length; i++)
            {
                if (rankUIWrapperAnimationPlayers[i].isActiveAndEnabled)
                {
                    rankUIWrapperAnimationPlayers[i].PlayHideAnimation();
                }
            }
        }

        public void UpdateRank()
        {
            showingRanking = Rank.Ranking;
            rankingText.text = showingRanking.ToString();

            if (useSeparatedRankingText)
            {
                // Apply text, size of rankingPostfixText
                if (showingRanking % 10 == 1 && showingRanking % 100 != 11)
                {
                    rankingPostfixText.text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "st");
                }
                else if (showingRanking % 10 == 2 && showingRanking % 100 != 12)
                {
                    rankingPostfixText.text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "nd");
                }
                else if (showingRanking % 10 == 3 && showingRanking % 100 != 13)
                {
                    rankingPostfixText.text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "rd");
                }
                else
                {
                    rankingPostfixText.text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "th");
                }
                return;
            }
            else
            {
                // Apply text, size, color of postfix text
                if (showingRanking % 10 == 1 && showingRanking % 100 != 11)
                {
                    rankingText.text += string.Format("<size={0}><color=#{1}>{2}</color></size>", rankingText.fontSize * textPostfixScale, ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "st");
                }
                else if (showingRanking % 10 == 2 && showingRanking % 100 != 12)
                {
                    rankingText.text += string.Format("<size={0}><color=#{1}>{2}</color></size>", rankingText.fontSize * textPostfixScale, ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "nd");
                }
                else if (showingRanking % 10 == 3 && showingRanking % 100 != 13)
                {
                    rankingText.text += string.Format("<size={0}><color=#{1}>{2}</color></size>", rankingText.fontSize * textPostfixScale, ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "rd");
                }
                else
                {
                    rankingText.text += string.Format("<size={0}><color=#{1}>{2}</color></size>", rankingText.fontSize * textPostfixScale, ColorUtility.ToHtmlStringRGBA(textPostfixTextColor), "th");
                }
            }
        }

        public void AnimateRank()
        {
            if (rankAnimationCoroutine == null)
            {
                rankAnimationCoroutine = StartCoroutine(RankAnimationCoroutine());
            }
        }

        Coroutine rankAnimationCoroutine;
        IEnumerator RankAnimationCoroutine()
        {
            while (showingRanking != Rank.Ranking)
            {
                if (showingRanking > Rank.Ranking) // rise
                {
                    UpdateRank();

                    // Animations and effects.
                    for (int i = 0; i < rankingAnimations.Length; i++)
                    {
                        rankingAnimations[i].PlayRiseAnimation();
                    }
                    if (rankingRiseSfx != null)
                    {
                        UISound.Play(rankingRiseSfx);
                    }
                    if (rankingRiseVfx != null)
                    {
                        rankingRiseVfx.Play();
                    }
                    yield break;
                }
                else if (showingRanking < Rank.Ranking) // fall
                {
                    UpdateRank();

                    // Animations and effects.
                    for (int i = 0; i < rankingAnimations.Length; i++)
                    {
                        rankingAnimations[i].PlayFallAnimation();
                    }
                    if (rankingFallSfx != null)
                    {
                        UISound.Play(rankingFallSfx);
                    }
                    if (rankingFallVfx != null)
                    {
                        rankingFallVfx.Play();
                    }
                    yield break;
                }

                // Wait for designated seconds before next iteration.
                yield return new WaitForSeconds(Rank.AnimationInterval);
            }

            yield return new WaitForSeconds(Rank.AnimationInterval);
            rankAnimationCoroutine = null;
        }

        void OnDestroy()
        {
            if (Rank.rankObject == this)
            {
                Rank.rankObject = null;
            }
        }
    }
}