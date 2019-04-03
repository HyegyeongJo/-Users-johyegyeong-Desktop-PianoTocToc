using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(ScorePointAnimationPlayer), typeof(Text))]
    public class ResultScoreCounter : MonoBehaviour
    {
        void OnEnable()
        {
            if (Leaderboard.RecordType == LeaderboardRecordType.Score)
            {
                if (countScoreCoroutine != null)
                {
                    StopCoroutine(countScoreCoroutine);
                    countScoreCoroutine = null;
                }
                countScoreCoroutine = StartCoroutine(CountScoreCoroutine());
            }
            else
            {
                if (countTimeCoroutine != null)
                {
                    StopCoroutine(countTimeCoroutine);
                    countTimeCoroutine = null;
                }
                countTimeCoroutine = StartCoroutine(CountTimeCoroutine());
            }
        }

        Coroutine countScoreCoroutine;
        IEnumerator CountScoreCoroutine()
        {
            var uiText = GetComponent<Text>();
            var animationPlayer = GetComponent<ScorePointAnimationPlayer>();
            uiText.text = "";

            yield return new WaitForSeconds(1f);

            int showingScore = Score.MinimumScorePoint;
            float animationStep;
            if ((float) Score.CurrentScorePoint / (float) Score.AnimationStep * (float) Score.AnimationInterval > (ResultUI.Instance.maxDurationCountingResultScore * 60f))
            {
                animationStep = (float) Score.CurrentScorePoint * (float) Score.AnimationInterval / (ResultUI.Instance.maxDurationCountingResultScore * 60f);
            }
            else
            {
                animationStep = (float) Score.AnimationStep;
            }
            while (showingScore < Score.CurrentScorePoint)
            {
                uiText.text = showingScore.ToString();
                animationPlayer.PlayGainAnimation();
                if (ResultUI.Instance.countingResultScoreSfx != null)
                {
                    UISound.Play(ResultUI.Instance.countingResultScoreSfx);
                }
                for (int i = 0; i < Score.AnimationInterval; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

                showingScore = Mathf.RoundToInt(showingScore + animationStep);
            }
            showingScore = Score.CurrentScorePoint;
            uiText.text = showingScore.ToString();
            animationPlayer.PlayGainAnimation();

            countScoreCoroutine = null;
        }

        Coroutine countTimeCoroutine;
        IEnumerator CountTimeCoroutine()
        {
            var uiText = GetComponent<Text>();
            var animationPlayer = GetComponent<ScorePointAnimationPlayer>();
            uiText.text = "";

            yield return new WaitForSeconds(1f);

            float showingTime = 0f;
            float animationStep;
            if ((float) Timer.CurrentTime / (float) Timer.AnimationStep * (float) Score.AnimationInterval > (ResultUI.Instance.maxDurationCountingResultScore * 60f))
            {
                animationStep = (float) Timer.CurrentTime * (float) Score.AnimationInterval / (ResultUI.Instance.maxDurationCountingResultScore * 60f);
            }
            else
            {
                animationStep = (float) Timer.AnimationStep;
            }
            while (showingTime < Timer.CurrentTime)
            {
                uiText.text = TimerUI.SecondsToTimespanString(showingTime, true, uiText.fontSize * .75f);
                animationPlayer.PlayGainAnimation();
                if (ResultUI.Instance.countingResultScoreSfx != null)
                {
                    UISound.Play(ResultUI.Instance.countingResultScoreSfx);
                }
                for (int i = 0; i < Score.AnimationInterval; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

                showingTime = showingTime + animationStep;
            }
            showingTime = Timer.CurrentTime;
            uiText.text = TimerUI.SecondsToTimespanString(showingTime, true, uiText.fontSize * .75f);
            animationPlayer.PlayGainAnimation();
        }
    }
}