using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	public class WebcamToggleListSetter : MonoBehaviour
	{
		int maxCharLength = 25;

		#pragma warning disable 0414
		VerticalLayoutGroup verticalLayoutGroup;
		ToryToggle toryToggle;

		void Awake()
		{
			ToryToggle toryToggle = GetComponent<ToryToggle>();
			verticalLayoutGroup = GetComponentInParent<VerticalLayoutGroup>();

			if (UIOrientationSetter.CurrentOrientation == UIOrientation.PortraitLeft ||
				UIOrientationSetter.CurrentOrientation == UIOrientation.PortraitRight)
			{
				maxCharLength = 12;
			}

			toryToggle.options = new string[WebCamTexture.devices.Length];
			for (int i = 0; i < toryToggle.options.Length; i++)
			{
				string s = WebCamTexture.devices[i].name;
				toryToggle.options[i] = (s.Length < maxCharLength) ? s : s.Substring(0, maxCharLength).Trim() + "...";
			}
		}
	}
}