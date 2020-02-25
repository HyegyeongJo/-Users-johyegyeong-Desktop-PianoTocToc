using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class TimerUI : MonoBehaviour
    {
        public bool shouldBeHiddenOnStart = true;

        public Timer.TimerMode mode;
        public float countdownStartTime;
        public bool showMilliseconds;
        [Range(0f, 1f)]
        public float millisecondsTextScale = .75f;

        public bool containingLapCountUI;
        public Color lapCountColor;
        public Color lapCountPostfixColor;
        [Range(0f, 1f)]
        public float lapCountPostfixScale = .75f;

        public AudioClip timerTickSfx;
        public AudioClip timerTockSfx;
        public AudioClip timerAlertSfx;

        public ParticleSystem timerTickVfx;
        public ParticleSystem timerTockVfx;
        public ParticleSystem timerAlertVfx;

        public Text timerText;
        public Text lapText;

        private float showingTime;

        private TimerUIWrapperAnimationPlayer[] timerUIWrapperAnimationPlayers;
        #pragma warning disable 0414
        private TimerAnimationPlayer[] timerAnimations;

        public bool IsShown
        {
            get
            {
                if (timerUIWrapperAnimationPlayers.Length <= 0)
                {
                    return false;
                }
                else
                {
                    return timerUIWrapperAnimationPlayers[0].gameObject.activeInHierarchy;
                }
            }
        }

        void Awake()
        {
            // Register this object to ToryUX.Timer
            if (Timer.timerObject != null && Timer.timerObject != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("TimerUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                Timer.timerObject = this;
            }

            // Collect animation players of children.
            timerUIWrapperAnimationPlayers = GetComponentsInChildren<TimerUIWrapperAnimationPlayer>(true);
            timerAnimations = GetComponentsInChildren<TimerAnimationPlayer>(true);

            timerText = GetComponentInChildren<TagClasses.TimerUITimeTextObject>(true).GetComponent<Text>();

            containingLapCountUI = (GetComponentInChildren<TagClasses.TimerUILapTextObject>(true) != null);
            if (containingLapCountUI)
            {
                lapText = GetComponentInChildren<TagClasses.TimerUILapTextObject>(true).GetComponent<Text>();
            }

            // Initialize timer.
            Timer.Mode = mode;
            Timer.CountdownStartTime = countdownStartTime;

            // Hide if needed.
            if (shouldBeHiddenOnStart)
            {
                for (int i = 0; i < timerUIWrapperAnimationPlayers.Length; i++)
                {
                    timerUIWrapperAnimationPlayers[i].gameObject.SetActive(false);
                }
            }
        }

        public void UpdateTimer()
        {
            showingTime = Timer.CurrentTime;
            timerText.text = SecondsToTimespanString(showingTime, showMilliseconds, timerText.fontSize * millisecondsTextScale);
        }

        public void Show()
        {
            UpdateTimer();
            UpdateLapCount();

            for (int i = 0; i < timerUIWrapperAnimationPlayers.Length; i++)
            {
                timerUIWrapperAnimationPlayers[i].gameObject.SetActive(true);
                timerUIWrapperAnimationPlayers[i].PlayShowAnimation();
            }
            runTimerCoroutine = StartCoroutine(RunTimerCoroutine());
        }

        public void Hide()
        {
            for (int i = 0; i < timerUIWrapperAnimationPlayers.Length; i++)
            {
                if (timerUIWrapperAnimationPlayers[i].isActiveAndEnabled)
                {
                    timerUIWrapperAnimationPlayers[i].PlayHideAnimation();
                }
            }
            // StopCoroutine("DisplayTimerCoroutine");
        }

        public void RunTimer()
        {
            if (runTimerCoroutine != null)
            {
                StopCoroutine(runTimerCoroutine);
                runTimerCoroutine = null;
            }
            runTimerCoroutine = StartCoroutine(RunTimerCoroutine());
        }

        Coroutine runTimerCoroutine;
        IEnumerator RunTimerCoroutine()
        {
            while (true)
            {
                UpdateTimer();

                if (!IsShown || !Timer.IsRunning || Timer.IsPaused)
                {
                    runTimerCoroutine = null;
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void UpdateLapCount()
        {
            lapText.text = string.Format("Lap <color=#{3}>{0}</color><size={2}> / <color=#{4}>{1}</color></size>", Timer.LapCount, Timer.MaxLapCount, lapText.fontSize * lapCountPostfixScale, ColorUtility.ToHtmlStringRGBA(lapCountColor), ColorUtility.ToHtmlStringRGBA(lapCountPostfixColor));
        }

        void OnDestroy()
        {
            if (Timer.timerObject == this)
            {
                TimerEvent.ResetEvents();
                Timer.timerObject = null;
            }
        }

        public static string SecondsToTimespanString(float seconds, bool includeMilliseconds, float millisecondSize = -1f)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string s;
            if (t.Minutes < 10)
            {
                s = string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
            }
            else
            {
                s = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }

            if (includeMilliseconds && millisecondSize > 0)
            {
                s += string.Format("<size={1}>.{0:D2}</size>", (int) (t.Milliseconds * 0.1f), millisecondSize);
            }
            return s;
        }
    }
}