using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
	[ExecuteInEditMode]
	public class ToryButton : Button
	{
		public Text text;

		private RectTransform rectTransform;

		protected override void Awake()
		{
			base.Awake();
			FetchTextObjects();
			rectTransform = GetComponent<RectTransform>();
			onClick.AddListener(PlaySFX);
		}

		void FetchTextObjects()
		{
			try
			{
				text = transform.Find("Text").GetComponent<Text>();;
			}
			catch
			{
				Debug.LogErrorFormat("ToryButton {0} cannot find Text object named \"Text\" among its children.", name);
			}
			if (text == null)
			{
				Debug.LogErrorFormat("ToryButton {0} cannot find Text object named \"Text\" among its children.", name);
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData.delta.magnitude > 0)
			{
				base.OnPointerEnter(eventData);
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (SettingsUI.Instance.isActiveAndEnabled)
			{
				if (SettingsUI.Instance.selectionSound != null && interactable)
				{
					UISound.Play(SettingsUI.Instance.selectionSound);
				}
				SettingsUI.ScrollTo(rectTransform);
			}
		}

		public void PlaySFX()
		{
			if (interactable && SettingsUI.Instance.buttonSound != null && interactable)
			{
				UISound.Play(SettingsUI.Instance.buttonSound);
			}
		}
	}
}