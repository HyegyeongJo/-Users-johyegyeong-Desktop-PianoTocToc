using UnityEngine;
using UnityEditor;

namespace ToryValue
{
	// PropertyDrawer: https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
	public abstract class ToryValueDrawer<T> : PropertyDrawer
	{
		protected Rect position, relativePropertiesPosition;
		protected GUIContent label;
		protected SerializedProperty property, keyProperty, valueProperty, defaultValueProperty, savedValueProperty;
		protected ToryValue<T>[] inspectedToryValues;

		int cachedIndentLevel;
		float cachedLabelWidth;
		Color cachedBackgroundColor;
		protected float labelWidth = 12f;

		Color32 colorWhenSaved = new Color32(0, 161, 223, 255);
		Color32 colorWhenNotSaved = new Color32(255, 0, 41, 255);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			DrawProperty(position, label, property);
			EditorGUI.EndProperty();
		}

		void DrawProperty(Rect position, GUIContent label, SerializedProperty property)
		{
			PrepareDrawingProperty(position, label, property);
			DrawPrefixLabel();
			PrepareDrawingRelativeProperties();
			DrawKeyProperty();
			DrawValueProperty();
			DrawDefaultValueProperty();
			DrawSavedValueProperty();
			RestoreEditorGUI();
		}

		void PrepareDrawingProperty(Rect position, GUIContent label, SerializedProperty property)
		{
			this.position = position;
			this.label = label;
			this.property = property;
			inspectedToryValues = GetInspectedToryValues(property);
		}

		ToryValue<T>[] GetInspectedToryValues(SerializedProperty serializedProperty)
		{
			ToryValue<T>[] toryValues = new ToryValue<T>[serializedProperty.serializedObject.targetObjects.Length];
			object inspectedObject;
			for (int i = 0; i < toryValues.Length; i++)
			{
				inspectedObject = fieldInfo.GetValue(serializedProperty.serializedObject.targetObjects[i]);
				toryValues[i] = GetActualObject(inspectedObject, serializedProperty);
			}
			return toryValues;
		}

		protected abstract ToryValue<T> GetActualObject(object inspectedObject, SerializedProperty serializedProperty);

		void DrawPrefixLabel()
		{
			DrawTooltip();
			DrawPrefixLabelAndSetRelativePropertiesPosition();
		}

