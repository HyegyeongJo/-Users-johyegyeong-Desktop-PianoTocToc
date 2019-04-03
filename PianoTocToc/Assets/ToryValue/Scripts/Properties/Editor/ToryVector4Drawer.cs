using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToryValue.Editor 
{
	// PropertyDrawer: https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
	// Formating: https://forum.unity.com/threads/making-a-proper-drawer-similar-to-vector3-how.385532/
	[CustomPropertyDrawer(typeof(ToryVector4))]
	public class ToryVector4Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);

			// Draw label
			TooltipAttribute[] tooltipAttr = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true) as TooltipAttribute[];
			if (tooltipAttr.Length > 0)
			{
				label.tooltip = tooltipAttr[0].tooltip;
			}
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Calculate rects
			float lw = 12f;                                 			// Label width
			float fw = (position.width - lw) * 0.25f; 					// Field width
			float ph = (position.height - EditorGUIUtility.standardVerticalSpacing * 3f) * 0.25f;	// position height.
			float px = position.x;
			float py = position.y;

			// Label
			Rect keyLabelRect = new Rect(px, py, lw, ph);
			px += lw;
			Rect keyRect = new Rect(px, py, position.width - lw, ph);

			// Value field
			px = position.x;
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect valueLabelRect = new Rect(px, py, lw, ph);
			px += lw;
			Rect valueXRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect valueYRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect valueZRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect valueWRect = new Rect(px, py, fw, ph);

			// Default value field
			px = position.x;
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect defaultValueLabelRect = new Rect(px, py, lw, ph);
			px += lw;
			Rect defaultValueXRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect defaultValueYRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect defaultValueZRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect defaultValueWRect = new Rect(px, py, fw, ph);

			// Saved value field
			px = position.x;
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect savedValueLabelRect = new Rect(px, py, lw, ph);
			px += lw;
			Rect savedValueXRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect savedValueYRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect savedValueZRect = new Rect(px, py, fw, ph);
			px += fw;
			Rect savedValueWRect = new Rect(px, py, fw, ph);

			// Draw the key field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.LabelField(keyLabelRect, new GUIContent("K", "Key"));
				EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Do nth.
			}

			// Set the key.
			SerializedProperty keyProperty = property.FindPropertyRelative("key");
			string key = keyProperty.stringValue;
			if (key.Equals(""))
			{
				keyProperty.stringValue = label.text;
			}

			// Get targets.
			ToryVector4[] targets = new ToryVector4[property.serializedObject.targetObjects.Length];
			for (int i = 0; i < property.serializedObject.targetObjects.Length; i++)
			{
				targets[i] = fieldInfo.GetValue(property.serializedObject.targetObjects[i]) as ToryVector4;
			}

			// Set label width.
			float cachedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = lw;

			// Draw the value field.
			Vector4 val = Vector4.zero;
			SerializedProperty currentValueProperty = property.FindPropertyRelative("currentValue");
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.LabelField(valueLabelRect, new GUIContent("V", "Value"));
				val.x = EditorGUI.FloatField(valueXRect, new GUIContent("X"), currentValueProperty.vector4Value.x);
				val.y = EditorGUI.FloatField(valueYRect, new GUIContent("Y"), currentValueProperty.vector4Value.y);
				val.z = EditorGUI.FloatField(valueZRect, new GUIContent("Z"), currentValueProperty.vector4Value.z);
				val.w = EditorGUI.FloatField(valueWRect, new GUIContent("W"), currentValueProperty.vector4Value.w);
			}
			if (EditorGUI.EndChangeCheck())
			{
				currentValueProperty.vector4Value = val;

				// Trigger the value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("currentValue").vector4Value });
					}
				}
			}

			// Draw the default value field.
			SerializedProperty defaultValueProperty = property.FindPropertyRelative("defaultValue");
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.LabelField(defaultValueLabelRect, new GUIContent("D", "Default Value"));
				val.x = EditorGUI.FloatField(defaultValueXRect, new GUIContent("X"), defaultValueProperty.vector4Value.x);
				val.y = EditorGUI.FloatField(defaultValueYRect, new GUIContent("Y"), defaultValueProperty.vector4Value.y);
				val.z = EditorGUI.FloatField(defaultValueZRect, new GUIContent("Z"), defaultValueProperty.vector4Value.z);
				val.w = EditorGUI.FloatField(defaultValueWRect, new GUIContent("W"), defaultValueProperty.vector4Value.w);
			}
			if (EditorGUI.EndChangeCheck())
			{
				defaultValueProperty.vector4Value = val;

				// Trigger the default value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerDefaultValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("defaultValue").vector4Value });
					}
				}
			}

			// Draw the saved value field.
			SerializedProperty savedValueProperty = property.FindPropertyRelative("savedValue");
			EditorGUI.BeginChangeCheck();
			{
				// Change the background color according to the existance of the playerpref value.
				Color bgc = GUI.backgroundColor;
				if (!PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(keyProperty.stringValue)) ||
					!savedValueProperty.vector4Value.Equals(
						PlayerPrefsElite.GetVector4(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
				{
					// Change the color to red,
					GUI.backgroundColor = new Color32(255, 0, 41, 255);
				}
				else
				{
					// Change the color to blue.
					GUI.backgroundColor = new Color32(0, 161, 223, 255);
				}

				EditorGUI.LabelField(savedValueLabelRect, new GUIContent("S", "Saved Value"));
				val.x = EditorGUI.FloatField(savedValueXRect, new GUIContent("X"), savedValueProperty.vector4Value.x);
				val.y = EditorGUI.FloatField(savedValueYRect, new GUIContent("Y"), savedValueProperty.vector4Value.y);
				val.z = EditorGUI.FloatField(savedValueZRect, new GUIContent("Z"), savedValueProperty.vector4Value.z);
				val.w = EditorGUI.FloatField(savedValueWRect, new GUIContent("W"), savedValueProperty.vector4Value.w);

				// Revert the background color.
				GUI.backgroundColor = bgc;
			}
			if (EditorGUI.EndChangeCheck())
			{
				savedValueProperty.vector4Value = val;

				// Trigger the saved value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerSavedValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").vector4Value });
					}
				}

				// Set the value to the playerprefs.
				if (SecureKeysChecker.CheckSecureKeys())
				{
					if (PlayerPrefsElite.key != null)
					{
						PlayerPrefsElite.SetVector4(KeyFormatter.GetSavedKey(keyProperty.stringValue), 
						                            savedValueProperty.vector4Value);

						// Trigger the value saved event.
						for (int i = 0; i < targets.Length; i++)
						{
							MethodInfo method = targets[i].GetType().GetMethod("TriggerValueSavedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
							if (method != null)
							{
								method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").vector4Value });
							}
						}
					}
				}
			}
			// Sync the field and PlayerPrefs.
			if (PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(keyProperty.stringValue)))
			{
				if (!savedValueProperty.vector4Value.Equals(
					PlayerPrefsElite.GetVector4(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
				{
					savedValueProperty.vector4Value = 
						PlayerPrefsElite.GetVector4(KeyFormatter.GetSavedKey(keyProperty.stringValue));
				}
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			// Set the label width back to what it was.
			EditorGUIUtility.labelWidth = cachedLabelWidth;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 4f + EditorGUIUtility.standardVerticalSpacing * 3f;
		}
	}
}
