using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryVector2))]
	public class ToryVector2Drawer : ToryVector2And3Drawer<UnityEngine.Vector2>
	{
		protected override ToryValue<UnityEngine.Vector2> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryVector2>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<UnityEngine.Vector2> toryValue)
		{
			toryValue.Value = valueProperty.vector2Value;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<UnityEngine.Vector2> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.vector2Value);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, UnityEngine.Vector2 inspectedToryValue)
		{
			serializedProperty.vector2Value = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<UnityEngine.Vector2> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.vector2Value);
			SyncSerializedPropertyWithValue(savedValueProperty, (UnityEngine.Vector2)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetVector2(keyProperty.stringValue).Equals(savedValueProperty.vector2Value));
		}

		protected override void SaveInspectedToryValue(ToryValue<UnityEngine.Vector2> toryValue)
		{
			PlayerPrefsElite.SetVector2(toryValue.Key, savedValueProperty.vector2Value);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetVector2(keyProperty.stringValue).Equals(savedValueProperty.vector2Value));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.vector2Value = PlayerPrefsElite.GetVector2(keyProperty.stringValue);
		}
	}
}
