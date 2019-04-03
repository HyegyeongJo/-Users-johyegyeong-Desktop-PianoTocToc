using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToryValue;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	/// <summary>
	/// <c>MonoBehaviour</c> component to set ui orientation certain direction.
	/// Has a static property and a static method to change the orientation at ease without referencing <c>MonoBehaviour</c> component.
	/// <c>UIOrientationSetter.CurrentOrientation</c> returns current UIOrientation value.
	/// <c>UIOrientationSetter.SetOrientation(newOrientation)</c> sets new ui orientation.
	/// </summary>
	[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
	public class UIOrientationSetter : MonoCollection<UIOrientationSetter>, IDefaultValueSetter
	{
		public UIOrientation Orientation
		{
			get
			{
				return orientation;
			}
			set
			{
				orientation = value;
				ApplyOrientation();
			}
		}
		public UIOrientation orientation;

		#if UNITY_EDITOR
		private UIOrientation previousOrientation;
		#endif

		/// <summary>
		/// Returns current ui orientation.
		/// When there are more than one UIOrientationSetter in the scene, it will look for one of them and return the value.
		/// </summary>
		public static UIOrientation CurrentOrientation
		{
			get
			{
				try
				{
					return allEnabledIntances.First().Orientation;
				}
				catch
				{
					return UIOrientation.Unknown;
				}
			}
		}
		static UIOrientation defaultOrientation;

		private CanvasScaler canvasScaler;
		private RectTransform rectTransform;

		static ToryBool isFlipped;
		public static ToryBool IsFlipped
		{
			get
			{
				if (isFlipped == null || string.IsNullOrEmpty(isFlipped.Key))
				{
					isFlipped = new ToryBool("IsUIOrientationFlipped");
				}
				return isFlipped;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			canvasScaler = GetComponentInParent<CanvasScaler>();
			if (canvasScaler == null)
			{
				#if UNITY_EDITOR || DEVELOPMENT_BUILD
				Debug.LogWarning("UIOrientationSetter needs CanvasScaler component in its parent.");
				#endif
				gameObject.SetActive(false);
			}

			CanvasScaler scaler = null;
			foreach (var orientationSetter in allEnabledIntances)
			{
				if (scaler == null)
				{
					scaler = orientationSetter.canvasScaler;
				}
				else
				{
					if (scaler.referenceResolution.Equals(orientationSetter.canvasScaler.referenceResolution))
					{
						scaler = orientationSetter.canvasScaler;
					}
					else
					{
						#if UNITY_EDITOR || DEVELOPMENT_BUILD
						Debug.LogError("All CanvasScaler of all UIOrientationSetter's parent should share the same value!\\nThis inconsistency may lead to inaccurate UI layout.");
						#endif
					}
				}
			}

			defaultOrientation = CurrentOrientation;
			rectTransform = GetComponent<RectTransform>();
		}

		void Start()
		{
			if (Application.isPlaying)
			{
				IsFlipped.ValueChanged += delegate
				{
					SetOrientation();
				};

				IsFlipped.LoadSavedValue();
				SetOrientation();
			}
		}

		#if UNITY_EDITOR
		void Update()
		{
			if (!Application.isPlaying)
			{
				if (!Orientation.Equals(previousOrientation))
				{
					previousOrientation = Orientation;

					if (allEnabledIntances.Count < 1)
					{
						Awake();
					}
					foreach (var orientationSetter in allEnabledIntances)
					{
						orientationSetter.Orientation = Orientation;
						orientationSetter.ApplyOrientation();
					}
				}
			}
		}
		#endif

		void ApplyOrientation()
		{
			switch (Orientation)
			{
				case UIOrientation.Landscape:
					rectTransform.sizeDelta = canvasScaler.referenceResolution;
					rectTransform.localEulerAngles = Vector3.zero;
					break;
				case UIOrientation.LandscapeUpsideDown:
					rectTransform.sizeDelta = canvasScaler.referenceResolution;
					rectTransform.localEulerAngles = new Vector3(0, 0, 180f);
					break;
				case UIOrientation.PortraitLeft:
					rectTransform.sizeDelta = new Vector2(canvasScaler.referenceResolution.y, canvasScaler.referenceResolution.x);
					rectTransform.localEulerAngles = new Vector3(0, 0, 90f);
					break;
				case UIOrientation.PortraitRight:
					rectTransform.sizeDelta = new Vector2(canvasScaler.referenceResolution.y, canvasScaler.referenceResolution.x);
					rectTransform.localEulerAngles = new Vector3(0, 0, -90f);
					break;
			}
		}

		public void RevertToDefault()
		{
			IsFlipped.LoadDefaultValue();
			IsFlipped.Save();
		}

		/// <summary>
		/// Change ui orientation to a specific UIOrientation.
		/// </summary>
		/// <param name="newOrientation">New orientation.</param>
		static void SetOrientation()
		{
			foreach (var orientationSetter in UIOrientationSetter.allEnabledIntances)
			{
				orientationSetter.Orientation = IsFlipped.Value? FlippedOrientation() : defaultOrientation;
			}
		}

		/// <summary>
		/// Flips ui orientation; i.e., landscape left to landscape right.
		/// </summary>
		static UIOrientation FlippedOrientation()
		{
			switch (defaultOrientation)
			{
				case UIOrientation.Landscape:
					return UIOrientation.LandscapeUpsideDown;
				case UIOrientation.LandscapeUpsideDown:
					return UIOrientation.Landscape;
				case UIOrientation.PortraitLeft:
					return UIOrientation.PortraitRight;
				case UIOrientation.PortraitRight:
					return UIOrientation.PortraitLeft;
			}
			return UIOrientation.Unknown;
		}

		/// <summary>
		/// Returns reference resolution of CanvasScaler in UIOrientationSetter's parent.
		/// IMPORTANT: To make this value reliable, all referenced CanvasScaler should share the same values.
		/// </summary>
		/// <value>The reference resolution.</value>
		public static Vector2 ReferenceResolution
		{
			get
			{
				if (allEnabledIntances.Count == 0)
				{
					#if UNITY_EDITOR || DEVELOPMENT_BUILD
					Debug.Log("No orientationSetter added to UIOrientationSetter yet.");
					#endif
					return Vector2.zero;
				}

				var orientationSetter = allEnabledIntances.First();

				#if UNITY_EDITOR
				if (orientationSetter == null)
				{
					Debug.Log("orientationSetter == null");
					return Vector2.zero;
				}
				#endif

				switch (orientationSetter.Orientation)
				{
					case UIOrientation.PortraitLeft:
					case UIOrientation.PortraitRight:
						return new Vector2(orientationSetter.canvasScaler.referenceResolution.y, orientationSetter.canvasScaler.referenceResolution.x);

					case UIOrientation.Landscape:
					case UIOrientation.LandscapeUpsideDown:
						return orientationSetter.canvasScaler.referenceResolution;

					default:
						return Vector2.zero;
				}
			}
		}
	}

	public enum UIOrientation
	{
		Unknown = 0,
		Landscape,
		LandscapeUpsideDown,
		PortraitLeft,
		PortraitRight
	}
}