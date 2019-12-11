using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
	[RequireComponent(typeof(TorySimpleToggle))]
	public class FlipUIOrientationSetter : DefaultValueSetterMonoBehaviour
	{
		private TorySimpleToggle toggleComponent;
		public TorySimpleToggle ToggleComponent
		{
			get
			{
				if (toggleComponent == null)
				{
					toggleComponent = GetComponent<TorySimpleToggle>();
				}
				return toggleComponent;
			}
		}

		[SerializeField]
		bool defaultValue;

		public void OnEnable()
		{
			UIOrientationSetter.IsFlipped.LoadSavedValue();
			ToggleComponent.isOn = UIOrientationSetter.IsFlipped.Value;
			defaultValue = UIOrientationSetter.IsFlipped.DefaultValue;

			ToggleComponent.UpdateValue();
		}

		public void FlipUIOrientation(bool flip)
		{
			UIOrientationSetter.IsFlipped.Value = flip;
			UIOrientationSetter.IsFlipped.Save();
		}

		public override void RevertToDefault()
		{
			UIOrientationSetter.IsFlipped.DefaultValue = defaultValue;
			UIOrientationSetter.IsFlipped.LoadDefaultValue();
			UIOrientationSetter.IsFlipped.Save();

			// Fetch UI value.
			ToggleComponent.isOn = UIOrientationSetter.IsFlipped.Value;
			ToggleComponent.UpdateValue();
		}
	}
}