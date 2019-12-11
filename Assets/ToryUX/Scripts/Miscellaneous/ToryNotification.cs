using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	public class ToryNotification : MonoBehaviour
	{
		#region Singleton

		static volatile ToryNotification instance;
		static readonly object syncRoot = new object();

		public static ToryNotification Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = FindObjectOfType<ToryNotification>();
						}
					}
				}
				return instance;
			}
		}

		#endregion

		[SerializeField] GameObject toryNotificationUILayerPrefab;
		GameObject toryNotificationUILayer;

		Image background;
		Text message;

		float hideTime;

		void InstanciateNotificationLayer(bool activate = true)
		{
			toryNotificationUILayer = GameObject.Instantiate(toryNotificationUILayerPrefab, GetComponentInChildren<UIOrientationSetter>().transform);
			toryNotificationUILayer.transform.SetAsLastSibling();

			background = toryNotificationUILayer.GetComponent<Image>();
			message = toryNotificationUILayer.GetComponentInChildren<Text>();

			toryNotificationUILayer.SetActive(activate);
		}

		public void Show(string text, int duration)
		{
			if (toryNotificationUILayer == null)
			{
				InstanciateNotificationLayer();
			}

			message.text = text;
			toryNotificationUILayer.SetActive(true);

			if (duration > 0)
			{
				if (hideSoonCoroutine == null)
				{
					hideSoonCoroutine = StartCoroutine(HideSoonCoroutine(duration));
				}
				else
				{
					hideTime = Time.timeSinceLevelLoad + duration;
				}
			}
		}

		Coroutine hideSoonCoroutine;
		IEnumerator HideSoonCoroutine(float duration)
		{
			hideTime = Time.timeSinceLevelLoad + duration;
			yield return new WaitUntil(() => hideTime < Time.timeSinceLevelLoad);

			Hide();
			hideSoonCoroutine = null;
		}

		public void Hide()
		{
			if (toryNotificationUILayer != null)
			{
				// toryNotificationUILayer.SetActive(false);
				toryNotificationUILayer.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
			}
		}

		public void SetTextColor(Color c)
		{
			if (toryNotificationUILayer == null)
			{
				InstanciateNotificationLayer(false);
			}

			message.color = c;
		}

		public void SetBackgroundColor(Color c)
		{
			if (toryNotificationUILayer == null)
			{
				InstanciateNotificationLayer(false);
			}

			if (c.a >= 0.8f)
			{
				background.color = new Color(c.r, c.g, c.b, 0.8f);
			}
			else
			{
				background.color = c;
			}
		}
	}
}