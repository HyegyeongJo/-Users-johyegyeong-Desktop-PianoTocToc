using System.Reflection;

namespace ToryValue 
{
	[UnityEditor.CustomPropertyDrawer(typeof(ToryBool))]
	public class ToryBoolDrawer : ToryValueDrawer<bool>
	{
		protected override ToryValue<bool> GetActualObject(object inspectedObject, UnityEditor.SerializedProperty serializedProperty)
		{
			return PropertyDrawerUtility.GetActualObject<ToryBool>(inspectedObject, serializedProperty);
		}

		protected override void ApplyChangeToInspectedToryValue(ToryValue<bool> toryValue)
		{
			toryValue.Value = valueProperty.boolValue;
			SyncSerializedPropertyWithValue(valueProperty, toryValue);
		}

		void SyncSerializedPropertyWithValue(UnityEditor.SerializedProperty serializedProperty, bool inspectedToryValue)
		{
			serializedProperty.boolValue = inspectedToryValue;
		}

		protected override void ApplyChangeToInspectedDefaultValue(ToryValue<bool> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("DefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, defaultValueProperty.boolValue);
			SyncSerializedPropertyWithValue(defaultValueProperty, toryValue.GetDefaultValue());
		}

		protected override void ApplyChangeToInspectedSavedValue(ToryValue<bool> toryValue)
		{
			PropertyInfo property = toryValue.GetType().GetProperty("SavedValue", BindingFlags.NonPublic | BindingFlags.Instance);
			property.SetValue(toryValue, savedValueProperty.boolValue);
			SyncSerializedPropertyWithValue(savedValueProperty, (bool)property.GetValue(toryValue));
		}

		protected override bool Saved()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        PlayerPrefsElite.GetBoolean(keyProperty.stringValue).Equals(savedValueProperty.boolValue));
		}

		protected override void SaveInspectedToryValue(ToryValue<bool> toryValue)
		{
			PlayerPrefsElite.SetBoolean(toryValue.Key, savedValueProperty.boolValue);
		}

		protected override bool SavedButInconsistent()
		{
			return (UnityEngine.PlayerPrefs.HasKey(keyProperty.stringValue) && 
			        !PlayerPrefsElite.GetBoolean(keyProperty.stringValue).Equals(savedValueProperty.boolValue));
		}

		protected override void FetchInspectedToryValueSavedValue()
		{
			savedValueProperty.boolValue = PlayerPrefsElite.GetBoolean(keyProperty.stringValue);
		}
	}
}
