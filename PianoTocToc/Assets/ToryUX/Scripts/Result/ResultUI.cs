using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public partial class ResultUI : MonoBehaviour
    {
        public static ResultUI Instance
        {
            get
            {
                return instance;
            }
        }
        private static ResultUI instance;

        /// <summary>
        /// Occurs when the countdown animation starts or finishes.
        /// </summary>
        public static event Action OnCountDownStart;
        public static event Action OnCountDownFinish;
        public static bool countDownStarted;

        /// <summary>
        /// Occurs when the result screen times out and completely faded out.
        /// Good place to register game restart code.
        /// </summary>
        public static event Action OnFadedOut;

        public bool shouldBeHiddenOnStart = true;

        public GameObject heroObjectOnSuccess;
        public GameObject heroObjectOnFail;

        public AudioClip sfxOnSuccess;
        public AudioClip sfxOnFail;

        public ParticleSystem vfxOnSuccess;
        public ParticleSystem vfxOnFail;

        public AudioClip countingResultScoreSfx;
        public float maxDurationCountingResultScore = 1f;

        public GameObject upperWheelObject;
        public GameObject bestScoreCelebrationObject;
        public AudioClip bestScoreSfx;
        public ParticleSystem bestScoreVfx;
        public Text bestScorePointText;

        public AudioClip countDownSfx;

        public bool toggleBlurBackground = true;
        public bool toggleVignettes = true;
        public bool toggleBloomFlash = true;

        void Awake()
        {
            // Behave as a singleton for the sake of convenient static thingies.
            if (instance != null && instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("ResultUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                ResultUI.instance = this;
            }

            OnCountDownStart += () =>
            {
                countDownStarted = true;
            };

            // Collect needed game objects.
            upperWheelObject = GetComponentInChildren<TagClasses.ResultUIUpperWheelObject>(true).gameObject;
            bestScoreCelebrationObject = GetComponentInChildren<TagClasses.ResultUIBestScoreCelebrationObject>(true).gameObject;
            bestScorePointText = GetComponentInChildren<TagClasses.ResultUIBestScoreTextObject>(true).GetComponent<Text>();
            heroObjectOnSuccess = GetComponentInChildren<TagClasses.ResultUIHeroImageOnSuccessObject>(true).gameObject;
            heroObjectOnFail = GetComponentInChildren<TagClasses.ResultUIHeroImageOnFailObject>(true).gameObject;
            if (GetComponentInChildren<TagClasses.ResultUIFailVfxObject>(true) != null)
            {
                vfxOnFail = GetComponentInChildren<TagClasses.ResultUIFailVfxObject>(true).gameObject.GetComponent<ParticleSystem>();
            }
            if (GetComponentInChildren<TagClasses.ResultUISuccessVfxObject>(true) != null)
            {
                vfxOnSuccess = GetComponentInChildren<TagClasses.ResultUISuccessVfxObject>(true).gameObject.GetComponent<ParticleSystem>();
            }

            if (shouldBeHiddenOnStart)
            {
                gameObject.SetActive(false);
            }
            else
            {
                // Wait a frame till everything get settles before show up the UI.
                StartCoroutine(DelayedShowCoroutine());
            }
        }

        void OnEnable()
        {
            countDownStarted = false;
        }

        IEnumerator DelayedShowCoroutine()
        {
            yield return new WaitForEndOfFrame();
            Show();
        }

        /// <summary>
        /// Shows result UI.
        /// </summary>
        public static void Show(bool isFail = false)
        {
            Instance.gameObject.SetActive(true);
            Instance.ShowCamLeaderboard();
            CameraEffects.CurtainImage.gameObject.SetActive(false);
            Instance.heroObjectOnFail.SetActive(isFail);
            Instance.heroObjectOnSuccess.SetActive(!isFail);
            if (!isFail && Instance.sfxOnSuccess != null)
            {
                UISound.Play(Instance.sfxOnSuccess);
            }
            else if (isFail && Instance.sfxOnFail != null)
            {
                UISound.Play(Instance.sfxOnFail);
            }
            if (!isFail && Instance.vfxOnSuccess != null)
            {
                Instance.vfxOnSuccess.Play();
            }
            else if (isFail && Instance.vfxOnFail != null)
            {
                Instance.vfxOnFail.Play();
            }
            if (Instance.toggleBloomFlash)
            {
                CameraEffects.FlashBloom();
            }
            if (Instance.toggleVignettes)
            {
                CameraEffects.ShowVignetteEffect();
            }
            if (Instance.toggleBlurBackground)
            {
                CameraEffects.ShowTranslucentLayer();
            }

            // Check if achieved record score.
            if (CamLeaderboardOnResultUI.Instance == null)
            {
                if (Score.HighScore <= Score.CurrentScorePoint)
                {
                    Score.ResetHighScore(Score.CurrentScorePoint);
                    Instance.bestScoreCelebrationObject.SetActive(true);
                }
                else
                {
                    Instance.bestScoreCelebrationObject.SetActive(false);
                }
                Instance.bestScorePointText.text = Score.HighScore.ToString();
            }
            else
            {
                // Check if achieved record is better than high record of recent 7 days.
                switch (Leaderboard.RecordType)
                {
                    case LeaderboardRecordType.Score:
                        if (Leaderboard.EntriesLastSevenDays.Count < 1 || Leaderboard.EntriesLastSevenDays[0].score <= Score.CurrentScorePoint)
                        {
                            Score.ResetHighScore(Score.CurrentScorePoint);
                            Instance.bestScoreCelebrationObject.SetActive(true);
                        }
                        else
                        {
                            Instance.bestScoreCelebrationObject.SetActive(false);
                        }
                        Instance.bestScorePointText.text = Score.HighScore.ToString();
                        break;

                    case LeaderboardRecordType.Stopwatch:
                        if (Leaderboard.EntriesLastSevenDays.Count < 1 || Leaderboard.EntriesLastSevenDays[0].timeRecord >= Timer.CurrentTime)
                        {
                            Timer.ResetHighRecordTime(Timer.CurrentTime);
                            Instance.bestScoreCelebrationObject.SetActive(true);
                        }
                        else
                        {
                            Instance.bestScoreCelebrationObject.SetActive(false);
                        }

                        if (Timer.HighRecordTime > 0)
                        {
                            Instance.bestScorePointText.text = TimerUI.SecondsToTimespanString(Timer.HighRecordTime, false);
                        }
                        else
                        {
                            Instance.bestScorePointText.text = "-";
                        }
                        break;

                    case LeaderboardRecordType.Countdown:
                        if (Leaderboard.EntriesLastSevenDays.Count < 1 || Leaderboard.EntriesLastSevenDays[0].timeRecord <= Timer.CurrentTime)
                        {
                            Timer.ResetHighRecordTime(Timer.CurrentTime);
                            Instance.bestScoreCelebrationObject.SetActive(true);
                        }
                        else
                        {
                            Instance.bestScoreCelebrationObject.SetActive(false);
                        }

                        if (Timer.HighRecordTime > 0)
                        {
                            Instance.bestScorePointText.text = TimerUI.SecondsToTimespanString(Timer.HighRecordTime, false);
                        }
                        else
                        {
                            Instance.bestScorePointText.text = "-";
                        }
                        break;
                }
            }
        }

        partial void ShowCamLeaderboard();

        /// <summary>
        /// Component "CountDownAnimationHandler" will call this method on count down start.
        /// </summary>
        public static void TriggerCountDownStartEvent()
        {
            if (ResultUI.OnCountDownStart != null)
            {
                ResultUI.OnCountDownStart();
            }
        }

        /// <summary>
        /// Component "CountDownAnimationHandler" will call this method on count down finish.
        /// </summary>
        public static void TriggerCountDownFinishEvent()
        {
            if (ResultUI.OnCountDownFinish != null)
            {
                ResultUI.OnCountDownFinish();
            }
        }

        /// <summary>
        /// Fade out by covering the screen up with black image.
        /// Upon fade out completes, <c>OnFadedOut</c> event will fire.
        /// </summary>
        public static void FadeOut()
        {
            if (Instance.fadeOutCoroutine != null)
            {
                Instance.StopCoroutine(Instance.fadeOutCoroutine);
                Instance.fadeOutCoroutine = null;
            }
            Instance.fadeOutCoroutine = Instance.StartCoroutine(Instance.FadeOutCoroutine(1f));
        }

        Coroutine fadeOutCoroutine;
        IEnumerator FadeOutCoroutine(float duration)
        {
            if (Instance.toggleVignettes)
            {
                CameraEffects.HideVignetteEffect(duration);
            }

            CameraEffects.FadeOut(duration, false);
            yield return new WaitForSeconds(duration);

            if (ResultUI.OnFadedOut != null)
            {
                ResultUI.OnFadedOut();
            }
            if (Instance.toggleBlurBackground)
            {
                CameraEffects.HideTranslucentLayer();
            }

            Instance.gameObject.SetActive(false);
            fadeOutCoroutine = null;
        }

        void OnDestroy()
        {
            if (ResultUI.Instance == this)
            {
                ResultUI.instance = null;
                OnCountDownStart = null;
                OnCountDownFinish = null;
                OnFadedOut = null;
            }
        }
    }
}