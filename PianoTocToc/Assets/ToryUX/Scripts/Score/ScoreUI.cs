using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class ScoreUI : MonoBehaviour
    {
        public bool shouldBeHiddenOnStart = true;

        public AudioClip scoreGainSfx;
        public AudioClip scoreLoseSfx;
        public AudioClip comboStartSfx;
        public AudioClip comboGainSfx;
        public AudioClip comboFailSfx;

        public ParticleSystem scoreGainVfx;
        public ParticleSystem scoreLoseVfx;
        public ParticleSystem comboStartVfx;
        public ParticleSystem comboGainVfx;
        public ParticleSystem comboFailVfx;

        public Text scorePointText;
        public Text comboPointText;

        private int showingScorePoints;

        private ScoreUIWrapperAnimationPlayer[] scoreUIWrapperAnimationPlayers;
        private IComboUIWrapperAnimationPlayer[] comboUIWrapperAnimationPlayers;
        private ScorePointAnimationPlayer[] scorePointAnimations;
        private ComboPointAnimationPlayer[] comboPointAnimations;

        public bool IsShown
        {
            get
            {
                if (scoreUIWrapperAnimationPlayers.Length <= 0)
                {
                    return false;
                }
                else
                {
                    return scoreUIWrapperAnimationPlayers[0].gameObject.activeInHierarchy;
                }
            }
        }

        void Awake()
        {
            // Register this object to ToryUX.Score.
            if (Score.scoreObject != null && Score.scoreObject != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("ScoreUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                Score.scoreObject = this;
            }

            // Initialize score.
            showingScorePoints = Score.MinimumScorePoint;
            scorePointText.text = showingScorePoints.ToString();
            comboPointText.text = Score.CurrentComboPoint.ToString();

            // Collect animation players of children.
            scoreUIWrapperAnimationPlayers = GetComponentsInChildren<ScoreUIWrapperAnimationPlayer>(true);
            comboUIWrapperAnimationPlayers = GetComponentsInChildren<IComboUIWrapperAnimationPlayer>(true);
            scorePointAnimations = GetComponentsInChildren<ScorePointAnimationPlayer>(true);
            comboPointAnimations = GetComponentsInChildren<ComboPointAnimationPlayer>(true);

            scorePointText = GetComponentInChildren<TagClasses.ScoreUIScoreTextObject>(true).GetComponent<Text>();
            comboPointText = GetComponentInChildren<TagClasses.ScoreUIComboTextObject>(true).GetComponent<Text>();

            // Hide combo UI objects.
            for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
            {
                if (!(comboUIWrapperAnimationPlayers[i] is EchoComboUIWrapperAnimationPlayer))
                {
                    (comboUIWrapperAnimationPlayers[i] as MonoBehaviour).gameObject.SetActive(false);
                }
            }

            // Hide if needed.
            if (shouldBeHiddenOnStart)
            {
                for (int i = 0; i < scoreUIWrapperAnimationPlayers.Length; i++)
                {
                    scoreUIWrapperAnimationPlayers[i].gameObject.SetActive(false);
                }
            }
        }

        public void Show()
        {
            showingScorePoints = Score.CurrentScorePoint;
            scorePointText.text = showingScorePoints.ToString();
            for (int i = 0; i < scoreUIWrapperAnimationPlayers.Length; i++)
            {
                scoreUIWrapperAnimationPlayers[i].gameObject.SetActive(true);
                scoreUIWrapperAnimationPlayers[i].PlayShowAnimation();
            }

            for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
            {
                (comboUIWrapperAnimationPlayers[i] as MonoBehaviour).gameObject.SetActive(comboUIWrapperAnimationPlayers[i].ShouldShowWithScoreUI);
            }
        }

        public void ShowCombo()
        {
            for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
            {
                (comboUIWrapperAnimationPlayers[i] as MonoBehaviour).gameObject.SetActive(true);
                comboUIWrapperAnimationPlayers[i].PlayActivateAnimation();
            }

            if (comboStartVfx != null)
            {
                comboStartVfx.Play();
            }
            if (comboStartSfx != null)
            {
                UISound.Play(comboStartSfx);
            }
        }

        public void Hide()
        {
            for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
            {
                if ((comboUIWrapperAnimationPlayers[i] as MonoBehaviour).isActiveAndEnabled)
                {
                    comboUIWrapperAnimationPlayers[i].PlayDeactivateAnimation();
                }
            }
            for (int i = 0; i < scoreUIWrapperAnimationPlayers.Length; i++)
            {
                if (scoreUIWrapperAnimationPlayers[i].isActiveAndEnabled)
                {
                    scoreUIWrapperAnimationPlayers[i].PlayHideAnimation();
                }
            }
        }

        public void UpdateScorePoints()
        {
            showingScorePoints = Score.CurrentScorePoint;
            scorePointText.text = showingScorePoints.ToString();
        }

        public void AnimateScorePoints()
        {
            if (scorePointAnimationCoroutine != null)
            {
                StopCoroutine(scorePointAnimationCoroutine);
                scorePointAnimationCoroutine = null;
            }
            scorePointAnimationCoroutine = StartCoroutine(ScorePointAnimationCoroutine());
        }

        Coroutine scorePointAnimationCoroutine;
        IEnumerator ScorePointAnimationCoroutine()
        {
            while (showingScorePoints != Score.CurrentScorePoint)
            {
                if (showingScorePoints < Score.CurrentScorePoint)
                {
                    // Score points should increase.
                    showingScorePoints = Mathf.Min(Score.CurrentScorePoint, showingScorePoints + Score.AnimationStep);
                    scorePointText.text = showingScorePoints.ToString();

                    // Animations and effects.
                    for (int i = 0; i < scorePointAnimations.Length; i++)
                    {
                        scorePointAnimations[i].PlayGainAnimation();
                    }
                    if (scoreGainVfx != null)
                    {
                        scoreGainVfx.Play();
                    }
                    if (scoreGainSfx != null)
                    {
                        UISound.Play(scoreGainSfx);
                    }

                    // End coroutine when showing point meets current point.
                    if (showingScorePoints == Score.CurrentScorePoint)
                    {
                        yield break;
                    }
                }
                else if (showingScorePoints > Score.CurrentScorePoint)
                {
                    // Score points should decrease.
                    showingScorePoints = Mathf.Max(Score.CurrentScorePoint, showingScorePoints - Score.AnimationStep);
                    scorePointText.text = showingScorePoints.ToString();

                    // Animations and effects.
                    for (int i = 0; i < scorePointAnimations.Length; i++)
                    {
                        scorePointAnimations[i].PlayLoseAnimation();
                    }
                    if (scoreLoseVfx != null)
                    {
                        scoreLoseVfx.Play();
                    }
                    if (scoreLoseSfx != null)
                    {
                        UISound.Play(scoreLoseSfx);
                    }

                    // End coroutine when showing point meets current point.
                    if (showingScorePoints == Score.CurrentScorePoint)
                    {
                        yield break;
                    }
                }

                // Wait for designated frames before next iteration.
                for (int i = 0; i < Score.AnimationInterval; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public void UpdateComboPoints()
        {
            comboPointText.text = Score.CurrentComboPoint.ToString();
        }

        public void AnimateComboPoints()
        {
            comboPointText.text = Score.CurrentComboPoint.ToString();

            for (int i = 0; i < comboPointAnimations.Length; i++)
            {
                comboPointAnimations[i].PlayGainAnimation();
            }
            if (comboGainVfx != null)
            {
                comboGainVfx.Play();
            }
            if (comboGainSfx != null)
            {
                UISound.Play(comboGainSfx);
            }
        }

        public void BreakCombo()
        {
            for (int i = 0; i < comboPointAnimations.Length; i++)
            {
                if (comboPointAnimations[i].isActiveAndEnabled)
                {
                    comboPointAnimations[i].PlayFailAnimation();
                }
            }
            if (comboFailVfx != null)
            {
                comboFailVfx.Play();
            }
            if (comboFailSfx != null)
            {
                UISound.Play(comboFailSfx);
            }
            for (int i = 0; i < comboUIWrapperAnimationPlayers.Length; i++)
            {
                if ((comboUIWrapperAnimationPlayers[i] as MonoBehaviour).isActiveAndEnabled)
                {
                    comboUIWrapperAnimationPlayers[i].PlayFailAnimationAndHide();
                }
            }
        }

        void OnDestroy()
        {
            if (Score.scoreObject == this)
            {
                Score.ResetEvents();
                Score.scoreObject = null;
            }
        }
    }
}