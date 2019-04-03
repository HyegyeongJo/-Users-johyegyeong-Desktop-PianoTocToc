using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToryValue.Editor 
{
	// PropertyDrawer: https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
	// Formating: https://forum.unity.com/threads/making-a-proper-drawer-similar-to-vector3-how.385532/
	[CustomPropertyDrawer(typeof(ToryString))]
	public class ToryStringDrawer : PropertyDrawer
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
			// Label width: https://answers.unity.com/questions/606325/how-do-i-implement-draggable-properties-with-custo.html
			float fw = position.width * 0.25f; 				// Field width
			float lw = 12f;                                 // Label width
			float px = position.x;
			Rect keyRect = new Rect(px, position.y, fw, position.height);
			px += fw;
			Rect valueRect = new Rect(px, position.y, fw, position.height);
			px += fw;
			Rect defaultValueRect = new Rect(px, position.y, fw, position.height);
			px += fw;
			Rect savedValueRect = new Rect(px, position.y, fw, position.height);

			// Set label width.
			float cachedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = lw;

			// Draw the key field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), new GUIContent("K", "Key"));
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
			ToryString[] targets = new ToryString[property.serializedObject.targetObjects.Length];
			for (int i = 0; i < property.serializedObject.targetObjects.Length; i++)
			{
				targets[i] = fieldInfo.GetValue(property.serializedObject.targetObjects[i]) as ToryString;
			}

			// Draw the value field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("currentValue"), new GUIContent("V", "Value"));
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Trigger the value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("currentValue").stringValue });
					}
				}
			}

			// Draw the default value field.
			EditorGUI.BeginChangeCheck();
			{
				EditorGUI.PropertyField(defaultValueRect, property.FindPropertyRelative("defaultValue"), new GUIContent("D", "Default Value"));
			}
			if (EditorGUI.EndChangeCheck())
			{
				// Trigger the default value change event.
				for (int i = 0; i < targets.Length; i++)
				{
					MethodInfo method = targets[i].GetType().GetMethod("TriggerDefaultValueChangedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("defaultValue").stringValue });
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
					!savedValueProperty.stringValue.Equals(
						PlayerPrefsElite.GetString(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
				{
					// Change the color to red,
					GUI.backgroundColor = new Color32(255, 0, 41, 255);
				}
				else
				{
					// Change the color to blue.
					GUI.backgroundColor = new Color32(0, 161, 223, 255);
				}

				EditorGUI.PropertyField(savedValueRect, savedValueProperty, new GUIContent("S", "Saved Value"));

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
						method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").stringValue });
					}
				}

				// Set the value to the playerprefs.
				if (SecureKeysChecker.CheckSecureKeys())
				{
					if (PlayerPrefsElite.key != null)
					{
						PlayerPrefsElite.SetString(KeyFormatter.GetSavedKey(keyProperty.stringValue),
						                           savedValueProperty.stringValue);

						// Trigger the value saved event.
						for (int i = 0; i < targets.Length; i++)
						{
							MethodInfo method = targets[i].GetType().GetMethod("TriggerValueSavedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
							if (method != null)
							{
								method.Invoke(targets[i], new object[] { property.FindPropertyRelative("savedValue").stringValue });
							}
						}
					}
				}
			}
			// Sync the field and PlayerPrefs.
			if (PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(keyProperty.stringValue)))
			{
				if (!savedValueProperty.stringValue.Equals(
					PlayerPrefsElite.GetString(KeyFormatter.GetSavedKey(keyProperty.stringValue))))
				{
					savedValueProperty.stringValue =
						PlayerPrefsElite.GetString(KeyFormatter.GetSavedKey(keyProperty.stringValue));
				}
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			// Set the label width back to what it was.
			EditorGUIUtility.labelWidth = cachedLabelWidth;

			EditorGUI.EndProperty();
		}
	}
}
