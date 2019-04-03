using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class ProgressUI : MonoBehaviour
    {
        public bool shouldBeHiddenOnStart = true;

		private Image fillMaskImage;
		private RectTransform[] indicators;

		private ProgressUIWrapperAnimationPlayer progressUIWrapperAnimationPlayer;

		private float showingProgression;

		public bool IsShown
		{
			get
			{
				return progressUIWrapperAnimationPlayer.gameObject.activeInHierarchy;
			}
		}

		public static float BarHeight
		{
			get;
			private set;
		}

        void Awake()
        {
            // Register this object to ToryUX.Progress
            if (Progress.progressObject != null && Progress.progressObject != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("ProgressUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                Progress.progressObject = this;
            }

			// Get indicators.
			var indicatorObjects = GetComponentsInChildren<TagClasses.ProgressUIIndicatorObject>();
			indicators = new RectTransform[indicatorObjects.Length];
			for (int i = 0; i < indicatorObjects.Length; i++)
			{
				indicators[i] = indicatorObjects[i].GetComponent<RectTransform>();
			}

			fillMaskImage = GetComponentInChildren<TagClasses.ProgressUIFillMaskObject>().GetComponent<Image>();

			// Collect animation players of children.
			progressUIWrapperAnimationPlayer = GetComponentInChildren<ProgressUIWrapperAnimationPlayer>();

			// Check UI fill bar height.
			ProgressUI.BarHeight = fillMaskImage.rectTransform.rect.height;

			// Initialize score.
			Progress.Reset();
			showingProgression = 0;
			SetProgress(true);

			// Hide if needed.
			if (shouldBeHiddenOnStart)
			{
				progressUIWrapperAnimationPlayer.gameObject.SetActive(false);
			}
		}

		void Update()
		{
			if (IsShown)
			{
				SetProgress();
			}
		}

		public void Show()
		{
			SetProgress(true);
			progressUIWrapperAnimationPlayer.PlayShowAnimation();
			progressUIWrapperAnimationPlayer.gameObject.SetActive(true);
		}

		public void Hide()
		{
			progressUIWrapperAnimationPlayer.PlayHideAnimation();
		}

		public void SetProgress(bool instantly = false)
		{
			showingProgression = Mathf.Clamp01(showingProgression + (Progress.CurrentProgression - showingProgression) * Progress.UIFillSpeed);

			fillMaskImage.fillAmount = showingProgression;
			for (int i = 0; i < indicators.Length; i++)
			{
				indicators[i].anchoredPosition = Vector2.up * Mathf.Lerp(-ProgressUI.BarHeight * 0.5f, ProgressUI.BarHeight * 0.5f, showingProgression);
			}
		}


        void OnDestroy()
        {
            if (Progress.progressObject == this)
            {
                Progress.ResetEvents();
                Progress.progressObject = null;
            }
        }
    }
}