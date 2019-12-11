using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[ExecuteInEditMode]
	public class ToryVerticalLayoutGroup : VerticalLayoutGroup
	{
		public int columnPaddingOnLandscape = 200;
		public int columnPaddingOnPortrait = 100;

		public int letterboxPaddingOnLandscape = 100;
		public int letterboxPaddingOnPortrait = 200;

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetLayout();
		}

		#if UNITY_EDITOR
		void Update()
		{
			if (!Application.isPlaying)
			{
				SetLayout();
			}
		}
		#endif

		public void SetLayout()
		{
			switch (UIOrientationSetter.CurrentOrientation)
			{
			case UIOrientation.Landscape:
			case UIOrientation.LandscapeUpsideDown:
				padding.left = columnPaddingOnLandscape;
				padding.right = columnPaddingOnLandscape;
				padding.top = letterboxPaddingOnLandscape;
				padding.bottom = letterboxPaddingOnLandscape;
				break;

			case UIOrientation.PortraitLeft:
			case UIOrientation.PortraitRight:
				padding.left = columnPaddingOnPortrait;
				padding.right = columnPaddingOnPortrait;
				padding.top = letterboxPaddingOnPortrait;
				padding.bottom = letterboxPaddingOnPortrait;
				break;

			default:
				break;
			}
		}
	}
}