using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    /// <summary>
    /// A static class to handle game's timer.
    /// It will work together with one <c>TimerUI</c> component in the scene.
    /// </summary>
    public static class Timer
    {
        public static TimerUI timerObject;

        public enum TimerMode
        {
            Countdown,
            Stopwatch
        }

        public static TimerMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                if (IsRunning)
                {
                    Stop();

                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning("Timer.Mode has changed while the timer is running; current timer will stop.");
                    #endif
                }
            }
        }

        [SerializeField]
        private static TimerMode mode;

        private static float referenceTime;
        private static float pausedTime;

        public static float CountdownStartTime
        {
            get
            {
                return countdownStartTime;
            }
            set
            {
                countdownStartTime = value;
            }
        }
        private static float countdownStartTime;

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
        /// Timer's last saved time.
        /// Reset when timer <c>Start()</c> after <c>Stop()</c> or scene reloaded.
        /// </summary>
        /// <returns></returns>
        public static float CurrentTime
        {
            get
            {
                if (IsRunning)
                {
                    return GetTime();
                }
                return currentTime;
            }
            set
            {
                currentTime = value;
            }
        }
        private static float currentTime;

        private static float GetTime()
        {
            if (!IsRunning)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("Timer is not running but GetTime() was called; Return timer start time.");
                #endif
                switch (Mode)
                {
                    case TimerMode.Countdown:
                        return CountdownStartTime;
                    case TimerMode.Stopwatch:
                        return 0f;
                }
            }

            switch (Mode)
            {
                case TimerMode.Countdown:
                    if (IsPaused)
                    {
                        return Mathf.Max(0, CountdownStartTime - pausedTime + referenceTime);
                    }
                    else
                    {
                        return Mathf.Max(0, CountdownStartTime - Time.timeSinceLevelLoad + referenceTime);
                    }
                case TimerMode.Stopwatch:
                    if (IsPaused)
                    {
                        return pausedTime - referenceTime;
                    }
                    else
                    {
                        return Time.timeSinceLevelLoad - referenceTime;
                    }
            }

            // Below cannot happen.
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError("Timer.GetTime() failed because Timer.Mode was set invalid.");
            #endif
            return float.NaN;
        }

        /// <summary>
        /// Record score. Can only be read.
        /// <c>ResultUI</c> component will automatically update this value.
        /// Call <c>ResetHighRecordTime(float)</c> to manually set the value.
        /// </summary>
        public static float HighRecordTime
        {
            get
            {
                return PlayerPrefs.GetFloat("HighRecordTime", -1f);
            }
        }

        /// <summary>
        /// Resets high record time.
        /// Pass integer value to manually modify the record.
        /// This method will automatically called by <c>ResultUI</c> component.
        /// </summary>
        public static void ResetHighRecordTime(float highTimeRecord = 0f)
        {
            PlayerPrefs.SetFloat("HighRecordTime", highTimeRecord);
        }

        /// <summary>
        /// Adds or subtracts time manually.
        /// Returns <value>false</value> when timer is not running.
        /// </summary>
        /// <param name="time">Time to add or subtract in seconds.</param>
        public static bool Add(float time)
        {
            if (!IsRunning)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("Timer is not running but Add() was called; Nothing happens.");
                #endif
                return false;
            }

            referenceTime += time;
            return true;
        }

        public static int MaxLapCount
        {
            get
            {
                return maxLapCount;
            }
            set
            {
                maxLapCount = (value > 0) ? value : 1;
            }
        }
        private static int maxLapCount = 1;

        public static int LapCount
        {
            get
            {
                return lapCount;
            }
            set
            {
                lapCount = (value > 0) ? value : 1;

                if (timerObject != null && IsUIShown)
                {
                    timerObject.UpdateLapCount();
                }
            }
        }
        private static int lapCount = 1;

        public static bool IsRunning
        {
            get
            {
                return isRunning;
            }
            private set
            {
                isRunning = value;
            }
        }
        private static bool isRunning = false;

        public static bool IsPaused
        {
            get
            {
                return isPaused;
            }
            private set
            {
                isPaused = value;
            }
        }
        private static bool isPaused;

        public static void Start()
        {
            referenceTime = Time.timeSinceLevelLoad;
            IsRunning = true;
            IsPaused = false;

            if (timerObject != null && IsUIShown)
            {
                timerObject.RunTimer();
            }
        }

        public static void Stop()
        {
            CurrentTime = GetTime();
            IsRunning = false;
            IsPaused = false;
        }

        public static void Pause()
        {
            if (IsRunning && !IsPaused)
            {
                pausedTime = Time.timeSinceLevelLoad;
                IsPaused = true;
            }
        }

        public static void Resume()
        {
            if (IsRunning && IsPaused)
            {
                referenceTime += Time.timeSinceLevelLoad - pausedTime;
                IsPaused = false;

                if (timerObject != null && IsUIShown)
                {
                    timerObject.RunTimer();
                }
            }
        }

        public static void Reset()
        {
            currentTime = 0f;
            IsRunning = false;
            IsPaused = false;
        }

        /// <summary>
        /// Makes timer UI appear.
        /// </summary>
        public static void ShowUI()
        {
            if (timerObject != null && !IsUIShown)
            {
                timerObject.gameObject.SetActive(true);
                timerObject.Show();
            }
        }

        /// <summary>
        /// Makes timer UI disappear.
        /// </summary>
        public static void HideUI()
        {
            if (timerObject != null && IsUIShown)
            {
                timerObject.Hide();
            }
        }

        /// <summary>
        /// Returns true if timer UI game object is active.
        /// </summary>
        public static bool IsUIShown
        {
            get
            {
                if (timerObject != null)
                {
                    return timerObject.IsShown;
                }
                else
                {
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning("No timer UI is registered to Timer static class!");
                    #endif
                    return false;
                }
            }
        }
    }
}