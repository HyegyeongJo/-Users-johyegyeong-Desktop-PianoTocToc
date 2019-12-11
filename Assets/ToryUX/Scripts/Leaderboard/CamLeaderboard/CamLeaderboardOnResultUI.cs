using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ToryUX
{
    [RequireComponent(typeof(PlayableDirector), typeof(ResultUI))]
    public class CamLeaderboardOnResultUI : CamLeaderboardUI
    {
        #region Singleton
        static volatile CamLeaderboardOnResultUI instance;
        static readonly object syncRoot = new object();

        public static CamLeaderboardOnResultUI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<CamLeaderboardOnResultUI>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public static CamLeaderboardOnResultUIController Controller
        {
            get;
            private set;
        }

        [SerializeField]
        private int minRankToTakePicture = 5;
        public static int MinRankToTakePicture
        {
            get
            {
                return Instance.minRankToTakePicture;
            }
        }

        public AudioClip goodJobSfx;
        public AudioClip cameraSfx;

        void Awake()
        {
            #region Create Singleton Instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.LogWarning("CamLeaderboardController component can only be one in a scene. Destroying duplicate.");
                Destroy(gameObject);
            }
            #endregion

            // Collect needed objects.
            Controller = GetComponentInChildren<CamLeaderboardOnResultUIController>(true);
            Controller.gameObject.SetActive(false);
        }

        void Start()
        {
            // Initialize Leaderboard.
            Init(minRankToTakePicture);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (instance == this)
            {
                instance = null;
            }
        }

        public static void ReadyCamIfNeeded(float delay)
        {
            if (Leaderboard.RecordType == LeaderboardRecordType.Score &&
                Leaderboard.GetTodayRank(Score.CurrentScorePoint) <= MinRankToTakePicture &&
                Score.CurrentScorePoint > 0)
            {
                if (Instance.readyCamCoroutine != null)
                {
                    Instance.StopCoroutine(Instance.readyCamCoroutine);
                    Instance.readyCamCoroutine = null;
                }

                Controller.shouldUsePhoto = true;
            }
            else if (Leaderboard.GetTodayRank(Timer.CurrentTime) <= MinRankToTakePicture && Timer.CurrentTime > 0)
            {
                if (Instance.readyCamCoroutine != null)
                {
                    Instance.StopCoroutine(Instance.readyCamCoroutine);
                    Instance.readyCamCoroutine = null;
                }

                Controller.shouldUsePhoto = true;
            }
            else
            {
                Controller.shouldUsePhoto = false;
            }
        }

        Coroutine readyCamCoroutine;
        IEnumerator ReadyCamCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            // Controller.webcam.StartCamera();

            yield return new WaitUntil(() => WebcamTextureManager.Instance.IsReady);
            Controller.webcam.ResizeToFit();

            readyCamCoroutine = null;
        }
    }

    // Being inside the same file with CamLeaderboardController, ResultUI can avoid dependency issues of doing below.
    [RequireComponent(typeof(PlayableDirector))]
    public partial class ResultUI : MonoBehaviour
    {
        partial void ShowCamLeaderboard()
        {
            if (CamLeaderboardOnResultUI.Instance == null)
            {
                return;
            }

            if (CamLeaderboardAvailable())
            {
                GetComponent<PlayableDirector>().Play(CamLeaderboardOnResultUI.Controller.timeline);
                // CamLeaderboardOnResultUI.ReadyCamIfNeeded(7.9f);
                CamLeaderboardOnResultUI.ReadyCamIfNeeded(5.9f);
            }
            else
            {
                GetComponent<PlayableDirector>().Play(CamLeaderboardOnResultUI.Controller.backupTimeline);
                StartCoroutine(CamLeaderboardControllerDisablerCoroutine());
            }
        }

        IEnumerator CamLeaderboardControllerDisablerCoroutine()
        {
            // This does not work as clean: the view shows up briefly before being disabled.
            yield return null;
            CamLeaderboardOnResultUI.Controller.gameObject.SetActive(false);
        }

        bool CamLeaderboardAvailable()
        {
            if (Leaderboard.RecordType == LeaderboardRecordType.Stopwatch)
            {
                return true;
            }

            switch (Leaderboard.RecordType)
            {
                case LeaderboardRecordType.Score:
                    if (Score.CurrentScorePoint > 0)
                    {
                        return true;
                    }
                    if (Leaderboard.EntriesToday.Count > 0 && Leaderboard.EntriesToday[0].score > 0)
                    {
                        return true;
                    }
                    break;
                case LeaderboardRecordType.Countdown:
                    if (Timer.CurrentTime > 0)
                    {
                        return true;
                    }
                    if (Leaderboard.EntriesToday.Count > 0 && Leaderboard.EntriesToday[0].timeRecord > 0)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}