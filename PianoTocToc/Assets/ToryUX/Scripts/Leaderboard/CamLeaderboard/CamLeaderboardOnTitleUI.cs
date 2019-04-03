using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ToryUX
{
    [RequireComponent(typeof(TitleUI))]
    public class CamLeaderboardOnTitleUI : CamLeaderboardUI
    {
        public static CamLeaderboardOnTitleUI Instance
        {
            get
            {
                return instance;
            }
        }
        private static CamLeaderboardOnTitleUI instance;

        public static CamLeaderboardOnTitleUIController Controller
        {
            get;
            private set;
        }

        public Color todaySolidColor = Color.white;
        public Color todayGradientColor = Color.white;
        public Color todayTextColor = Color.white;
        public Color weeklySolidColor = Color.white;
        public Color weeklyGradientColor = Color.white;
        public Color weeklyTextColor = Color.white;

        public float recordBarMinWidth = 370f;

        void Awake()
        {
            // Behave as a singleton for the sake of convenient static thingies.
            if (instance != null && instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("CamLeaderboardController component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                CamLeaderboardOnTitleUI.instance = this;
            }

            // Collect needed objects.
            Controller = GetComponentInChildren<CamLeaderboardOnTitleUIController>(true);
            Controller.gameObject.SetActive(false);
        }

        void Start()
        {
            // Initialize Leaderboard.
            Init();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (instance == this)
            {
                instance = null;
            }
        }

        public static void StartLeaderboardCoroutine()
        {
            if (Instance.leaderboardCoroutine != null)
            {
                Instance.StopCoroutine(Instance.leaderboardCoroutine);
                Instance.leaderboardCoroutine = null;
            }
            Instance.leaderboardCoroutine = Instance.StartCoroutine(Instance.LeaderboardCoroutine());
        }

        public static void StopLeaderboardCoroutine()
        {
            if (Instance.leaderboardCoroutine != null)
            {
                Instance.StopCoroutine(Instance.leaderboardCoroutine);
                Instance.leaderboardCoroutine = null;
            }
        }

        Coroutine leaderboardCoroutine;
        IEnumerator LeaderboardCoroutine()
        {
            yield return new WaitForSeconds(10f);
            Controller.gameObject.SetActive(true);
            Controller.Show(() =>
                {
                    Controller.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
                    StartLeaderboardCoroutine();
                },
                () =>
                {
                    Controller.gameObject.SetActive(false);
                }
            );

            leaderboardCoroutine = null;
        }
    }

    // Being inside the same file with CamLeaderboardController, TitleUI can avoid dependency issues of doing below.
    public partial class TitleUI : MonoBehaviour
    {
        partial void StartLeaderboardCoroutine()
        {
            if (CamLeaderboardOnTitleUI.Instance != null)
            {
                CamLeaderboardOnTitleUI.StartLeaderboardCoroutine();
            }
        }

        partial void StopLeaderboardCoroutine()
        {
            if (CamLeaderboardOnTitleUI.Instance != null)
            {
                CamLeaderboardOnTitleUI.StopLeaderboardCoroutine();
            }
        }
    }
}