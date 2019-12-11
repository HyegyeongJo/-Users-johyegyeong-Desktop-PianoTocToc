using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToryValue
{
	[CustomPropertyDrawer(typeof(ToryRectInt))]
	public class ToryRectIntDrawer : ToryVector2And3Drawer<RectInt>
	{
		float fieldWidth;

		protected override ToryValue<RectInt> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryRectInt>(inspectedObject, serializedProperty);
		}

		protected override void PrepareDrawingRelativeProperties()
		{
			base.PrepareDrawingRelativeProperties();
			fieldWidth = (relativePropertiesPosition.width - labelWidth) * 0.25f;
		}

		protected override void DrawValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(1);
			Rect xPos = new Rect(pos.x, pos.y, fieldWidth, pos.height);
			Rect yPos = new Rect(pos.x + fieldWidth, pos.y, fieldWidth, pos.height);
			Rect zPos = new Rect(pos.x + fieldWidth * 2f, pos.y, fieldWidth, pos.height);
			Rect wPos = new Rect(pos.x + fieldWidth * 3f, pos.y, fieldWidth, pos.height);
			EditorGUI.PropertyField(xPos, valueProperty.FindPropertyRelative("x"), new GUIContent("X"));
			EditorGUI.PropertyField(yPos, valueProperty.FindPropertyRelative("y"), new GUIContent("Y"));
			EditorGUI.PropertyField(zPos, valueProperty.FindPropertyRelative("width"), new GUIContent("W"));
			EditorGUI.PropertyField(wPos, valueProperty.FindPropertyRelative("height"), new GUIContent("H"));
		}

		protected override void DrawDefaultValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(2);
			Rect xPos = new Rect(pos.x, pos.y, fieldWidth, pos.height);
			Rect yPos = new Rect(pos.x + fieldWidth, pos.y, fieldWidth, pos.height);
			Rect zPos = new Rect(pos.x + fieldWidth * 2f, pos.y, fieldWidth, pos.height);
			Rect wPos = new Rect(pos.x + fieldWidth * 3f, pos.y, fieldWidth, pos.height);
			EditorGUI.PropertyField(xPos, defaultValueProperty.FindPropertyRelative("x"), new GUIContent("X"));
			EditorGUI.PropertyField(yPos, defaultValueProperty.FindPropertyRelative("y"), new GUIContent("Y"));
			EditorGUI.PropertyField(zPos, defaultValueProperty.FindPropertyRelative("width"), new GUIContent("W"));
			EditorGUI.PropertyField(wPos, defaultValueProperty.FindPropertyRelative("height"), new GUIContent("H"));
		}

		protected override void DrawSavedValuePropertyField()
		{
			Rect pos = GetPropertyFieldPositionAt(3);
			Rect xPos = new Rect(pos.x, pos.y, fieldWidth, pos.height);
			Rect yPos = new Rect(pos.x + fieldWidth, pos.y, fieldWidth, pos.height);
			Rect zPos = new Rect(pos.x + fieldWidth * 2f, pos.y, fieldWidth, pos.height);
			Rect wPos = new Rect(pos.x + fieldWidth * 3f, pos.y, fieldWidth, pos.height);
			EditorGUI.PropertyField(xPos, savedValueProperty.FindPropertyRelative("x"), new GUIContent("X"));
			EditorGUI.PropertyField(yPos, savedValueProperty.FindPropertyRelative("y"), new GUIContent("Y"));
			EditorGUI.PropertyField(zPos, savedValueProperty.FindPropertyRelative("width"), new GUIContent("W"));
			EditorGUI.PropertyField(wPos, savedValueProperty.FindPropertyRelative("height"), new GUIContent("H"));
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<RectInt> toryValue)
		{
			toryValue.Value = valueProperty.rectIntValue;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(SerializedProperty serializedProperty, RectInt inspectedToryValue)
		{
			serializedProperty.rectIntValue = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<RectInt> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.rectIntValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<RectInt> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.rectIntValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (RectInt)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			Vector4 v = Rect2Vector4(savedValueProperty.rectIntValue);
			return (PlayerPrefs.HasKey(keyProperty.stringValue) &&
					PlayerPrefsElite.GetVector4(keyProperty.stringValue).Equals(v));
		}

		Vector4 Rect2Vector4(RectInt rect)
		{
			return new Vector4(rect.x, rect.y, rect.width, rect.height);
		}

		protected override void SaveInspectedToryValue(ToryValue<RectInt> toryValue)
		{
			Vector4 v = Rect2Vector4(savedValueProperty.rectIntValue);
			PlayerPrefsElite.SetVector4(toryValue.Key, v);
		}

		protected override bool SavedButInconsistent()
		{
			Vector4 v = Rect2Vector4(savedValueProperty.rectIntValue);
			return (PlayerPrefs.HasKey(keyProperty.stringValue) &&
					!PlayerPrefsElite.GetVector4(keyProperty.stringValue).Equals(v));
		}

		RectInt Vector42Rect(Vector4 v)
		{
			return new RectInt((int)v.x, (int)v.y, (int)v.z, (int)v.w);
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.rectIntValue = Vector42Rect(PlayerPrefsElite.GetVector4(keyProperty.stringValue));
		}
	}
}