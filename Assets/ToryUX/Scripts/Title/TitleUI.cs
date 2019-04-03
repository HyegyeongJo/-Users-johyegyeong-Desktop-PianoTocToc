using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public partial class TitleUI : MonoBehaviour
    {
        public static TitleUI Instance
        {
            get
            {
                return instance;
            }
        }
        private static TitleUI instance;

        public bool shouldBeHiddenOnStart = false;

        public AudioClip gameStartSfx;

		public bool toggleBlurBackground = true;
		public bool toggleVignettes = true;
		public bool toggleBloomFlash = true;


        void Awake()
        {
            Application.targetFrameRate = 60;

            // Behave as a singleton for the sake of convenient static thingies.
            if (instance != null && instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("TitleUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                TitleUI.instance = this;
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

        IEnumerator DelayedShowCoroutine()
        {
            yield return new WaitForEndOfFrame();
            Show();
        }

        /// <summary>
        /// Shows title UI.
        /// </summary>
        public static void Show()
        {
            Instance.gameObject.SetActive(true);
			if (Instance.toggleBlurBackground)
			{
				CameraEffects.ShowTranslucentLayer();
			}
			if (Instance.toggleVignettes)
			{
            	CameraEffects.ShowVignetteEffect();
			}
            CameraEffects.FadeIn();

            Instance.StartLeaderboardCoroutine();
        }

        /// <summary>
        /// Hide title UI.
        /// It is considered to start the game when this method is called; hence playing game start sfx and such.
        /// </summary>
        public static void Hide()
        {
            if (Instance.toggleBlurBackground)
            {
                CameraEffects.HideTranslucentLayer();
            }
            if (Instance.toggleVignettes)
            {
                CameraEffects.HideVignetteEffect();
            }
            if (Instance.toggleBloomFlash)
            {
                CameraEffects.FlashBloom();
            }
            if (Instance.gameStartSfx != null)
            {
                UISound.Play(Instance.gameStartSfx);
            }

            if (Instance.GetComponent<ShowAndHideAnimationPlayer>() == null)
            {
                Instance.gameObject.SetActive(false);
            }
            else
            {
                Instance.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
            }

            Instance.StopLeaderboardCoroutine();
        }

        partial void StartLeaderboardCoroutine();
        partial void StopLeaderboardCoroutine();


        void OnDestroy()
        {
            if (TitleUI.Instance == this)
            {
                TitleUI.instance = null;
            }
        }
    }
}