		void DrawTooltip()
		{
			TooltipAttribute[] tooltipAttr = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true) as TooltipAttribute[];
			if (tooltipAttr.Length > 0)
			{
				label.tooltip = tooltipAttr[0].tooltip;
			}
			else
			{
				label.tooltip = "";
			}
		}

		void DrawPrefixLabelAndSetRelativePropertiesPosition()
		{
			relativePropertiesPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		}

		protected virtual void PrepareDrawingRelativeProperties()
		{
			SetIndentLevel(0);
			SetLabelWidth(labelWidth);
			keyProperty = property.FindPropertyRelative("key");
			valueProperty = property.FindPropertyRelative("value");
			defaultValueProperty = property.FindPropertyRelative("defaultValue");
			savedValueProperty = property.FindPropertyRelative("savedValue");
		}

		void SetIndentLevel(int level)
		{
			cachedIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = level;
		}

		void SetLabelWidth(float width)
		{
			cachedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = width;
		}

		void DrawKeyProperty()
		{
			DrawKeyLabelField();
			DrawKeyPropertyField();
			FillKeyPropertyIfEmpty();
		}

		protected virtual void DrawKeyLabelField()
		{
			// We draw the both label and property fields together in the DrawKeyPropertyField method.
		}

		// Draggable property: https://answers.unity.com/questions/606325/how-do-i-implement-draggable-properties-with-custo.html
		protected virtual void DrawKeyPropertyField()
		{
			Rect keyPosition = GetRelativePropertyPositionAt(0);
			EditorGUI.PropertyField(keyPosition, keyProperty, new GUIContent("K", "Key"));
		}

		// Vector3-like drawing: https://forum.unity.com/threads/making-a-proper-drawer-similar-to-vector3-how.385532/
		Rect GetRelativePropertyPositionAt(int ithProperty)
		{
			float propertyWidth = relativePropertiesPosition.width * 0.25f;
			float posX = relativePropertiesPosition.x + propertyWidth * ithProperty;
			return new Rect(posX, relativePropertiesPosition.y, propertyWidth, relativePropertiesPosition.height);
		}

		void FillKeyPropertyIfEmpty()
		{
			if (keyProperty.stringValue.Equals("") || keyProperty.stringValue.Equals(property.name) ||
				KeyPropertyHasBeenSetByEditorAutomatically())
			{
				keyProperty.stringValue = property.serializedObject.targetObject.name + "." + property.propertyPath;
			}
		}

		bool KeyPropertyHasBeenSetByEditorAutomatically()
		{
			return keyProperty.stringValue.Split('[')[0].Equals(property.serializedObject.targetObject.name + "." + property.propertyPath.Split('[')[0]);
		}

		void DrawValueProperty()
		{
			EditorGUI.BeginChangeCheck();
			DrawValueLabelField();
			DrawValuePropertyField();
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(valueProperty.serializedObject.targetObjects, "Value Change");
				ApplyChangeToInspectedToryValues();
			}
		}

		protected virtual void DrawValueLabelField()
		{
			// We draw the both label and property fields in the DrawValuePropertyField method.
		}

		protected virtual void DrawValuePropertyField()
		{
			Rect valuePosition = GetRelativePropertyPositionAt(1);
			EditorGUI.PropertyField(valuePosition, valueProperty, new GUIContent("V", "Value"));
		}

		void ApplyChangeToInspectedToryValues()
		{
			for (int i = 0; i < inspectedToryValues.Length; i++)
			{
				ApplyChangeToInspectedToryValue(inspectedToryValues[i]);
				PrefabUtility.RecordPrefabInstancePropertyModifications(valueProperty.serializedObject.targetObjects[i]);
			}
		}

		protected abstract void ApplyChangeToInspectedToryValue(ToryValue<T> toryValue);

		void DrawDefaultValueProperty()
		{
			EditorGUI.BeginChangeCheck();
			DrawDefaultValueLabelField();
			DrawDefaultValuePropertyField();
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(defaultValueProperty.serializedObject.targetObjects, "Default Value Change");
				ApplyChangeToInspectedDefaultValues();
			}
		}

		protected virtual void DrawDefaultValueLabelField()
		{
			// We draw the both label and property fields in the DrawDefaultValuePropertyField method.
		}

		protected virtual void DrawDefaultValuePropertyField()
		{
			Rect defaultValuePosition = GetRelativePropertyPositionAt(2);
			EditorGUI.PropertyField(defaultValuePosition, defaultValueProperty, new GUIContent("D", "Default value"));
		}

		void ApplyChangeToInspectedDefaultValues()
		{
			for (int i = 0; i < inspectedToryValues.Length; i++)
			{
				ApplyChangeToInspectedDefaultValue(inspectedToryValues[i]);
				PrefabUtility.RecordPrefabInstancePropertyModifications(defaultValueProperty.serializedObject.targetObjects[i]);
			}
		}

		protected abstract void ApplyChangeToInspectedDefaultValue(ToryValue<T> toryValue);

		void DrawSavedValueProperty()
		{
			EditorGUI.BeginChangeCheck();
			DrawSavedValuePropertyAndBackground();
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(savedValueProperty.serializedObject.targetObjects, "Saved Value Change");
				PlayerPrefsEliteUtility.MakePlayerPrefsEliteAvailableInEditMode();
				ApplyChangeToInspectedSavedValues();
				SaveChangeToInspectedToryValues();
			}
			if (SavedButInconsistent())
			{
				PlayerPrefsEliteUtility.MakePlayerPrefsEliteAvailableInEditMode();
				FetchInspectedToryValueSavedValue();
			}
		}

		void DrawSavedValuePropertyAndBackground()
		{
			CacheBackgroundColor();
			DrawBackground();
			DrawSavedValueLabelField();
			DrawSavedValuePropertyField();
			RestoreBackgroundColor();
		}

		void CacheBackgroundColor()
		{
			cachedBackgroundColor = GUI.backgroundColor;
		}

		void DrawBackground()
		{
			GUI.backgroundColor = Saved() ? (Color)colorWhenSaved : (Color)colorWhenNotSaved;
		}

		protected abstract bool Saved();

		protected virtual void DrawSavedValueLabelField()
		{
			// We draw the both label and property fields in the DrawSavedPropertyField method.
		}

		protected virtual void DrawSavedValuePropertyField()
		{
			Rect savedValuePosition = GetRelativePropertyPositionAt(3);
			string tooltip = "Saved value (" + (Saved() ? "saved" : "not saved") + ")";
			EditorGUI.PropertyField(savedValuePosition, savedValueProperty, new GUIContent("S", tooltip));
		}

		void RestoreBackgroundColor()
		{
			GUI.backgroundColor = cachedBackgroundColor;
		}

		void ApplyChangeToInspectedSavedValues()
		{
			for (int i = 0; i < inspectedToryValues.Length; i++)
			{
				ApplyChangeToInspectedSavedValue(inspectedToryValues[i]);
				PrefabUtility.RecordPrefabInstancePropertyModifications(savedValueProperty.serializedObject.targetObjects[i]);
			}
		}

		protected abstract void ApplyChangeToInspectedSavedValue(ToryValue<T> toryValue);

		void SaveChangeToInspectedToryValues()
		{
			for (int i = 0; i < inspectedToryValues.Length; i++)
			{
				SaveInspectedToryValue(inspectedToryValues[i]);
			}
		}

		protected abstract void SaveInspectedToryValue(ToryValue<T> toryValue);

		protected abstract bool SavedButInconsistent();

		protected abstract void FetchInspectedToryValueSavedValue();

		void RestoreEditorGUI()
		{
			EditorGUI.indentLevel = cachedIndentLevel;
			EditorGUIUtility.labelWidth = cachedLabelWidth;
		}
	}
}