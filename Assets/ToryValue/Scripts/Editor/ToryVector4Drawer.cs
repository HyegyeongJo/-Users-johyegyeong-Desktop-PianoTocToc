using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ToryValue
{
	[CustomPropertyDrawer(typeof(ToryVector4))]
	public class ToryVector4Drawer : ToryVector2And3Drawer<Vector4>
	{
		float fieldWidth;

		protected override ToryValue<Vector4> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryVector4>(inspectedObject, serializedProperty);
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
			EditorGUI.PropertyField(zPos, valueProperty.FindPropertyRelative("z"), new GUIContent("Z"));
			EditorGUI.PropertyField(wPos, valueProperty.FindPropertyRelative("w"), new GUIContent("W"));
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
			EditorGUI.PropertyField(zPos, defaultValueProperty.FindPropertyRelative("z"), new GUIContent("Z"));
			EditorGUI.PropertyField(wPos, defaultValueProperty.FindPropertyRelative("w"), new GUIContent("W"));
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
			EditorGUI.PropertyField(zPos, savedValueProperty.FindPropertyRelative("z"), new GUIContent("Z"));
			EditorGUI.PropertyField(wPos, savedValueProperty.FindPropertyRelative("w"), new GUIContent("W"));
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<Vector4> toryValue)
		{
			toryValue.Value = valueProperty.vector4Value;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(SerializedProperty serializedProperty, Vector4 inspectedToryValue)
		{
			serializedProperty.vector4Value = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<Vector4> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.vector4Value);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<Vector4> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.vector4Value);
			SyncSerializedPropertyWithValue(savedValueProperty, (Vector4)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetVector4(keyProperty.stringValue).Equals(savedValueProperty.vector4Value));
		}

		protected override void SaveInspectedToryValue(ToryValue<Vector4> toryValue)
		{
			PlayerPrefsElite.SetVector4(toryValue.Key, savedValueProperty.vector4Value);
		}

		protected override bool SavedButInconsistent()
		{
			return (PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetVector4(keyProperty.stringValue).Equals(savedValueProperty.vector4Value));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.vector4Value = PlayerPrefsElite.GetVector4(keyProperty.stringValue);
		}
	}
}