using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public class TimerEvent : MonoBehaviour
    {
        public static TimerEvent Instance
        {
            get
            {
                return instance;
            }
        }
        private static TimerEvent instance;

        public static event CountdownFinishAction OnCountdownFinish;
        public delegate void CountdownFinishAction();

        public static event CountdownHitTimeAction OnCountdownHitTime;
        public delegate void CountdownHitTimeAction(float time);

        public static float[] CountdownEventPoints
        {
            get
            {
                return countdownEventPoints;
            }
            set
            {
                countdownEventPoints = value;

                // If the change is made while the timer is running, restart monitoring with updated values.
                if (Timer.IsRunning && Timer.Mode == Timer.TimerMode.Countdown)
                {
                    Instance.StartMonitoring();
                }
            }
        }
        private static float[] countdownEventPoints;

        public static event StopwatchHitTimeAction OnStopwatchHitTime;
        public delegate void StopwatchHitTimeAction(float time);

        public static float[] StopwatchEventPoints
        {
            get
            {
                return stopwatchEventPoints;
            }
            set
            {
                stopwatchEventPoints = value;

                // If the change is made while the timer is running, restart monitoring with updated values.
                if (Timer.IsRunning && Timer.Mode == Timer.TimerMode.Stopwatch)
                {
                    Instance.StartMonitoring();
                }
            }
        }
        private static float[] stopwatchEventPoints;

        /// <summary>
        /// Make <c>OnCountdownFinish</c>, <c>OnCountdownHitTime</c> and <c>OnStopwatchHitTime</c> to null.
        /// </summary>
        public static void ResetEvents()
        {
            OnCountdownFinish = null;
            OnCountdownHitTime = null;
            OnStopwatchHitTime = null;
        }

        void Awake()
        {
            // Register this object to ToryUX.Timer
            if (instance != null && instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("TimerEvent component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                TimerEvent.instance = this;
            }
        }

        public void StartMonitoring()
        {
            if (timerEventCoroutine != null)
            {
                StopCoroutine(timerEventCoroutine);
                timerEventCoroutine = null;
            }
            timerEventCoroutine = StartCoroutine(TimerEventCoroutine());
        }

        Coroutine timerEventCoroutine;
        IEnumerator TimerEventCoroutine()
        {
            // Ready some variables needed.
            float time;
            float nextEventPoint;
            bool eventPointUpdated;
            switch (Timer.Mode)
            {
                case Timer.TimerMode.Countdown:
                    nextEventPoint = float.MinValue;
                    break;
                case Timer.TimerMode.Stopwatch:
                    nextEventPoint = float.MaxValue;
                    break;
                default:
                    // This cannot happen; just to avoid not-initialized variable error.
                    nextEventPoint = float.NaN;
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogError("TimerEvent.StartMonitoring() failed because Timer.Mode was set invalid.");
                    #endif
                    yield break;
            }

            while (true)
            {
                time = Timer.CurrentTime;
                switch (Timer.Mode)
                {
                    case Timer.TimerMode.Countdown:
                        if (CountdownEventPoints != null &&
                            CountdownEventPoints.Length > 0 &&
                            OnCountdownHitTime != null)
                        {
                            // Check for nearest event point.
                            eventPointUpdated = false;
                            for (int i = 0; i < CountdownEventPoints.Length; i++)
                            {
                                if (CountdownEventPoints[i] <= time && CountdownEventPoints[i] > nextEventPoint)
                                {
                                    nextEventPoint = CountdownEventPoints[i];
                                    eventPointUpdated = true;
                                }
                            }

                            if (eventPointUpdated)
                            {
                                // Wait for event point and fire an event.
                                yield return new WaitForSeconds(time - nextEventPoint);
                                OnCountdownHitTime(nextEventPoint);
                                yield return new WaitForEndOfFrame();
                            }
                            else
                            {
                                // If there is no event point remaning, wait for countdown to finish and fire the event.
                                if (OnCountdownFinish != null)
                                {
                                    yield return new WaitForSeconds(time);
                                    OnCountdownFinish();
                                }
                                yield break;
                            }
                        }
                        else if (OnCountdownFinish != null)
                        {
                            // If there is no event point set, wait for countdown to finish and fire the event.
                            yield return new WaitForSeconds(time);
                            OnCountdownFinish();
                            yield break;
                        }
                        else
                        {
                            // No need to wait for anything.
                            yield break;
                        }
                        break;

                    case Timer.TimerMode.Stopwatch:
                        if (StopwatchEventPoints != null &&
                            StopwatchEventPoints.Length > 0 &&
                            OnStopwatchHitTime != null)
                        {
                            // Check for nearest event point.
                            eventPointUpdated = false;
                            for (int i = 0; i < StopwatchEventPoints.Length; i++)
                            {
                                if (StopwatchEventPoints[i] >= time && StopwatchEventPoints[i] < nextEventPoint)
                                {
                                    nextEventPoint = StopwatchEventPoints[i];
                                    eventPointUpdated = true;
                                }
                            }

                            if (eventPointUpdated)
                            {
                                // Wait for event point and fire an event.
                                yield return new WaitForSeconds(nextEventPoint - time);
                                OnStopwatchHitTime(nextEventPoint);
                                yield return new WaitForEndOfFrame();
                            }
                            else
                            {
                                // No more event point to wait.
                                yield break;
                            }
                        }
                        else
                        {
                            // No event will ever fire in this case.
                            yield break;
                        }
                        break;
                }
            }
        }

        void OnEnable()
        {
            if (Timer.IsRunning)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("TimerEvent component enabled and found the Timer is running now; picking up for the event.");
                #endif

                if (timerEventCoroutine != null)
                {
                    StopCoroutine(timerEventCoroutine);
                    timerEventCoroutine = null;
                }
                timerEventCoroutine = StartCoroutine(TimerEventCoroutine());
            }
        }

        void OnDisable()
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!this.enabled)
            {
                Debug.LogWarning("TimerEvent component disabled; timer events will stop working.");
            }
            #endif
            
            if (timerEventCoroutine != null)
            {
                StopCoroutine(timerEventCoroutine);
                timerEventCoroutine = null;
            }
        }

        void OnDestroy()
        {
            if (TimerEvent.Instance == this)
            {
                ResetEvents();
                TimerEvent.instance = null;
            }
        }
    }
}