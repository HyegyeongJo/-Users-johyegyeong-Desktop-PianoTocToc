using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
	#if UNITY_EDITOR
	[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
	#else
	[RequireComponent(typeof(RectTransform))]
	#endif
	public class UISafeZoneSetter : MonoBehaviour
	{
		private RectTransform parentRectTrasnform;
		private RectTransform rectTransform;

		public float marginLeft, marginRight, marginTop, marginBottom;

		#if UNITY_EDITOR
		void OnEnable()
		{
			Awake();
		}
		#endif

		void Awake()
		{
			parentRectTrasnform = GetComponentInParent<RectTransform>();
			if (parentRectTrasnform == null)
			{
				#if UNITY_EDITOR || DEVELOPMENT_BUILD
				Debug.LogWarning("UISafeZoneSetter needs RectTransform component in its parent.");
				#endif
				gameObject.SetActive(false);
			}

			rectTransform = GetComponent<RectTransform>();
		}

		#if UNITY_EDITOR
		void Update()
		{
			if (!Application.isPlaying)
			{
				if (SafeZoneChanged())
				{
					rectTransform.offsetMin = new Vector2(marginLeft, marginBottom);
					rectTransform.offsetMax = new Vector2(-marginRight, -marginTop);
				}
			}
		}
		#endif

		bool SafeZoneChanged()
		{
			if (rectTransform.offsetMin.x != marginLeft ||
				rectTransform.offsetMin.y != marginBottom ||
				rectTransform.offsetMax.x != -marginRight ||
				rectTransform.offsetMax.y != -marginTop)
			{
				return true;
			}

			return false;
		}
	}
}