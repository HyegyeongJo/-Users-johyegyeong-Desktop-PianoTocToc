using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ToryUX
{
	[CustomEditor(typeof(TorySlider))]
	public class TorySliderEditor : SliderEditor
	{
		TorySlider component;

		RectTransform wholeAreaTransform;
		RectTransform sliderAreaTransform;
		float sliderAreaTransformLeft;
		float sliderAreaTransformRight;

		SerializedProperty stepCounts;
		SerializedProperty valueMultiplier;
		SerializedProperty valueFormat;
		SerializedProperty valueUnit;

		SerializedProperty bindToryValueType;
		string[] bindValueTypeStrings;

		SerializedProperty toryFloatProperty;
		SerializedProperty toryIntProperty;

		bool bindToryValueProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			component = (TorySlider) target;

			stepCounts = serializedObject.FindProperty("stepCounts");
			valueMultiplier = serializedObject.FindProperty("valueMultiplier");
			valueFormat = serializedObject.FindProperty("valueFormat");
			valueUnit = serializedObject.FindProperty("valueUnit");

			bindToryValueType = serializedObject.FindProperty("bindToryValueType");
			toryFloatProperty = serializedObject.FindProperty("toryFloatProperty");
			toryIntProperty = serializedObject.FindProperty("toryIntProperty");

			wholeAreaTransform = component.GetComponent<RectTransform>();
			try
			{
				sliderAreaTransform = component.transform.Find("SliderArea").GetComponent<RectTransform>();
				sliderAreaTransformLeft = sliderAreaTransform.offsetMin.x;
				sliderAreaTransformRight = wholeAreaTransform.rect.width + sliderAreaTransform.offsetMax.x;
			}
			catch
			{
				Debug.LogErrorFormat("TorySlider {0} cannot find Text object named \"SliderArea\" among its children.", name);
			}

			bindValueTypeStrings = new string[]
			{
				"-",
				Enum.GetName(typeof(BindToryValueType), BindToryValueType.Float), // enum 1
				Enum.GetName(typeof(BindToryValueType), BindToryValueType.Int) // enum 2
			};

			if (ToryValue.SecureKeysChecker.CheckSecureKeys())
			{
				if (PlayerPrefsElite.key != null)
				{
					component.BindToryValue();
				}
			}
			bindToryValueProperty = false;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (component.interactable)
			{
				component.handleRect.gameObject.SetActive(true);
			}
			else
			{
				component.handleRect.gameObject.SetActive(false);
			}

			if (component.labelText != null)
			{
				EditorGUILayout.Space();
				Undo.RecordObject(component.labelText, "Label text change");
				component.labelText.text = EditorGUILayout.TextField("Label", component.labelText.text);
				EditorGUILayout.Space();
			}

			if (sliderAreaTransform != null)
			{
				EditorGUILayout.LabelField("Slider Size", EditorStyles.boldLabel);
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.MinMaxSlider(ref sliderAreaTransformLeft, ref sliderAreaTransformRight, 0, wholeAreaTransform.rect.width);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Left", GUILayout.MaxWidth(50));
				sliderAreaTransformLeft = EditorGUILayout.FloatField(sliderAreaTransformLeft, GUILayout.MaxWidth(50));
				EditorGUILayout.LabelField(string.Format("⇤ {0} ⇥", (sliderAreaTransformRight - sliderAreaTransformLeft).ToString()), EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(0));
				EditorGUILayout.LabelField("Right", GUILayout.MaxWidth(50));
				sliderAreaTransformRight = wholeAreaTransform.rect.width - EditorGUILayout.FloatField(wholeAreaTransform.rect.width - sliderAreaTransformRight, GUILayout.MaxWidth(50));
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(sliderAreaTransform, "Slider area size change");
					sliderAreaTransform.offsetMin = new Vector2(sliderAreaTransformLeft, 0);
					Undo.RecordObject(sliderAreaTransform, "Slider area size change");
					sliderAreaTransform.offsetMax = new Vector2(sliderAreaTransformRight - wholeAreaTransform.rect.width, 0);
				}
				EditorGUILayout.Space();
			}

			serializedObject.ApplyModifiedProperties();

			float prevValue = component.value;
			bool prevWholeNumbers = component.wholeNumbers;

			base.OnInspectorGUI();
			serializedObject.Update();

			if (component.wholeNumbers != prevWholeNumbers)
			{
				component.BindToryValue();
				serializedObject.Update();
			}

			if (component.value != prevValue)
			{
				serializedObject.ApplyModifiedProperties();
				component.UpdateValue(component.value);
				serializedObject.Update();
			}

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(stepCounts);
			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(valueMultiplier);
			EditorGUILayout.PropertyField(valueFormat);
			EditorGUILayout.PropertyField(valueUnit);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				component.UpdateValue(component.value);
				serializedObject.Update();
			}

			// ToryValue Fields

			EditorGUILayout.Space();

			if (bindToryValueProperty)
			{
				serializedObject.ApplyModifiedProperties();
				component.BindToryValue();
				serializedObject.Update();
				bindToryValueProperty = false;
			}

			EditorGUI.BeginChangeCheck();
			int bindToryValueTypeIndex = (int) bindToryValueType.enumValueIndex; // Don't need to modify index in TorySlider.
			bindToryValueType.enumValueIndex = EditorGUILayout.Popup("Bind Tory Value", bindToryValueTypeIndex, bindValueTypeStrings);
			if (EditorGUI.EndChangeCheck())
			{
				bindToryValueProperty = true;
				serializedObject.ApplyModifiedProperties();
				return;
			}

			if ((BindToryValueType) bindToryValueType.enumValueIndex == BindToryValueType.Float)
			{
				component.wholeNumbers = false;

				if (component.toryFloatProperty.GetPersistentEventCount() > 1)
				{
				EditorGUILayout.HelpBox("If number of bound ToryValue exceed 1, default and current value of ToryUX component follows first bound ToryValue.", MessageType.Warning);
				}
				else
				{
					EditorGUILayout.HelpBox("Only \"Property\" of ToryValue in gameobject component can be bound with ToryUX component.", MessageType.Info);
				}

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(toryFloatProperty);
				if (EditorGUI.EndChangeCheck())
				{
					bindToryValueProperty = true;
					serializedObject.ApplyModifiedProperties();
					return;
				}

				if (component.boundToryFloats.Count > 0)
				{
					component.DefaultValue = EditorGUILayout.FloatField("Default Value", (component.DefaultValue != null) ? (float) component.DefaultValue : 0f);
					if (GUILayout.Button("Revert To Default"))
					{
						component.RevertToDefault();
					}
				}
			}
			else if ((BindToryValueType) bindToryValueType.enumValueIndex == BindToryValueType.Int)
			{
				component.wholeNumbers = true;

				if (component.toryIntProperty.GetPersistentEventCount() > 1)
				{
				EditorGUILayout.HelpBox("If number of bound ToryValue exceed 1, default and current value of ToryUX component follows first bound ToryValue.", MessageType.Warning);
				}
				else
				{
					EditorGUILayout.HelpBox("Only \"Property\" of ToryValue in gameobject component can be bound with ToryUX component.", MessageType.Info);
				}

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(toryIntProperty);
				if (EditorGUI.EndChangeCheck())
				{
					bindToryValueProperty = true;
					serializedObject.ApplyModifiedProperties();
					return;
				}

				if (component.boundToryInts.Count > 0)
				{
					component.DefaultValue = EditorGUILayout.IntField("Default Value", (component.DefaultValue != null) ? (int) component.DefaultValue : 0);
					if (GUILayout.Button("Revert To Default"))
					{
						component.RevertToDefault();
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}