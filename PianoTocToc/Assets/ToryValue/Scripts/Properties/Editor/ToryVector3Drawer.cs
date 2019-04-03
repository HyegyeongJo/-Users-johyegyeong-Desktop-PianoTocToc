using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToryValue.Editor 
{
	// PropertyDrawer: https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
	// Formating: https://forum.unity.com/threads/making-a-proper-drawer-similar-to-vector3-how.385532/
	[CustomPropertyDrawer(typeof(ToryVector3))]
	public class ToryVector3Drawer : PropertyDrawer
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
			float lw = 12f;                                 // Label width
			float fw = position.width - lw; 				// Field width
			float py = position.y;
			float ph = (position.height - EditorGUIUtility.standardVerticalSpacing * 3f) * 0.25f;  // position height.
			Rect keyLabelRect = new Rect(position.x, py, lw, ph);
			Rect keyRect = new Rect(position.x + lw, py, fw, ph);
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect valueLabelRect = new Rect(position.x, py, lw, ph);
			Rect valueRect = new Rect(position.x + lw, py, fw, ph);
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect defaultValueLabelRect = new Rect(position.x, py, lw, ph);
			Rect defaultValueRect = new Rect(position.x + lw, py, fw, ph);
			py += ph + EditorGUIUtility.standardVerticalSpacing;
			Rect savedValueLabelRect = new Rect(position.x, py, lw, ph);
			Rect savedValueRect = new Rect(position.x + lw, py, fw, ph);

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
			ToryVector3[] targets = new ToryVector3[property.serializedObject.targetObjects.Length];
			for (int i = 0; i < property.serializedObject.targetObjects.Length; i++)
			{
				targets[i] = fieldInfo.GetValue(property.serializedObject.targetObjects[i]) as ToryVector3;
			}

			// Draw the value field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.LabelField(valueLabelRect, new GUIContent("V", "Value"));
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("currentValue"), GUIContent.none);
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Trigger the value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("currentValue").vector3Value });
					}
				}
			}

			// Draw the default value field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.LabelField(defaultValueLabelRect, new GUIContent("D", "Default Value"));
				EditorGUI.PropertyField(defaultValueRect, property.FindPropertyRelative("defaultValue"), GUIContent.none);
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Trigger the default value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerDefaultValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("defaultValue").vector3Value });
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
				    !savedValueProperty.vector3Value.Equals(
					    PlayerPrefsElite.GetVector3(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
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
				EditorGUI.PropertyField(savedValueRect, savedValueProperty, GUIContent.none);

				// Revert the background color.
				GUI.backgroundColor = bgc;
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Trigger the saved value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerSavedValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").vector3Value });
					}
				}

				// Set the value to the playerprefs.
				if (SecureKeysChecker.CheckSecureKeys())
				{
					if (PlayerPrefsElite.key != null)
					{
						PlayerPrefsElite.SetVector3(KeyFormatter.GetSavedKey(keyProperty.stringValue),
						                            savedValueProperty.vector3Value);

						// Trigger the value saved event.
						for (int i = 0; i < targets.Length; i++)
						{
							MethodInfo method = targets[i].GetType().GetMethod("TriggerValueSavedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
							if (method != null)
							{
								method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").vector3Value });
							}
						}
					}
				}
			}
			// Sync the field and PlayerPrefs.
			if (PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(keyProperty.stringValue)))
			{
				if (!savedValueProperty.vector3Value.Equals(
					PlayerPrefsElite.GetVector3(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
				{
					savedValueProperty.vector3Value =
						PlayerPrefsElite.GetVector3(KeyFormatter.GetSavedKey(keyProperty.stringValue));
				}
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 4f + EditorGUIUtility.standardVerticalSpacing * 3f;
		}
	}
}
