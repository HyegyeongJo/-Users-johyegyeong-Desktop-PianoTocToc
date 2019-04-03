using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToryUX
{
    public class CountThree : MonoBehaviour
    {
        public AudioClip countOneSfx;
        public AudioClip countTwoSfx;
        public AudioClip countThreeSfx;

        [Space]
        public GameObject animationWrapper;

        public static bool IsActive
        {
            get
            {
                return Instance.animationWrapper.activeInHierarchy;
            }
        }

        public static event CountFinishAction OnCountFinish;
        public delegate void CountFinishAction();

        public static CountThree Instance
        {
            get
            {
                return instance;
            }
        }
        private static CountThree instance;

        public static void Start()
        {
            Instance.animationWrapper.SetActive(true);
        }

        public static void Stop()
        {
            Instance.animationWrapper.SetActive(false);
        }

        public static void Finish()
        {
            if (OnCountFinish != null)
            {
                OnCountFinish();
            }
            Stop();
        }

        void Awake()
        {
            // Behave as a singleton for the sake of convenient static thingies.
            if (instance != null && instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("CountThree component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            animationWrapper.SetActive(false);
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                OnCountFinish = null;
                instance = null;
            }
        }
    }
}